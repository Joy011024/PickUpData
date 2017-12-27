namespace FluorineFx.Messaging.Api
{
    using FluorineFx.Util;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class ServiceContainer : IServiceContainer, IServiceProvider
    {
        private IServiceProvider _parentProvider;
        private Dictionary<Type, object> _services;

        public ServiceContainer() : this(null)
        {
        }

        public ServiceContainer(IServiceProvider parentProvider)
        {
            this._services = new Dictionary<Type, object>();
            this._parentProvider = parentProvider;
        }

        public void AddService(Type serviceType, object service)
        {
            this.AddService(serviceType, service, false);
        }

        public void AddService(Type serviceType, object service, bool promote)
        {
            ValidationUtils.ArgumentNotNull(serviceType, "serviceType");
            ValidationUtils.ArgumentNotNull(service, "service");
            lock (this.SyncRoot)
            {
                if (promote)
                {
                    IServiceContainer container = this.Container;
                    if (container != null)
                    {
                        container.AddService(serviceType, service, promote);
                        goto Label_0091;
                    }
                }
                if (this._services.ContainsKey(serviceType))
                {
                    throw new ArgumentException(string.Format("Service {0} already exists", serviceType.FullName));
                }
                this._services[serviceType] = service;
            Label_0091:;
            }
        }

        public object GetService(Type serviceType)
        {
            ValidationUtils.ArgumentNotNull(serviceType, "serviceType");
            object service = null;
            lock (this.SyncRoot)
            {
                if (this._services.ContainsKey(serviceType))
                {
                    service = this._services[serviceType];
                }
                if ((service == null) && (this._parentProvider != null))
                {
                    service = this._parentProvider.GetService(serviceType);
                }
            }
            return service;
        }

        public void RemoveService(Type serviceType)
        {
            this.RemoveService(serviceType, false);
        }

        public void RemoveService(Type serviceType, bool promote)
        {
            ValidationUtils.ArgumentNotNull(serviceType, "serviceType");
            lock (this.SyncRoot)
            {
                if (promote)
                {
                    IServiceContainer container = this.Container;
                    if (container != null)
                    {
                        container.RemoveService(serviceType, promote);
                        goto Label_0090;
                    }
                }
                if (this._services.ContainsKey(serviceType))
                {
                    IService service = this._services[serviceType] as IService;
                    if (service != null)
                    {
                        service.Shutdown();
                    }
                }
                this._services.Remove(serviceType);
            Label_0090:;
            }
        }

        internal void Shutdown()
        {
            lock (this.SyncRoot)
            {
                foreach (object obj2 in this._services.Values)
                {
                    IService service = obj2 as IService;
                    if (service != null)
                    {
                        service.Shutdown();
                    }
                }
                this._services.Clear();
                this._services = null;
                this._parentProvider = null;
            }
        }

        private IServiceContainer Container
        {
            get
            {
                IServiceContainer service = null;
                if (this._parentProvider != null)
                {
                    service = (IServiceContainer) this._parentProvider.GetService(typeof(IServiceContainer));
                }
                return service;
            }
        }

        public object SyncRoot
        {
            get
            {
                return ((ICollection) this._services).SyncRoot;
            }
        }
    }
}

