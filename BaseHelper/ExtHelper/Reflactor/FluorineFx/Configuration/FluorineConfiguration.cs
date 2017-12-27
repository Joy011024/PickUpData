namespace FluorineFx.Configuration
{
    using System;
    using System.Configuration;
    using System.Security;
    using System.Security.Permissions;
    using System.Threading;

    public sealed class FluorineConfiguration
    {
        private static FluorineFx.Configuration.CacheMap _cacheMap = new FluorineFx.Configuration.CacheMap();
        private static FluorineFx.Configuration.FluorineSettings _fluorineSettings;
        private static bool _fullTrust;
        private static FluorineConfiguration _instance;
        private static object _objLock = new object();

        private FluorineConfiguration()
        {
        }

        private static bool CheckApplicationPermissions()
        {
            try
            {
                new PermissionSet(PermissionState.Unrestricted).Demand();
                return true;
            }
            catch (SecurityException)
            {
            }
            return false;
        }

        internal string GetCustomClass(string type)
        {
            if (this.ClassMappings != null)
            {
                return this.ClassMappings.GetCustomClass(type);
            }
            return type;
        }

        internal string GetMappedTypeName(string customClass)
        {
            if (this.ClassMappings != null)
            {
                return this.ClassMappings.GetType(customClass);
            }
            return customClass;
        }

        internal string GetMethodName(string serviceLocation, string method)
        {
            if (this.ServiceMap != null)
            {
                return this.ServiceMap.GetMethodName(serviceLocation, method);
            }
            return method;
        }

        internal string GetServiceLocation(string serviceName)
        {
            if (this.ServiceMap != null)
            {
                return this.ServiceMap.GetServiceLocation(serviceName);
            }
            return serviceName;
        }

        internal string GetServiceName(string serviceLocation)
        {
            if (this.ServiceMap != null)
            {
                return this.ServiceMap.GetServiceName(serviceLocation);
            }
            return serviceLocation;
        }

        public bool AcceptNullValueTypes
        {
            get
            {
                return ((_fluorineSettings != null) && _fluorineSettings.AcceptNullValueTypes);
            }
        }

        internal FluorineFx.Configuration.CacheMap CacheMap
        {
            get
            {
                return _cacheMap;
            }
        }

        internal ClassMappingCollection ClassMappings
        {
            get
            {
                return _fluorineSettings.ClassMappings;
            }
        }

        public FluorineFx.Configuration.FluorineSettings FluorineSettings
        {
            get
            {
                return _fluorineSettings;
            }
        }

        public bool FullTrust
        {
            get
            {
                return _fullTrust;
            }
        }

        public FluorineFx.Configuration.HttpCompressSettings HttpCompressSettings
        {
            get
            {
                if ((_fluorineSettings != null) && (_fluorineSettings.HttpCompressSettings != null))
                {
                    return _fluorineSettings.HttpCompressSettings;
                }
                return FluorineFx.Configuration.HttpCompressSettings.Default;
            }
        }

        public ImportNamespaceCollection ImportNamespaces
        {
            get
            {
                if (_fluorineSettings != null)
                {
                    return _fluorineSettings.ImportNamespaces;
                }
                return null;
            }
        }

        public static FluorineConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_objLock)
                    {
                        if (_instance == null)
                        {
                            FluorineConfiguration configuration = new FluorineConfiguration();
                            _fluorineSettings = ConfigurationManager.GetSection("fluorinefx/settings") as FluorineFx.Configuration.FluorineSettings;
                            if (_fluorineSettings == null)
                            {
                                _fluorineSettings = new FluorineFx.Configuration.FluorineSettings();
                            }
                            if ((_fluorineSettings != null) && (_fluorineSettings.Cache != null))
                            {
                                foreach (CachedService service in _fluorineSettings.Cache)
                                {
                                    _cacheMap.AddCacheDescriptor(service.Type, service.Timeout, service.SlidingExpiration);
                                }
                            }
                            _fullTrust = CheckApplicationPermissions();
                            Thread.MemoryBarrier();
                            _instance = configuration;
                        }
                    }
                }
                return _instance;
            }
        }

        internal LoginCommandCollection LoginCommands
        {
            get
            {
                if (_fluorineSettings != null)
                {
                    return _fluorineSettings.LoginCommands;
                }
                return null;
            }
        }

        public NullableTypeCollection NullableValues
        {
            get
            {
                if (_fluorineSettings != null)
                {
                    return _fluorineSettings.Nullables;
                }
                return null;
            }
        }

        internal FluorineFx.Configuration.OptimizerSettings OptimizerSettings
        {
            get
            {
                if (_fluorineSettings != null)
                {
                    return _fluorineSettings.Optimizer;
                }
                return null;
            }
        }

        public FluorineFx.Configuration.RemotingServiceAttributeConstraint RemotingServiceAttributeConstraint
        {
            get
            {
                if (_fluorineSettings != null)
                {
                    return _fluorineSettings.RemotingServiceAttribute;
                }
                return FluorineFx.Configuration.RemotingServiceAttributeConstraint.Access;
            }
        }

        internal ServiceCollection ServiceMap
        {
            get
            {
                if (_fluorineSettings != null)
                {
                    return _fluorineSettings.Services;
                }
                return null;
            }
        }

        internal FluorineFx.Configuration.SwxSettings SwxSettings
        {
            get
            {
                if (_fluorineSettings != null)
                {
                    return _fluorineSettings.SwxSettings;
                }
                return null;
            }
        }

        public FluorineFx.Configuration.TimezoneCompensation TimezoneCompensation
        {
            get
            {
                if (_fluorineSettings != null)
                {
                    return _fluorineSettings.TimezoneCompensation;
                }
                return FluorineFx.Configuration.TimezoneCompensation.None;
            }
        }

        public bool WsdlGenerateProxyClasses
        {
            get
            {
                return ((_fluorineSettings != null) && _fluorineSettings.WsdlGenerateProxyClasses);
            }
        }

        public string WsdlProxyNamespace
        {
            get
            {
                if (_fluorineSettings != null)
                {
                    return _fluorineSettings.WsdlProxyNamespace;
                }
                return "FluorineFx.Proxy";
            }
        }
    }
}

