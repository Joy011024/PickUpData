namespace FluorineFx.Messaging
{
    using FluorineFx;
    using FluorineFx.Context;
    using FluorineFx.Exceptions;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmp;
    using FluorineFx.Messaging.Rtmp.Service;
    using FluorineFx.Messaging.Services;
    using FluorineFx.Security;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.Security;

    public class MessageBroker
    {
        private ClientManager _clientManager;
        private Hashtable _destinations;
        private Hashtable _destinationServiceMap;
        private Hashtable _endpoints;
        private Hashtable _factories;
        private FluorineFx.Messaging.GlobalScope _globalScope;
        private ILoginCommand _loginCommand;
        private string _messageBrokerId;
        private static Hashtable _messageBrokers = new Hashtable(1);
        private FluorineFx.Messaging.MessageServer _messageServer;
        private Hashtable _services;
        private static object _syncLock = new object();
        public static string DefaultMessageBrokerId = "default";
        private static readonly ILog log = LogManager.GetLogger(typeof(MessageBroker));

        public MessageBroker(FluorineFx.Messaging.MessageServer messageServer)
        {
            this._messageServer = messageServer;
            this._services = new Hashtable();
            this._endpoints = new Hashtable();
            this._factories = new Hashtable();
            this._destinationServiceMap = new Hashtable();
            this._destinations = new Hashtable();
            this._clientManager = new ClientManager(this);
        }

        internal void AddEndpoint(IEndpoint endpoint)
        {
            this._endpoints[endpoint.Id] = endpoint;
        }

        internal void AddFactory(string id, IFlexFactory factory)
        {
            this._factories.Add(id, factory);
        }

        internal void AddService(IService service)
        {
            this._services[service.id] = service;
        }

        internal IClient GetClient(string id)
        {
            return this._clientManager.LookupClient(id);
        }

        [CLSCompliant(false)]
        public Destination GetDestination(string destinationId)
        {
            foreach (DictionaryEntry entry in this._destinations)
            {
                Destination destination = entry.Value as Destination;
                if (destination.Id == destinationId)
                {
                    return destination;
                }
            }
            return null;
        }

        public string GetDestinationBySource(string source)
        {
            foreach (DictionaryEntry entry in this._destinations)
            {
                Destination destination = entry.Value as Destination;
                if (destination.Source == source)
                {
                    return destination.Id;
                }
            }
            return null;
        }

        public string GetDestinationId(IMessage message)
        {
            if (message.destination != null)
            {
                return message.destination;
            }
            if (message is RemotingMessage)
            {
                RemotingMessage message2 = message as RemotingMessage;
                string destinationBySource = this.GetDestinationBySource(message2.source);
                if (destinationBySource != null)
                {
                    return destinationBySource;
                }
                Destination destination = null;
                foreach (DictionaryEntry entry in this._services)
                {
                    IService service = entry.Value as IService;
                    if (service.IsSupportedMessage(message))
                    {
                        Destination[] destinations = service.GetDestinations();
                        foreach (Destination destination2 in destinations)
                        {
                            if (destination2.Source == message2.source)
                            {
                                return destination2.Id;
                            }
                            if (destination2.Source == "*")
                            {
                                destination = destination2;
                            }
                        }
                    }
                }
                if (destination != null)
                {
                    return destination.Id;
                }
            }
            return null;
        }

        public string GetDestinationId(string source)
        {
            ValidationUtils.ArgumentNotNullOrEmpty(source, "source");
            string destinationBySource = this.GetDestinationBySource(source);
            if (destinationBySource != null)
            {
                return destinationBySource;
            }
            Destination destination = null;
            foreach (DictionaryEntry entry in this._services)
            {
                IService service = entry.Value as IService;
                if (service.IsSupportedMessageType("flex.messaging.messages.RemotingMessage"))
                {
                    Destination[] destinations = service.GetDestinations();
                    foreach (Destination destination2 in destinations)
                    {
                        if (destination2.Source == source)
                        {
                            return destination2.Id;
                        }
                        if (destination2.Source == "*")
                        {
                            destination = destination2;
                        }
                    }
                }
            }
            if (destination != null)
            {
                return destination.Id;
            }
            return null;
        }

        internal IEndpoint GetEndpoint(string endpointId)
        {
            foreach (DictionaryEntry entry in this._endpoints)
            {
                IEndpoint endpoint = entry.Value as IEndpoint;
                if (endpoint.Id == endpointId)
                {
                    return endpoint;
                }
            }
            return null;
        }

        internal IEndpoint GetEndpoint(string path, string contextPath, bool secure)
        {
            foreach (DictionaryEntry entry in this._endpoints)
            {
                IEndpoint endpoint = entry.Value as IEndpoint;
                ChannelSettings settings = endpoint.GetSettings();
                if ((settings != null) && settings.Bind(path, contextPath))
                {
                    return endpoint;
                }
            }
            return null;
        }

        public IFlexFactory GetFactory(string id)
        {
            return (this._factories[id] as IFlexFactory);
        }

        public static MessageBroker GetMessageBroker(string messageBrokerId)
        {
            lock (_syncLock)
            {
                if (messageBrokerId == null)
                {
                    messageBrokerId = DefaultMessageBrokerId;
                }
                return (_messageBrokers[messageBrokerId] as MessageBroker);
            }
        }

        internal IService GetService(IMessage message)
        {
            IService serviceByDestinationId = this.GetServiceByDestinationId(message.destination);
            if (serviceByDestinationId == null)
            {
                CommandMessage message2 = message as CommandMessage;
                if (message2 == null)
                {
                    return serviceByDestinationId;
                }
                if (message2.messageRefType == null)
                {
                    return serviceByDestinationId;
                }
                foreach (DictionaryEntry entry in this._services)
                {
                    IService service2 = entry.Value as IService;
                    if (service2.IsSupportedMessageType(message2.messageRefType))
                    {
                        return service2;
                    }
                }
            }
            return serviceByDestinationId;
        }

        internal IService GetService(string id)
        {
            return (this._services[id] as IService);
        }

        internal IService GetServiceByDestinationId(string destinationId)
        {
            if (destinationId != null)
            {
                string str = this._destinationServiceMap[destinationId] as string;
                if (str != null)
                {
                    return (this._services[str] as IService);
                }
            }
            return null;
        }

        internal IService GetServiceByMessageType(string messageRef)
        {
            if (messageRef != null)
            {
                foreach (DictionaryEntry entry in this._services)
                {
                    IService service = entry.Value as IService;
                    if (service.IsSupportedMessageType(messageRef))
                    {
                        return service;
                    }
                }
            }
            return null;
        }

        internal void RegisterDestination(Destination destination, IService service)
        {
            this._destinationServiceMap[destination.Id] = service.id;
            this._destinations[destination.Id] = destination;
        }

        protected void RegisterMessageBroker()
        {
            if (this._messageBrokerId == null)
            {
                this._messageBrokerId = DefaultMessageBrokerId;
            }
            lock (_syncLock)
            {
                if (_messageBrokers.ContainsKey(this._messageBrokerId))
                {
                    throw new FluorineException(__Res.GetString("MessageBroker_RegisterError", new object[] { this._messageBrokerId }));
                }
                _messageBrokers[this._messageBrokerId] = this;
            }
        }

        public IMessage RouteMessage(IMessage message)
        {
            return this.RouteMessage(message, null);
        }

        internal IMessage RouteMessage(IMessage message, IEndpoint endpoint)
        {
            IService service = null;
            object obj2 = null;
            IMessage errorMessage = null;
            Exception exception2;
            CommandMessage message3 = message as CommandMessage;
            if ((message3 != null) && ((message3.operation == 8) || (message3.operation == 9)))
            {
                log.Debug(string.Format("Routing CommandMessage operation = {0}", message3.operation));
                try
                {
                    errorMessage = this.GetService("authentication-service").ServiceMessage(message3) as IMessage;
                }
                catch (SecurityException exception)
                {
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(exception.Message);
                    }
                    errorMessage = ErrorMessage.GetErrorMessage(message, exception);
                }
                catch (Exception exception4)
                {
                    exception2 = exception4;
                    if (log.get_IsErrorEnabled())
                    {
                        log.Error(__Res.GetString("MessageBroker_RoutingError"), exception2);
                    }
                    errorMessage = ErrorMessage.GetErrorMessage(message, exception2);
                }
            }
            else if ((message3 != null) && (message3.operation == 5))
            {
                log.Debug("Routing CommandMessage ping");
                errorMessage = new AcknowledgeMessage {
                    body = true
                };
            }
            else
            {
                log.Debug(string.Format("Routing message {0}", message.GetType().Name));
                if (FluorineContext.Current != null)
                {
                    FluorineContext.Current.RestorePrincipal(this.LoginCommand);
                }
                service = this.GetService(message);
                if (service != null)
                {
                    try
                    {
                        service.CheckSecurity(message);
                        obj2 = service.ServiceMessage(message);
                    }
                    catch (UnauthorizedAccessException exception3)
                    {
                        if (log.get_IsDebugEnabled())
                        {
                            log.Debug(exception3.Message);
                        }
                        obj2 = ErrorMessage.GetErrorMessage(message, exception3);
                    }
                    catch (Exception exception6)
                    {
                        exception2 = exception6;
                        if (log.get_IsErrorEnabled())
                        {
                            log.Error(exception2.Message, exception2);
                        }
                        obj2 = ErrorMessage.GetErrorMessage(message, exception2);
                    }
                }
                else
                {
                    string str = __Res.GetString("Destination_NotFound", new object[] { message.destination });
                    if (log.get_IsErrorEnabled())
                    {
                        log.Error(str);
                    }
                    obj2 = ErrorMessage.GetErrorMessage(message, new FluorineException(str));
                }
                if (obj2 is IMessage)
                {
                    errorMessage = obj2 as IMessage;
                }
                else
                {
                    errorMessage = new AcknowledgeMessage {
                        body = obj2
                    };
                }
            }
            if (errorMessage is AsyncMessage)
            {
                ((AsyncMessage) errorMessage).correlationId = message.messageId;
            }
            errorMessage.destination = message.destination;
            errorMessage.clientId = message.clientId;
            if (message.HeaderExists("amf_server_debug"))
            {
                log.Debug("MessageBroker processing debug header");
                ArrayList traceStack = NetDebug.GetTraceStack();
                errorMessage.SetHeader("amf_server_debug", traceStack.ToArray(typeof(object)) as object[]);
                NetDebug.Clear();
            }
            if ((FluorineContext.Current != null) && (FluorineContext.Current.Client != null))
            {
                errorMessage.SetHeader("DSId", FluorineContext.Current.Client.Id);
            }
            return errorMessage;
        }

        public void Start()
        {
            this.RegisterMessageBroker();
            this._globalScope = new FluorineFx.Messaging.GlobalScope();
            this._globalScope.Name = "default";
            ScopeResolver scopeResolver = new ScopeResolver(this._globalScope);
            IClientRegistry clientRegistry = this._clientManager;
            ServiceInvoker serviceInvoker = new ServiceInvoker();
            ScopeContext context = new ScopeContext("/", clientRegistry, scopeResolver, serviceInvoker, null);
            CoreHandler handler = new CoreHandler();
            this._globalScope.Context = context;
            this._globalScope.Handler = handler;
            this._globalScope.Register();
            this.StartServices();
            this.StartEndpoints();
        }

        internal void StartEndpoints()
        {
            foreach (DictionaryEntry entry in this._endpoints)
            {
                (entry.Value as IEndpoint).Start();
            }
        }

        internal void StartServices()
        {
            foreach (DictionaryEntry entry in this._services)
            {
                (entry.Value as IService).Start();
            }
        }

        public void Stop()
        {
            this.StopServices();
            this.StopEndpoints();
            if (this._globalScope != null)
            {
                this._globalScope.Stop();
                this._globalScope.Dispose();
                this._globalScope = null;
            }
            this.UnregisterMessageBroker();
        }

        internal void StopEndpoints()
        {
            foreach (DictionaryEntry entry in this._endpoints)
            {
                (entry.Value as IEndpoint).Stop();
            }
        }

        internal void StopServices()
        {
            foreach (DictionaryEntry entry in this._services)
            {
                (entry.Value as IService).Stop();
            }
        }

        internal void TraceChannelSettings()
        {
            foreach (DictionaryEntry entry in this._endpoints)
            {
                ChannelSettings settings = (entry.Value as IEndpoint).GetSettings();
                log.Debug(settings.ToString());
            }
        }

        protected void UnregisterMessageBroker()
        {
            if (this._messageBrokerId == null)
            {
                this._messageBrokerId = DefaultMessageBrokerId;
            }
            lock (_syncLock)
            {
                _messageBrokers.Remove(this._messageBrokerId);
            }
        }

        internal IClientRegistry ClientRegistry
        {
            get
            {
                return this._clientManager;
            }
        }

        internal FluorineFx.Messaging.Config.FlexClientSettings FlexClientSettings
        {
            get
            {
                return this._messageServer.ServiceConfigSettings.FlexClientSettings;
            }
        }

        public IGlobalScope GlobalScope
        {
            get
            {
                return this._globalScope;
            }
        }

        public string Id
        {
            get
            {
                return this._messageBrokerId;
            }
        }

        internal ILoginCommand LoginCommand
        {
            get
            {
                return this._loginCommand;
            }
            set
            {
                this._loginCommand = value;
            }
        }

        internal FluorineFx.Messaging.MessageServer MessageServer
        {
            get
            {
                return this._messageServer;
            }
        }

        public object SyncRoot
        {
            get
            {
                return _syncLock;
            }
        }
    }
}

