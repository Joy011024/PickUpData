namespace FluorineFx.Messaging.Config
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Messaging.Services;
    using FluorineFx.Remoting;
    using log4net;
    using System;
    using System.IO;
    using System.Xml;

    public sealed class ServiceConfigSettings
    {
        private ChannelSettingsCollection _channelSettingsCollection = new ChannelSettingsCollection();
        private FactorySettingsCollection _factorySettingsCollection = new FactorySettingsCollection();
        private FluorineFx.Messaging.Config.FlexClientSettings _flexClientSettings;
        private FluorineFx.Messaging.Config.SecuritySettings _securitySettings;
        private ServiceSettingsCollection _serviceSettingsCollection = new ServiceSettingsCollection();
        private static readonly ILog log = LogManager.GetLogger(typeof(ServiceConfigSettings));

        internal ServiceConfigSettings()
        {
        }

        public static ServiceConfigSettings Load(string configPath, string configFileName)
        {
            ChannelSettings settings2;
            FluorineFx.Messaging.Config.ServiceSettings settings4;
            AdapterSettings settings5;
            FluorineFx.Messaging.Config.SecuritySettings settings6;
            string path = Path.Combine(configPath, configFileName);
            ServiceConfigSettings serviceConfigSettings = new ServiceConfigSettings();
            if (File.Exists(path))
            {
                log.Debug(__Res.GetString("MessageServer_LoadingConfig", new object[] { path }));
                XmlDocument document = new XmlDocument();
                document.Load(path);
                XmlNodeList list = document.SelectNodes("/services-config/channels/channel-definition");
                foreach (XmlNode node in list)
                {
                    XmlNode node2 = node.SelectSingleNode("endpoint");
                    string str2 = node2.Attributes["class"].Value;
                    string str3 = node2.Attributes["uri"].Value;
                    settings2 = new ChannelSettings(node);
                    serviceConfigSettings.ChannelsSettings.Add(settings2);
                }
                XmlNodeList list2 = document.SelectNodes("/services-config/factories/factory");
                foreach (XmlNode node3 in list2)
                {
                    string str4 = node3.Attributes["id"].Value;
                    string str5 = node3.Attributes["class"].Value;
                    FactorySettings settings3 = new FactorySettings(node3);
                    serviceConfigSettings.FactoriesSettings.Add(settings3);
                }
                XmlNodeList list3 = document.SelectNodes("/services-config/services/service-include");
                foreach (XmlNode node4 in list3)
                {
                    string str6 = node4.Attributes["file-path"].Value;
                    str6 = Path.Combine(configPath, str6);
                    log.Debug(__Res.GetString("MessageServer_LoadingServiceConfig", new object[] { str6 }));
                    settings4 = new FluorineFx.Messaging.Config.ServiceSettings(serviceConfigSettings);
                    settings4.Init(str6);
                    if ((settings4.Id == "remoting-service") && (settings4.DefaultAdapter == null))
                    {
                        settings5 = new AdapterSettings("dotnet", typeof(RemotingAdapter).FullName, false);
                        settings4.AdapterSettings.Add(settings5);
                    }
                    serviceConfigSettings.ServiceSettings.Add(settings4);
                }
                XmlNodeList list4 = document.SelectNodes("/services-config/services/service");
                foreach (XmlNode node5 in list4)
                {
                    settings4 = new FluorineFx.Messaging.Config.ServiceSettings(serviceConfigSettings);
                    settings4.Init(node5);
                    if ((settings4.Id == "remoting-service") && (settings4.DefaultAdapter == null))
                    {
                        settings5 = new AdapterSettings("dotnet", typeof(RemotingAdapter).FullName, false);
                        settings4.AdapterSettings.Add(settings5);
                    }
                    serviceConfigSettings.ServiceSettings.Add(settings4);
                }
                XmlNode securityNode = document.SelectSingleNode("/services-config/security");
                if (securityNode != null)
                {
                    settings6 = new FluorineFx.Messaging.Config.SecuritySettings(null, securityNode);
                    serviceConfigSettings._securitySettings = settings6;
                }
                XmlNode flexClientNode = document.SelectSingleNode("/services-config/flex-client");
                if (flexClientNode != null)
                {
                    FluorineFx.Messaging.Config.FlexClientSettings settings7 = new FluorineFx.Messaging.Config.FlexClientSettings(flexClientNode);
                    serviceConfigSettings._flexClientSettings = settings7;
                    return serviceConfigSettings;
                }
                serviceConfigSettings._flexClientSettings = new FluorineFx.Messaging.Config.FlexClientSettings();
                return serviceConfigSettings;
            }
            log.Debug(__Res.GetString("MessageServer_LoadingConfigDefault", new object[] { path }));
            LoginCommandCollection loginCommands = FluorineConfiguration.Instance.LoginCommands;
            if (loginCommands != null)
            {
                settings6 = new FluorineFx.Messaging.Config.SecuritySettings(null);
                LoginCommandSettings item = new LoginCommandSettings {
                    Server = "asp.net",
                    Type = loginCommands.GetLoginCommand("asp.net")
                };
                settings6.LoginCommands.Add(item);
                serviceConfigSettings._securitySettings = settings6;
            }
            settings2 = new ChannelSettings("my-amf", "flex.messaging.endpoints.AMFEndpoint", "http://{server.name}:{server.port}/{context.root}/Gateway.aspx");
            serviceConfigSettings.ChannelsSettings.Add(settings2);
            settings4 = new FluorineFx.Messaging.Config.ServiceSettings(serviceConfigSettings, "remoting-service", typeof(RemotingService).FullName);
            string customClass = "flex.messaging.messages.RemotingMessage";
            string type = FluorineConfiguration.Instance.ClassMappings.GetType(customClass);
            settings4.SupportedMessageTypes[customClass] = type;
            serviceConfigSettings.ServiceSettings.Add(settings4);
            settings5 = new AdapterSettings("dotnet", typeof(RemotingAdapter).FullName, true);
            settings4.DefaultAdapter = settings5;
            DestinationSettings settings9 = new DestinationSettings(settings4, "fluorine", settings5, "*");
            settings4.DestinationSettings.Add(settings9);
            serviceConfigSettings._flexClientSettings = new FluorineFx.Messaging.Config.FlexClientSettings();
            return serviceConfigSettings;
        }

        public ChannelSettingsCollection ChannelsSettings
        {
            get
            {
                return this._channelSettingsCollection;
            }
        }

        public FactorySettingsCollection FactoriesSettings
        {
            get
            {
                return this._factorySettingsCollection;
            }
        }

        public FluorineFx.Messaging.Config.FlexClientSettings FlexClientSettings
        {
            get
            {
                return this._flexClientSettings;
            }
        }

        public FluorineFx.Messaging.Config.SecuritySettings SecuritySettings
        {
            get
            {
                return this._securitySettings;
            }
        }

        public ServiceSettingsCollection ServiceSettings
        {
            get
            {
                return this._serviceSettingsCollection;
            }
        }
    }
}

