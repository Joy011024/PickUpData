namespace FluorineFx.Messaging.Config
{
    using FluorineFx.Configuration;
    using FluorineFx.Remoting;
    using System;
    using System.Collections;
    using System.Xml;

    public sealed class ServiceSettings
    {
        private AdapterSettingsCollection _adapterSettings;
        private string _class;
        private FluorineFx.Messaging.Config.AdapterSettings _defaultAdapterSettings;
        private DestinationSettingsCollection _destinationSettings;
        private string _id;
        private object _objLock;
        private FluorineFx.Messaging.Config.ServiceConfigSettings _serviceConfigSettings;
        private Hashtable _supportedMessageTypes;

        internal ServiceSettings(FluorineFx.Messaging.Config.ServiceConfigSettings serviceConfigSettings)
        {
            this._objLock = new object();
            this._serviceConfigSettings = serviceConfigSettings;
            this._supportedMessageTypes = new Hashtable(1);
            this._destinationSettings = new DestinationSettingsCollection();
            this._adapterSettings = new AdapterSettingsCollection();
        }

        internal ServiceSettings(FluorineFx.Messaging.Config.ServiceConfigSettings serviceConfigSettings, string id, string @class)
        {
            this._objLock = new object();
            this._serviceConfigSettings = serviceConfigSettings;
            this._supportedMessageTypes = new Hashtable(1);
            this._destinationSettings = new DestinationSettingsCollection();
            this._adapterSettings = new AdapterSettingsCollection();
            this._id = id;
            this._class = @class;
        }

        internal FluorineFx.Messaging.Config.DestinationSettings CreateDestinationSettings(string id, string source)
        {
            lock (this._objLock)
            {
                if (!this.DestinationSettings.ContainsKey(id))
                {
                    FluorineFx.Messaging.Config.AdapterSettings adapter = new FluorineFx.Messaging.Config.AdapterSettings("dotnet", typeof(RemotingAdapter).FullName, false);
                    FluorineFx.Messaging.Config.DestinationSettings settings2 = new FluorineFx.Messaging.Config.DestinationSettings(this, id, adapter, source);
                    this.DestinationSettings.Add(settings2);
                    return settings2;
                }
                return this.DestinationSettings[id];
            }
        }

        internal void Init(string configPath)
        {
            XmlDocument document = new XmlDocument();
            document.Load(configPath);
            XmlElement documentElement = document.DocumentElement;
            this.Init(documentElement);
        }

        internal void Init(XmlNode serviceElement)
        {
            FluorineFx.Messaging.Config.AdapterSettings settings;
            this._id = serviceElement.Attributes["id"].Value;
            this._class = serviceElement.Attributes["class"].Value;
            string[] strArray = serviceElement.Attributes["messageTypes"].Value.Split(new char[] { ',' });
            foreach (string str2 in strArray)
            {
                string type = FluorineConfiguration.Instance.ClassMappings.GetType(str2);
                this._supportedMessageTypes[str2] = type;
            }
            XmlNode node = serviceElement.SelectSingleNode("adapters");
            if (node != null)
            {
                foreach (XmlNode node2 in node.SelectNodes("*"))
                {
                    settings = new FluorineFx.Messaging.Config.AdapterSettings(node2);
                    this._adapterSettings.Add(settings);
                    if (settings.Default)
                    {
                        this._defaultAdapterSettings = settings;
                    }
                }
            }
            else
            {
                settings = new FluorineFx.Messaging.Config.AdapterSettings("dotnet", typeof(RemotingAdapter).FullName, true);
                this._defaultAdapterSettings = settings;
                this._adapterSettings.Add(settings);
            }
            XmlNodeList list = serviceElement.SelectNodes("destination");
            foreach (XmlNode node3 in list)
            {
                FluorineFx.Messaging.Config.DestinationSettings settings2 = new FluorineFx.Messaging.Config.DestinationSettings(this, node3);
                this.DestinationSettings.Add(settings2);
            }
        }

        public AdapterSettingsCollection AdapterSettings
        {
            get
            {
                return this._adapterSettings;
            }
        }

        public string Class
        {
            get
            {
                return this._class;
            }
        }

        public FluorineFx.Messaging.Config.AdapterSettings DefaultAdapter
        {
            get
            {
                return this._defaultAdapterSettings;
            }
            set
            {
                this._defaultAdapterSettings = value;
            }
        }

        public DestinationSettingsCollection DestinationSettings
        {
            get
            {
                return this._destinationSettings;
            }
        }

        public string Id
        {
            get
            {
                return this._id;
            }
        }

        public FluorineFx.Messaging.Config.ServiceConfigSettings ServiceConfigSettings
        {
            get
            {
                return this._serviceConfigSettings;
            }
        }

        public Hashtable SupportedMessageTypes
        {
            get
            {
                return this._supportedMessageTypes;
            }
        }
    }
}

