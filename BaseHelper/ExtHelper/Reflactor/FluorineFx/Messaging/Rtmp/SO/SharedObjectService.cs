namespace FluorineFx.Messaging.Rtmp.SO
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Persistence;
    using FluorineFx.Messaging.Api.SO;
    using FluorineFx.Messaging.Rtmp.Persistence;
    using log4net;
    using System;
    using System.Collections;

    internal class SharedObjectService : ISharedObjectService, IScopeService, IService
    {
        private SharedObjectServiceConfiguration _configuration;
        private ILog _log;
        public static string ScopeType = "SharedObject";
        private static string SO_PERSISTENCE_STORE = "_transient_SO_PERSISTENCE_STORE_";
        private static string SO_TRANSIENT_STORE = "_transient_SO_TRANSIENT_STORE_";

        public SharedObjectService()
        {
            try
            {
                this._log = LogManager.GetLogger(typeof(SharedObjectService));
            }
            catch
            {
            }
        }

        public bool ClearSharedObjects(IScope scope, string name)
        {
            bool flag = false;
            if (this.HasSharedObject(scope, name))
            {
                flag = (scope.GetBasicScope(ScopeType, name) as ISharedObject).Clear();
            }
            return flag;
        }

        public bool CreateSharedObject(IScope scope, string name, bool persistent)
        {
            if (this.HasSharedObject(scope, name))
            {
                return true;
            }
            IBasicScope scope2 = new SharedObjectScope(scope, name, persistent, this.GetStore(scope, persistent));
            return scope.AddChildScope(scope2);
        }

        public ISharedObject GetSharedObject(IScope scope, string name)
        {
            return (scope.GetBasicScope(ScopeType, name) as ISharedObject);
        }

        public ISharedObject GetSharedObject(IScope scope, string name, bool persistent)
        {
            if (!this.HasSharedObject(scope, name))
            {
                this.CreateSharedObject(scope, name, persistent);
            }
            return this.GetSharedObject(scope, name);
        }

        public ICollection GetSharedObjectNames(IScope scope)
        {
            ArrayList list = new ArrayList();
            IEnumerator basicScopeNames = scope.GetBasicScopeNames(ScopeType);
            while (basicScopeNames.MoveNext())
            {
                list.Add(basicScopeNames.Current);
            }
            return list;
        }

        private IPersistenceStore GetStore(IScope scope, bool persistent)
        {
            IPersistenceStore store = null;
            if (!persistent)
            {
                if (!scope.HasAttribute(SO_TRANSIENT_STORE))
                {
                    store = new MemoryStore(scope);
                    scope.SetAttribute(SO_TRANSIENT_STORE, store);
                    return store;
                }
                return (scope.GetAttribute(SO_TRANSIENT_STORE) as IPersistenceStore);
            }
            if (!scope.HasAttribute(SO_PERSISTENCE_STORE))
            {
                try
                {
                    store = Activator.CreateInstance(ObjectFactory.Locate(this._configuration.PersistenceStore.Type), new object[] { scope }) as IPersistenceStore;
                    if (this._log.get_IsInfoEnabled())
                    {
                        this._log.Info(__Res.GetString("SharedObjectService_CreateStore", new object[] { store }));
                    }
                }
                catch (Exception exception)
                {
                    if (this._log.get_IsErrorEnabled())
                    {
                        this._log.Error(__Res.GetString("SharedObjectService_CreateStoreError"), exception);
                    }
                    store = new MemoryStore(scope);
                }
                scope.SetAttribute(SO_PERSISTENCE_STORE, store);
                return store;
            }
            return (scope.GetAttribute(SO_PERSISTENCE_STORE) as IPersistenceStore);
        }

        public bool HasSharedObject(IScope scope, string name)
        {
            return scope.HasChildScope(ScopeType, name);
        }

        public void Shutdown()
        {
        }

        public void Start(ConfigurationSection configuration)
        {
            this._configuration = configuration as SharedObjectServiceConfiguration;
        }
    }
}

