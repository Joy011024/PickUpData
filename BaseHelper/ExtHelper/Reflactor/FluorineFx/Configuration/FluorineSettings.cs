namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    [XmlType(TypeName="settings")]
    public sealed class FluorineSettings
    {
        private bool _acceptNullValueTypes = false;
        private BWControlServiceSettings _bwControlServiceSettings;
        private CacheCollection _cache;
        private ClassMappingCollection _classMappings;
        private FluorineFx.Configuration.HttpCompressSettings _httpCompressSettings;
        private ImportNamespaceCollection _importedNamespaces;
        private FluorineFx.Configuration.JSonSettings _jsonSettings;
        private LoginCommandCollection _loginCommandCollection;
        private NullableTypeCollection _nullables;
        private OptimizerSettings _optimizerSettings;
        private PlaylistSubscriberStreamSettings _playlistSubscriberStreamSettings;
        private RemotingServiceAttributeConstraint _remotingServiceAttributeConstraint = RemotingServiceAttributeConstraint.Access;
        private RtmpServerSettings _rtmpServerSettings = new RtmpServerSettings();
        private RuntimeSettings _runtimeSettings = new RuntimeSettings();
        private SchedulingServiceSettings _schedulingServiceSettings;
        private ServiceCollection _services;
        private SilverlightSettings _silverlightSettings;
        private StreamableFileFactorySettings _streamableFileFactorySettings;
        private FluorineFx.Configuration.SwxSettings _swxSettings = new FluorineFx.Configuration.SwxSettings();
        private FluorineFx.Configuration.TimezoneCompensation _timezoneCompensation = FluorineFx.Configuration.TimezoneCompensation.None;
        private bool _wsdlGenerateProxyClasses = true;
        private string _wsdlProxyNamespace = "FluorineFx.Proxy";

        [XmlElement(ElementName="acceptNullValueTypes")]
        public bool AcceptNullValueTypes
        {
            get
            {
                return this._acceptNullValueTypes;
            }
            set
            {
                this._acceptNullValueTypes = value;
            }
        }

        [XmlElement(ElementName="bwControlService")]
        public BWControlServiceSettings BWControlService
        {
            get
            {
                if (this._bwControlServiceSettings == null)
                {
                    this._bwControlServiceSettings = new BWControlServiceSettings();
                }
                return this._bwControlServiceSettings;
            }
            set
            {
                this._bwControlServiceSettings = value;
            }
        }

        [XmlArray("cache"), XmlArrayItem("cachedService", typeof(CachedService))]
        public CacheCollection Cache
        {
            get
            {
                if (this._cache == null)
                {
                    this._cache = new CacheCollection();
                }
                return this._cache;
            }
        }

        [XmlArrayItem("classMapping", typeof(ClassMapping)), XmlArray("classMappings")]
        public ClassMappingCollection ClassMappings
        {
            get
            {
                if (this._classMappings == null)
                {
                    this._classMappings = new ClassMappingCollection();
                }
                return this._classMappings;
            }
        }

        [XmlElement(ElementName="httpCompress")]
        public FluorineFx.Configuration.HttpCompressSettings HttpCompressSettings
        {
            get
            {
                return this._httpCompressSettings;
            }
            set
            {
                this._httpCompressSettings = value;
            }
        }

        [XmlArray("importNamespaces"), XmlArrayItem("add", typeof(ImportNamespace))]
        public ImportNamespaceCollection ImportNamespaces
        {
            get
            {
                if (this._importedNamespaces == null)
                {
                    this._importedNamespaces = new ImportNamespaceCollection();
                }
                return this._importedNamespaces;
            }
        }

        [XmlElement(ElementName="json")]
        public FluorineFx.Configuration.JSonSettings JSonSettings
        {
            get
            {
                if (this._jsonSettings == null)
                {
                    this._jsonSettings = new FluorineFx.Configuration.JSonSettings();
                }
                return this._jsonSettings;
            }
            set
            {
                this._jsonSettings = value;
            }
        }

        [XmlArray("security"), XmlArrayItem("login-command", typeof(LoginCommandSettings))]
        public LoginCommandCollection LoginCommands
        {
            get
            {
                if (this._loginCommandCollection == null)
                {
                    this._loginCommandCollection = new LoginCommandCollection();
                }
                return this._loginCommandCollection;
            }
        }

        [XmlArray("nullable"), XmlArrayItem("type", typeof(NullableType))]
        public NullableTypeCollection Nullables
        {
            get
            {
                if (this._nullables == null)
                {
                    this._nullables = new NullableTypeCollection();
                }
                return this._nullables;
            }
        }

        [XmlElement(ElementName="optimizer")]
        public OptimizerSettings Optimizer
        {
            get
            {
                return this._optimizerSettings;
            }
            set
            {
                this._optimizerSettings = value;
            }
        }

        [XmlElement(ElementName="playlistSubscriberStream")]
        public PlaylistSubscriberStreamSettings PlaylistSubscriberStream
        {
            get
            {
                if (this._playlistSubscriberStreamSettings == null)
                {
                    this._playlistSubscriberStreamSettings = new PlaylistSubscriberStreamSettings();
                }
                return this._playlistSubscriberStreamSettings;
            }
            set
            {
                this._playlistSubscriberStreamSettings = value;
            }
        }

        [XmlElement(ElementName="remotingServiceAttribute")]
        public RemotingServiceAttributeConstraint RemotingServiceAttribute
        {
            get
            {
                return this._remotingServiceAttributeConstraint;
            }
            set
            {
                this._remotingServiceAttributeConstraint = value;
            }
        }

        [XmlElement(ElementName="rtmpServer")]
        public RtmpServerSettings RtmpServer
        {
            get
            {
                return this._rtmpServerSettings;
            }
            set
            {
                this._rtmpServerSettings = value;
            }
        }

        [XmlElement(ElementName="runtime")]
        public RuntimeSettings Runtime
        {
            get
            {
                return this._runtimeSettings;
            }
            set
            {
                this._runtimeSettings = value;
            }
        }

        [XmlElement(ElementName="schedulingService")]
        public SchedulingServiceSettings SchedulingService
        {
            get
            {
                if (this._schedulingServiceSettings == null)
                {
                    this._schedulingServiceSettings = new SchedulingServiceSettings();
                }
                return this._schedulingServiceSettings;
            }
            set
            {
                this._schedulingServiceSettings = value;
            }
        }

        [XmlArray("services"), XmlArrayItem("service", typeof(ServiceConfiguration))]
        public ServiceCollection Services
        {
            get
            {
                if (this._services == null)
                {
                    this._services = new ServiceCollection();
                }
                return this._services;
            }
        }

        [XmlElement(ElementName="silverlight")]
        public SilverlightSettings Silverlight
        {
            get
            {
                if (this._silverlightSettings == null)
                {
                    this._silverlightSettings = new SilverlightSettings();
                }
                return this._silverlightSettings;
            }
            set
            {
                this._silverlightSettings = value;
            }
        }

        [XmlElement(ElementName="streamableFileFactory")]
        public StreamableFileFactorySettings StreamableFileFactory
        {
            get
            {
                if (this._streamableFileFactorySettings == null)
                {
                    this._streamableFileFactorySettings = new StreamableFileFactorySettings();
                }
                return this._streamableFileFactorySettings;
            }
            set
            {
                this._streamableFileFactorySettings = value;
            }
        }

        [XmlElement(ElementName="swx")]
        public FluorineFx.Configuration.SwxSettings SwxSettings
        {
            get
            {
                return this._swxSettings;
            }
            set
            {
                this._swxSettings = value;
            }
        }

        [XmlElement(ElementName="timezoneCompensation")]
        public FluorineFx.Configuration.TimezoneCompensation TimezoneCompensation
        {
            get
            {
                return this._timezoneCompensation;
            }
            set
            {
                this._timezoneCompensation = value;
            }
        }

        [XmlElement(ElementName="wsdlGenerateProxyClasses")]
        public bool WsdlGenerateProxyClasses
        {
            get
            {
                return this._wsdlGenerateProxyClasses;
            }
            set
            {
                this._wsdlGenerateProxyClasses = value;
            }
        }

        [XmlElement(ElementName="wsdlProxyNamespace")]
        public string WsdlProxyNamespace
        {
            get
            {
                return this._wsdlProxyNamespace;
            }
            set
            {
                this._wsdlProxyNamespace = value;
            }
        }
    }
}

