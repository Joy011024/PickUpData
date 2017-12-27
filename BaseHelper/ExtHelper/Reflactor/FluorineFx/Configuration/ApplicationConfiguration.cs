namespace FluorineFx.Configuration
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    [XmlRoot("configuration")]
    public class ApplicationConfiguration
    {
        private ApplicationHandlerConfiguration _applicationHandler;
        private FluorineFx.Configuration.ConsumerServiceConfiguration _consumerServiceConfiguration;
        private FluorineFx.Configuration.ProviderServiceConfiguration _providerService;
        private SharedObjectSecurityServiceConfiguration _sharedObjectSecurityService;
        private FluorineFx.Configuration.SharedObjectServiceConfiguration _sharedObjectServiceConfiguration;
        private StreamFilenameGeneratorConfiguration _streamFilenameGenerator;
        private StreamServiceConfiguration _streamService;

        public static ApplicationConfiguration Load(string path)
        {
            if (!File.Exists(path))
            {
                return new ApplicationConfiguration();
            }
            using (StreamReader reader = new StreamReader(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationConfiguration));
                ApplicationConfiguration configuration = serializer.Deserialize(reader) as ApplicationConfiguration;
                reader.Close();
                return configuration;
            }
        }

        [XmlElement("application-handler")]
        public ApplicationHandlerConfiguration ApplicationHandler
        {
            get
            {
                if (this._applicationHandler == null)
                {
                    return (this._applicationHandler = new ApplicationHandlerConfiguration());
                }
                return this._applicationHandler;
            }
            set
            {
                this._applicationHandler = value;
            }
        }

        [XmlElement("consumerService")]
        public FluorineFx.Configuration.ConsumerServiceConfiguration ConsumerServiceConfiguration
        {
            get
            {
                if (this._consumerServiceConfiguration == null)
                {
                    return (this._consumerServiceConfiguration = new FluorineFx.Configuration.ConsumerServiceConfiguration());
                }
                return this._consumerServiceConfiguration;
            }
            set
            {
                this._consumerServiceConfiguration = value;
            }
        }

        [XmlElement("providerService")]
        public FluorineFx.Configuration.ProviderServiceConfiguration ProviderServiceConfiguration
        {
            get
            {
                if (this._providerService == null)
                {
                    return (this._providerService = new FluorineFx.Configuration.ProviderServiceConfiguration());
                }
                return this._providerService;
            }
            set
            {
                this._providerService = value;
            }
        }

        [XmlElement("sharedObjectSecurityService")]
        public SharedObjectSecurityServiceConfiguration SharedObjectSecurityService
        {
            get
            {
                if (this._sharedObjectSecurityService == null)
                {
                    return (this._sharedObjectSecurityService = new SharedObjectSecurityServiceConfiguration());
                }
                return this._sharedObjectSecurityService;
            }
            set
            {
                this._sharedObjectSecurityService = value;
            }
        }

        [XmlElement("sharedObjectService")]
        public FluorineFx.Configuration.SharedObjectServiceConfiguration SharedObjectServiceConfiguration
        {
            get
            {
                if (this._sharedObjectServiceConfiguration == null)
                {
                    return (this._sharedObjectServiceConfiguration = new FluorineFx.Configuration.SharedObjectServiceConfiguration());
                }
                return this._sharedObjectServiceConfiguration;
            }
            set
            {
                this._sharedObjectServiceConfiguration = value;
            }
        }

        [XmlElement("streamFilenameGenerator")]
        public StreamFilenameGeneratorConfiguration StreamFilenameGenerator
        {
            get
            {
                if (this._streamFilenameGenerator == null)
                {
                    return (this._streamFilenameGenerator = new StreamFilenameGeneratorConfiguration());
                }
                return this._streamFilenameGenerator;
            }
            set
            {
                this._streamFilenameGenerator = value;
            }
        }

        [XmlElement("streamService")]
        public StreamServiceConfiguration StreamService
        {
            get
            {
                if (this._streamService == null)
                {
                    return (this._streamService = new StreamServiceConfiguration());
                }
                return this._streamService;
            }
            set
            {
                this._streamService = value;
            }
        }
    }
}

