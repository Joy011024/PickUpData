namespace FluorineFx.Messaging.Api
{
    using FluorineFx.Context;
    using FluorineFx.Messaging.Api.Persistence;
    using FluorineFx.Messaging.Api.Service;
    using System;

    public interface IScopeContext
    {
        IScope GetGlobalScope();
        IResource GetResource(string location);
        IScopeHandler LookupScopeHandler(string path);
        IScope ResolveScope(string path);
        IScope ResolveScope(IScope root, string path);

        IClientRegistry ClientRegistry { get; }

        IPersistenceStore PersistenceStore { get; }

        IScopeResolver ScopeResolver { get; }

        IServiceInvoker ServiceInvoker { get; }
    }
}

