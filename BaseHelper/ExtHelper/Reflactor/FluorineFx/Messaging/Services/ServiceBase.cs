namespace FluorineFx.Messaging.Services
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Exceptions;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Messages;
    using System;
    using System.Collections;
    using System.Threading;

    [CLSCompliant(false)]
    public class ServiceBase : IService
    {
        protected Destination _defaultDestination;
        protected Hashtable _destinations;
        protected MessageBroker _messageBroker;
        private object _objLock;
        protected FluorineFx.Messaging.Config.ServiceSettings _serviceSettings;

        internal ServiceBase()
        {
            this._objLock = new object();
        }

        internal ServiceBase(MessageBroker messageBroker, FluorineFx.Messaging.Config.ServiceSettings serviceSettings)
        {
            this._objLock = new object();
            this._messageBroker = messageBroker;
            this._serviceSettings = serviceSettings;
            this._destinations = new Hashtable();
            foreach (DestinationSettings settings in serviceSettings.DestinationSettings)
            {
                this.CreateDestination(settings);
            }
        }

        public virtual void CheckSecurity(Destination destination)
        {
            if (destination == null)
            {
                throw new FluorineException(__Res.GetString("Invalid_Destination", new object[] { "null" }));
            }
            if (destination.DestinationSettings != null)
            {
                string[] roles = destination.DestinationSettings.GetRoles();
                if ((roles.Length > 0) && !this.DoAuthorization(roles))
                {
                    throw new UnauthorizedAccessException(__Res.GetString("Security_AccessNotAllowed"));
                }
            }
        }

        public virtual void CheckSecurity(IMessage message)
        {
            Destination destination = this.GetDestination(message);
            if (destination == null)
            {
                throw new FluorineException(__Res.GetString("Invalid_Destination", new object[] { message.destination }));
            }
            this.CheckSecurity(destination);
        }

        public virtual Destination CreateDestination(DestinationSettings destinationSettings)
        {
            lock (this._objLock)
            {
                if (!this._destinations.ContainsKey(destinationSettings.Id))
                {
                    Destination destination = this.NewDestination(destinationSettings);
                    if (destinationSettings.Adapter != null)
                    {
                        destination.Init(destinationSettings.Adapter);
                    }
                    else
                    {
                        destination.Init(this._serviceSettings.DefaultAdapter);
                    }
                    this._destinations[destination.Id] = destination;
                    string str = destination.DestinationSettings.Properties["source"] as string;
                    if ((str != null) && (str == "*"))
                    {
                        this._defaultDestination = destination;
                    }
                    return destination;
                }
                return (this._destinations[destinationSettings.Id] as Destination);
            }
        }

        public bool DoAuthorization(string[] roles)
        {
            if (Thread.CurrentPrincipal == null)
            {
                throw new UnauthorizedAccessException(__Res.GetString("Security_AccessNotAllowed"));
            }
            if (this._messageBroker == null)
            {
                throw new FluorineException(__Res.GetString("MessageBroker_NotAvailable"));
            }
            if (this._messageBroker.LoginCommand == null)
            {
                throw new UnauthorizedAccessException(__Res.GetString("Security_LoginMissing"));
            }
            return this._messageBroker.LoginCommand.DoAuthorization(Thread.CurrentPrincipal, roles);
        }

        public Destination GetDestination(IMessage message)
        {
            lock (this._objLock)
            {
                return (this._destinations[message.destination] as Destination);
            }
        }

        public Destination GetDestination(string id)
        {
            lock (this._objLock)
            {
                return (this._destinations[id] as Destination);
            }
        }

        public Destination[] GetDestinations()
        {
            lock (this._objLock)
            {
                ArrayList list = new ArrayList(this._destinations.Values);
                return (list.ToArray(typeof(Destination)) as Destination[]);
            }
        }

        public Destination GetDestinationWithSource(string source)
        {
            lock (this._objLock)
            {
                foreach (Destination destination in this._destinations.Values)
                {
                    string str = destination.DestinationSettings.Properties["source"] as string;
                    if (source == str)
                    {
                        return destination;
                    }
                }
                return null;
            }
        }

        public MessageBroker GetMessageBroker()
        {
            return this._messageBroker;
        }

        public bool IsSupportedMessage(IMessage message)
        {
            return this.IsSupportedMessageType(message.GetType().FullName);
        }

        public bool IsSupportedMessageType(string messageClassName)
        {
            bool flag = this._serviceSettings.SupportedMessageTypes.Contains(messageClassName);
            if (!flag)
            {
                string customClass = FluorineConfiguration.Instance.ClassMappings.GetCustomClass(messageClassName);
                return this._serviceSettings.SupportedMessageTypes.Contains(customClass);
            }
            return flag;
        }

        protected virtual Destination NewDestination(DestinationSettings destinationSettings)
        {
            return new Destination(this, destinationSettings);
        }

        public virtual object ServiceMessage(IMessage message)
        {
            CommandMessage message2 = message as CommandMessage;
            if ((message2 == null) || (message2.operation != 5))
            {
                throw new NotSupportedException();
            }
            return true;
        }

        public virtual void Start()
        {
        }

        public virtual void Stop()
        {
        }

        public Destination DefaultDestination
        {
            get
            {
                return this._defaultDestination;
            }
        }

        public string id
        {
            get
            {
                return this._serviceSettings.Id;
            }
        }

        public FluorineFx.Messaging.Config.ServiceSettings ServiceSettings
        {
            get
            {
                return this._serviceSettings;
            }
        }
    }
}

