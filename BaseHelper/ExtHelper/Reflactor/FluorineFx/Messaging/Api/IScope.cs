namespace FluorineFx.Messaging.Api
{
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Persistence;
    using System;
    using System.Collections;

    public interface IScope : IBasicScope, ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener, IEventObservable, IPersistable, IEnumerable, IServiceContainer, IServiceProvider
    {
        bool AddChildScope(IBasicScope scope);
        bool Connect(IConnection connection);
        bool Connect(IConnection connection, object[] parameters);
        bool CreateChildScope(string name);
        void Disconnect(IConnection conn);
        IBasicScope GetBasicScope(string type, string name);
        IEnumerator GetBasicScopeNames(string type);
        ICollection GetClients();
        IEnumerator GetConnections();
        IScope GetScope(string name);
        ICollection GetScopeNames();
        bool HasChildScope(string name);
        bool HasChildScope(string type, string name);
        ICollection LookupConnections(IClient client);
        void RemoveChildScope(IBasicScope scope);

        IScopeContext Context { get; }

        string ContextPath { get; }

        IScopeHandler Handler { get; }

        bool HasHandler { get; }
    }
}

