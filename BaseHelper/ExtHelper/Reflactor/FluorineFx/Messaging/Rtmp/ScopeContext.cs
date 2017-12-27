namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx.Context;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Persistence;
    using FluorineFx.Messaging.Api.Service;
    using System;

    internal class ScopeContext : IScopeContext
    {
        private IClientRegistry _clientRegistry;
        private string _contextPath;
        private IPersistenceStore _persistanceStore;
        private IScopeResolver _scopeResolver;
        private IServiceInvoker _serviceInvoker;

        private ScopeContext()
        {
            this._contextPath = string.Empty;
        }

        public ScopeContext(string contextPath, IClientRegistry clientRegistry, IScopeResolver scopeResolver, IServiceInvoker serviceInvoker, IPersistenceStore persistanceStore)
        {
            this._contextPath = string.Empty;
            this._contextPath = contextPath;
            this._clientRegistry = clientRegistry;
            this._scopeResolver = scopeResolver;
            this._persistanceStore = persistanceStore;
            this._serviceInvoker = serviceInvoker;
        }

        public IScope GetGlobalScope()
        {
            return this._scopeResolver.GlobalScope;
        }

        public IResource GetResource(string location)
        {
            return new FileSystemResource(location);
        }

        public IScopeHandler LookupScopeHandler(string contextPath)
        {
            return null;
        }

        public IScope ResolveScope(string path)
        {
            return this._scopeResolver.ResolveScope(path);
        }

        public IScope ResolveScope(IScope root, string path)
        {
            return this._scopeResolver.ResolveScope(root, path);
        }

        public IClientRegistry ClientRegistry
        {
            get
            {
                return this._clientRegistry;
            }
        }

        public string ContextPath
        {
            get
            {
                return this._contextPath;
            }
        }

        public IPersistenceStore PersistenceStore
        {
            get
            {
                return this._persistanceStore;
            }
        }

        public IScopeResolver ScopeResolver
        {
            get
            {
                return this._scopeResolver;
            }
        }

        public IServiceInvoker ServiceInvoker
        {
            get
            {
                return this._serviceInvoker;
            }
        }
    }
}

