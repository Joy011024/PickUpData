namespace FluorineFx.Messaging.Api
{
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Service;
    using System;

    public interface IScopeHandler : IEventHandler
    {
        bool AddChildScope(IBasicScope scope);
        bool Connect(IConnection connection, IScope scope, object[] parameters);
        void Disconnect(IConnection connection, IScope scope);
        bool Join(IClient client, IScope scope);
        void Leave(IClient client, IScope scope);
        void RemoveChildScope(IBasicScope scope);
        bool ServiceCall(IConnection connection, IServiceCall call);
        bool Start(IScope scope);
        void Stop(IScope scope);
    }
}

