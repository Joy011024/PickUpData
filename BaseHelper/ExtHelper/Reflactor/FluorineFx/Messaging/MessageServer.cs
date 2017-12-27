namespace FluorineFx.Messaging
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Context;
    using FluorineFx.Exceptions;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Messaging.Services;
    using FluorineFx.Security;
    using FluorineFx.Silverlight;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.IO;
    using System.Web;

    public sealed class MessageServer : DisposableBase
    {
        private FluorineFx.Messaging.MessageBroker _messageBroker;
        private PolicyServer _policyServer;
        private FluorineFx.Messaging.Config.ServiceConfigSettings _serviceConfigSettings;
        private static readonly ILog log = LogManager.GetLogger(typeof(MessageServer));

        protected override void Free()
        {
            if (this._messageBroker != null)
            {
                this.Stop();
            }
        }

        protected override void FreeUnmanaged()
        {
            if (this._messageBroker != null)
            {
                this.Stop();
            }
        }

        public void Init(string configPath)
        {
            this.Init(configPath, false);
        }

        public void Init(string configPath, bool serviceBrowserAvailable)
        {
            Type type;
            this._messageBroker = new FluorineFx.Messaging.MessageBroker(this);
            this._serviceConfigSettings = FluorineFx.Messaging.Config.ServiceConfigSettings.Load(configPath, "services-config.xml");
            foreach (ChannelSettings settings in this._serviceConfigSettings.ChannelsSettings)
            {
                type = ObjectFactory.Locate(settings.Class);
                if (type != null)
                {
                    IEndpoint endpoint = ObjectFactory.CreateInstance(type, new object[] { this._messageBroker, settings }) as IEndpoint;
                    if (endpoint != null)
                    {
                        this._messageBroker.AddEndpoint(endpoint);
                    }
                }
                else
                {
                    log.Error(__Res.GetString("Type_InitError", new object[] { settings.Class }));
                }
                ChannelSettings channelSettings = new ChannelSettings("__@fluorinertmpt", null);
                IEndpoint endpoint2 = new RtmptEndpoint(this._messageBroker, channelSettings);
                this._messageBroker.AddEndpoint(endpoint2);
            }
            foreach (FactorySettings settings3 in this._serviceConfigSettings.FactoriesSettings)
            {
                type = ObjectFactory.Locate(settings3.ClassId);
                if (type != null)
                {
                    IFlexFactory factory = ObjectFactory.CreateInstance(type, new object[0]) as IFlexFactory;
                    if (factory != null)
                    {
                        this._messageBroker.AddFactory(settings3.Id, factory);
                    }
                }
                else
                {
                    log.Error(__Res.GetString("Type_InitError", new object[] { settings3.ClassId }));
                }
            }
            this._messageBroker.AddFactory("dotnet", new DotNetFactory());
            if (serviceBrowserAvailable && (this._serviceConfigSettings.ServiceSettings["remoting-service"] != null))
            {
                ServiceSettings serviceSettings = this._serviceConfigSettings.ServiceSettings["remoting-service"];
                AdapterSettings adapterSettings = this._serviceConfigSettings.ServiceSettings["remoting-service"].AdapterSettings["dotnet"];
                this.InstallServiceBrowserDestinations(serviceSettings, adapterSettings);
            }
            foreach (ServiceSettings settings6 in this._serviceConfigSettings.ServiceSettings)
            {
                type = ObjectFactory.Locate(settings6.Class);
                if (type != null)
                {
                    IService service = ObjectFactory.CreateInstance(type, new object[] { this._messageBroker, settings6 }) as IService;
                    if (service != null)
                    {
                        this._messageBroker.AddService(service);
                    }
                }
                else
                {
                    log.Error(__Res.GetString("Type_InitError", new object[] { settings6.Class }));
                }
            }
            if ((this._serviceConfigSettings.SecuritySettings != null) && ((this._serviceConfigSettings.SecuritySettings.LoginCommands != null) && (this._serviceConfigSettings.SecuritySettings.LoginCommands.Count > 0)))
            {
                string loginCommand = this._serviceConfigSettings.SecuritySettings.LoginCommands.GetLoginCommand("asp.net");
                type = ObjectFactory.Locate(loginCommand);
                if (type != null)
                {
                    ILoginCommand command = ObjectFactory.CreateInstance(type, new object[0]) as ILoginCommand;
                    this._messageBroker.LoginCommand = command;
                }
                else
                {
                    log.Error(__Res.GetString("Type_InitError", new object[] { loginCommand }));
                }
            }
            this.InitAuthenticationService();
            try
            {
                if ((FluorineConfiguration.Instance.FluorineSettings.Silverlight.PolicyServerSettings != null) && FluorineConfiguration.Instance.FluorineSettings.Silverlight.PolicyServerSettings.Enable)
                {
                    IResource resource = FluorineContext.Current.GetResource(FluorineConfiguration.Instance.FluorineSettings.Silverlight.PolicyServerSettings.PolicyFile);
                    if (!resource.Exists)
                    {
                        throw new FileNotFoundException("Policy file not found", FluorineConfiguration.Instance.FluorineSettings.Silverlight.PolicyServerSettings.PolicyFile);
                    }
                    log.Info(__Res.GetString("Silverlight_StartPS", new object[] { resource.File.FullName }));
                    this._policyServer = new PolicyServer(resource.File.FullName);
                }
            }
            catch (Exception exception)
            {
                log.Error(__Res.GetString("Silverlight_PSError"), exception);
            }
        }

        private void InitAuthenticationService()
        {
            ServiceSettings settings = new ServiceSettings(this._serviceConfigSettings, "authentication-service", typeof(AuthenticationService).FullName);
            string customClass = "flex.messaging.messages.AuthenticationMessage";
            string type = FluorineConfiguration.Instance.ClassMappings.GetType(customClass);
            settings.SupportedMessageTypes[customClass] = type;
            this._serviceConfigSettings.ServiceSettings.Add(settings);
            AuthenticationService service = new AuthenticationService(this._messageBroker, settings);
            this._messageBroker.AddService(service);
        }

        private void InstallServiceBrowserDestinations(ServiceSettings serviceSettings, AdapterSettings adapterSettings)
        {
            DestinationSettings settings = new DestinationSettings(serviceSettings, "FluorineFx.ServiceBrowser.FluorineServiceBrowser", adapterSettings, "FluorineFx.ServiceBrowser.FluorineServiceBrowser");
            serviceSettings.DestinationSettings.Add(settings);
            settings = new DestinationSettings(serviceSettings, "FluorineFx.ServiceBrowser.ManagementService", adapterSettings, "FluorineFx.ServiceBrowser.ManagementService");
            serviceSettings.DestinationSettings.Add(settings);
            settings = new DestinationSettings(serviceSettings, "FluorineFx.ServiceBrowser.CodeGeneratorService", adapterSettings, "FluorineFx.ServiceBrowser.CodeGeneratorService");
            serviceSettings.DestinationSettings.Add(settings);
            settings = new DestinationSettings(serviceSettings, "FluorineFx.ServiceBrowser.SqlService", adapterSettings, "FluorineFx.ServiceBrowser.SqlService");
            serviceSettings.DestinationSettings.Add(settings);
        }

        public void Service()
        {
            string str;
            if (this._messageBroker == null)
            {
                str = __Res.GetString("MessageBroker_NotAvailable");
                log.Fatal(str);
                throw new FluorineException(str);
            }
            string applicationPath = HttpContext.Current.Request.ApplicationPath;
            string path = HttpContext.Current.Request.Path;
            bool isSecureConnection = HttpContext.Current.Request.IsSecureConnection;
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("Endpoint_Bind", new object[] { path, applicationPath }));
            }
            IEndpoint endpoint = this._messageBroker.GetEndpoint(path, applicationPath, isSecureConnection);
            if (endpoint != null)
            {
                endpoint.Service();
            }
            else
            {
                str = __Res.GetString("Endpoint_BindFail", new object[] { path, applicationPath });
                log.Fatal(str);
                this._messageBroker.TraceChannelSettings();
                throw new FluorineException(str);
            }
        }

        public void ServiceRtmpt()
        {
            IEndpoint endpoint = this._messageBroker.GetEndpoint("__@fluorinertmpt");
            if (endpoint != null)
            {
                endpoint.Service();
            }
            else
            {
                string message = __Res.GetString("Endpoint_BindFail", new object[] { "__@fluorinertmpt", "" });
                log.Fatal(message);
                this._messageBroker.TraceChannelSettings();
                throw new FluorineException(message);
            }
        }

        public void Start()
        {
            if (log.get_IsInfoEnabled())
            {
                log.Info(__Res.GetString("MessageServer_Start"));
            }
            if (this._messageBroker != null)
            {
                this._messageBroker.Start();
            }
            else
            {
                log.Error(__Res.GetString("MessageServer_StartError"));
            }
        }

        public void Stop()
        {
            if (this._messageBroker != null)
            {
                if (log.get_IsInfoEnabled())
                {
                    log.Info(__Res.GetString("MessageServer_Stop"));
                }
                if (this._messageBroker != null)
                {
                    this._messageBroker.Stop();
                    this._messageBroker = null;
                }
                if (this._policyServer != null)
                {
                    this._policyServer.Close();
                    this._policyServer = null;
                }
            }
        }

        public FluorineFx.Messaging.MessageBroker MessageBroker
        {
            get
            {
                return this._messageBroker;
            }
        }

        internal FluorineFx.Messaging.Config.ServiceConfigSettings ServiceConfigSettings
        {
            get
            {
                return this._serviceConfigSettings;
            }
        }
    }
}

