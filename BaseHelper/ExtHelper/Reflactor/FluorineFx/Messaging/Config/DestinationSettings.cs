namespace FluorineFx.Messaging.Config
{
    using System;
    using System.Collections;
    using System.Xml;

    public sealed class DestinationSettings : Hashtable
    {
        private AdapterSettings _adapter;
        private string[] _cachedRoles;
        private ChannelSettingsCollection _channels;
        private string _id;
        private FluorineFx.Messaging.Config.MetadataSettings _metadata;
        private FluorineFx.Messaging.Config.MsmqSettings _msmqSettings;
        private FluorineFx.Messaging.Config.NetworkSettings _network;
        private Hashtable _properties;
        private XmlNode _propertiesNode;
        private FluorineFx.Messaging.Config.SecuritySettings _security;
        private FluorineFx.Messaging.Config.ServerSettings _server;
        private FluorineFx.Messaging.Config.ServiceSettings _serviceSettings;
        public const string FluorineCodeGeneratorDestination = "FluorineFx.ServiceBrowser.CodeGeneratorService";
        public const string FluorineDestination = "fluorine";
        public const string FluorineManagementDestination = "FluorineFx.ServiceBrowser.ManagementService";
        public const string FluorineServiceBrowserDestination = "FluorineFx.ServiceBrowser.FluorineServiceBrowser";
        public const string FluorineSqlServiceDestination = "FluorineFx.ServiceBrowser.SqlService";

        internal DestinationSettings(FluorineFx.Messaging.Config.ServiceSettings serviceSettings, XmlNode destinationNode)
        {
            this._serviceSettings = serviceSettings;
            this._properties = new Hashtable();
            this._channels = new ChannelSettingsCollection();
            this._id = destinationNode.Attributes["id"].Value;
            XmlNode node = destinationNode.SelectSingleNode("adapter");
            if (node != null)
            {
                string str = node.Attributes["ref"].Value;
                AdapterSettings settings = serviceSettings.AdapterSettings[str];
                this._adapter = settings;
            }
            this._propertiesNode = destinationNode.SelectSingleNode("properties");
            if (this._propertiesNode != null)
            {
                XmlNode node2 = this._propertiesNode.SelectSingleNode("source");
                if (node2 != null)
                {
                    this._properties["source"] = node2.InnerXml;
                }
                XmlNode node3 = this._propertiesNode.SelectSingleNode("factory");
                if (node3 != null)
                {
                    this._properties["factory"] = node3.InnerXml;
                }
                XmlNode node4 = this._propertiesNode.SelectSingleNode("attribute-id");
                if (node4 != null)
                {
                    this._properties["attribute-id"] = node4.InnerXml;
                }
                else
                {
                    this._properties["attribute-id"] = this._id;
                }
                XmlNode node5 = this._propertiesNode.SelectSingleNode("scope");
                if (node5 != null)
                {
                    this._properties["scope"] = node5.InnerXml;
                }
                XmlNode networkDefinitionNode = this._propertiesNode.SelectSingleNode("network");
                if (networkDefinitionNode != null)
                {
                    FluorineFx.Messaging.Config.NetworkSettings settings2 = new FluorineFx.Messaging.Config.NetworkSettings(networkDefinitionNode);
                    this._network = settings2;
                }
                XmlNode metadataDefinitionNode = this._propertiesNode.SelectSingleNode("metadata");
                if (metadataDefinitionNode != null)
                {
                    FluorineFx.Messaging.Config.MetadataSettings settings3 = new FluorineFx.Messaging.Config.MetadataSettings(metadataDefinitionNode);
                    this._metadata = settings3;
                }
                XmlNode severDefinitionNode = this._propertiesNode.SelectSingleNode("server");
                if (severDefinitionNode != null)
                {
                    FluorineFx.Messaging.Config.ServerSettings settings4 = new FluorineFx.Messaging.Config.ServerSettings(severDefinitionNode);
                    this._server = settings4;
                }
                XmlNode msmqDefinitionNode = this._propertiesNode.SelectSingleNode("msmq");
                if (msmqDefinitionNode != null)
                {
                    FluorineFx.Messaging.Config.MsmqSettings settings5 = new FluorineFx.Messaging.Config.MsmqSettings(msmqDefinitionNode);
                    this._msmqSettings = settings5;
                }
            }
            XmlNode securityNode = destinationNode.SelectSingleNode("security");
            if (securityNode != null)
            {
                FluorineFx.Messaging.Config.SecuritySettings settings6 = new FluorineFx.Messaging.Config.SecuritySettings(this, securityNode);
                this._security = settings6;
            }
            else
            {
                this._security = new FluorineFx.Messaging.Config.SecuritySettings(this);
            }
            XmlNode node11 = destinationNode.SelectSingleNode("channels");
            if (node11 != null)
            {
                XmlNodeList list = node11.SelectNodes("channel");
                foreach (XmlNode node12 in list)
                {
                    ChannelSettings settings7;
                    string str2 = node12.Attributes["ref"].Value;
                    if (str2 != null)
                    {
                        settings7 = this._serviceSettings.ServiceConfigSettings.ChannelsSettings[str2];
                        this._channels.Add(settings7);
                    }
                    else
                    {
                        settings7 = new ChannelSettings(node12);
                        this._channels.Add(settings7);
                    }
                }
            }
        }

        internal DestinationSettings(FluorineFx.Messaging.Config.ServiceSettings serviceSettings, string id, AdapterSettings adapter, string source)
        {
            this._serviceSettings = serviceSettings;
            this._properties = new Hashtable();
            this._channels = new ChannelSettingsCollection();
            this._id = id;
            this._adapter = adapter;
            this._properties["source"] = source;
        }

        public string[] GetRoles()
        {
            if (this._cachedRoles == null)
            {
                if (this.SecuritySettings != null)
                {
                    this._cachedRoles = this.SecuritySettings.GetRoles();
                }
                else
                {
                    this._cachedRoles = new string[0];
                }
            }
            return this._cachedRoles;
        }

        public AdapterSettings Adapter
        {
            get
            {
                return this._adapter;
            }
        }

        public ChannelSettingsCollection Channels
        {
            get
            {
                return this._channels;
            }
        }

        public string Id
        {
            get
            {
                return this._id;
            }
        }

        public FluorineFx.Messaging.Config.MetadataSettings MetadataSettings
        {
            get
            {
                return this._metadata;
            }
        }

        public FluorineFx.Messaging.Config.MsmqSettings MsmqSettings
        {
            get
            {
                return this._msmqSettings;
            }
        }

        public FluorineFx.Messaging.Config.NetworkSettings NetworkSettings
        {
            get
            {
                return this._network;
            }
        }

        public Hashtable Properties
        {
            get
            {
                return this._properties;
            }
        }

        public XmlNode PropertiesNode
        {
            get
            {
                return this._propertiesNode;
            }
        }

        public FluorineFx.Messaging.Config.SecuritySettings SecuritySettings
        {
            get
            {
                return this._security;
            }
        }

        public FluorineFx.Messaging.Config.ServerSettings ServerSettings
        {
            get
            {
                return this._server;
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

