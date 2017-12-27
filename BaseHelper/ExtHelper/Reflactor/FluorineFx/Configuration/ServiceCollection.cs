namespace FluorineFx.Configuration
{
    using FluorineFx.Collections.Generic;
    using System;
    using System.Collections.Generic;

    public sealed class ServiceCollection : CollectionBase<ServiceConfiguration>
    {
        private Dictionary<string, ServiceConfiguration> _serviceLocations = new Dictionary<string, ServiceConfiguration>(5);
        private Dictionary<string, ServiceConfiguration> _serviceNames = new Dictionary<string, ServiceConfiguration>(5);

        public override void Add(ServiceConfiguration value)
        {
            this._serviceNames[value.Name] = value;
            this._serviceLocations[value.ServiceLocation] = value;
            base.Add(value);
        }

        public bool Contains(string serviceName)
        {
            return this._serviceNames.ContainsKey(serviceName);
        }

        public string GetMethod(string serviceName, string name)
        {
            ServiceConfiguration configuration = null;
            if (this._serviceNames.ContainsKey(serviceName))
            {
                configuration = this._serviceNames[serviceName];
            }
            if (configuration != null)
            {
                return configuration.Methods.GetMethod(name);
            }
            return name;
        }

        public string GetMethodName(string serviceLocation, string method)
        {
            ServiceConfiguration configuration = null;
            if (this._serviceLocations.ContainsKey(serviceLocation))
            {
                configuration = this._serviceLocations[serviceLocation];
            }
            if (configuration != null)
            {
                return configuration.Methods.GetMethodName(method);
            }
            return method;
        }

        public string GetServiceLocation(string serviceName)
        {
            if (this._serviceNames.ContainsKey(serviceName))
            {
                return this._serviceNames[serviceName].ServiceLocation;
            }
            return serviceName;
        }

        public string GetServiceName(string serviceLocation)
        {
            if (this._serviceLocations.ContainsKey(serviceLocation))
            {
                return this._serviceLocations[serviceLocation].Name;
            }
            return serviceLocation;
        }

        public override void Insert(int index, ServiceConfiguration value)
        {
            this._serviceNames[value.Name] = value;
            this._serviceLocations[value.ServiceLocation] = value;
            base.Insert(index, value);
        }

        public override bool Remove(ServiceConfiguration value)
        {
            this._serviceNames.Remove(value.Name);
            this._serviceLocations.Remove(value.ServiceLocation);
            return base.Remove(value);
        }
    }
}

