namespace FluorineFx.Messaging.Api
{
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Service;
    using System;

    public interface IServiceCapableConnection : IConnection, ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener
    {
        void Invoke(IServiceCall serviceCall);
        void Invoke(string method);
        void Invoke(IServiceCall serviceCall, byte channel);
        void Invoke(string method, IPendingServiceCallback callback);
        void Invoke(string method, object[] parameters);
        void Invoke(string method, object[] parameters, IPendingServiceCallback callback);
        void Notify(IServiceCall serviceCall);
        void Notify(string method);
        void Notify(IServiceCall serviceCall, byte channel);
        void Notify(string method, object[] parameters);
    }
}

