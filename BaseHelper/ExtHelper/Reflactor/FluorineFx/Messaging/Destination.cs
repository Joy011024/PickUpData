namespace FluorineFx.Messaging
{
    using FluorineFx;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Services;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;

    [CLSCompliant(false)]
    public class Destination
    {
        protected FluorineFx.Messaging.Services.ServiceAdapter _adapter;
        private FactoryInstance _factoryInstance;
        protected IService _service;
        protected FluorineFx.Messaging.Config.DestinationSettings _settings;
        private static readonly ILog log = LogManager.GetLogger(typeof(Destination));

        private Destination()
        {
        }

        internal Destination(IService service, FluorineFx.Messaging.Config.DestinationSettings settings)
        {
            this._service = service;
            this._settings = settings;
        }

        internal virtual void Dump(DumpContext dumpContext)
        {
            dumpContext.AppendLine("Destination Id = " + this.Id);
        }

        public FactoryInstance GetFactoryInstance()
        {
            if (this._factoryInstance == null)
            {
                IFlexFactory factory = this.Service.GetMessageBroker().GetFactory(this.FactoryId);
                Hashtable properties = this._settings.Properties;
                this._factoryInstance = factory.CreateFactoryInstance(this.Id, properties);
            }
            return this._factoryInstance;
        }

        internal void Init(AdapterSettings adapterSettings)
        {
            if (adapterSettings != null)
            {
                Type type = ObjectFactory.Locate(adapterSettings.Class);
                if (type != null)
                {
                    this._adapter = ObjectFactory.CreateInstance(type) as FluorineFx.Messaging.Services.ServiceAdapter;
                    this._adapter.SetDestination(this);
                    this._adapter.SetAdapterSettings(adapterSettings);
                    this._adapter.SetDestinationSettings(this._settings);
                    this._adapter.Init();
                }
                else
                {
                    log.Error(__Res.GetString("Type_InitError", new object[] { adapterSettings.Class }));
                }
            }
            this.Service.GetMessageBroker().RegisterDestination(this, this._service);
            if (this.Scope == "application")
            {
                object obj2 = this.GetFactoryInstance().Lookup();
            }
        }

        public FluorineFx.Messaging.Config.DestinationSettings DestinationSettings
        {
            get
            {
                return this._settings;
            }
        }

        public string FactoryId
        {
            get
            {
                if (this._settings.Properties.Contains("factory"))
                {
                    return (this._settings.Properties["factory"] as string);
                }
                return "dotnet";
            }
        }

        public string Id
        {
            get
            {
                return this._settings.Id;
            }
        }

        public string Scope
        {
            get
            {
                if ((this._settings != null) && (this._settings.Properties != null))
                {
                    return (this._settings.Properties["scope"] as string);
                }
                return "request";
            }
        }

        public IService Service
        {
            get
            {
                return this._service;
            }
        }

        public FluorineFx.Messaging.Services.ServiceAdapter ServiceAdapter
        {
            get
            {
                return this._adapter;
            }
        }

        public string Source
        {
            get
            {
                if ((this._settings != null) && (this._settings.Properties != null))
                {
                    return (this._settings.Properties["source"] as string);
                }
                return null;
            }
        }
    }
}

