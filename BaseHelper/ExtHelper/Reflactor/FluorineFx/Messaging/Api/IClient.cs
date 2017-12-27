namespace FluorineFx.Messaging.Api
{
    using FluorineFx.Context;
    using FluorineFx.Messaging.Messages;
    using System;
    using System.Collections;

    public interface IClient : IAttributeStore
    {
        void AddSessionDestroyedListener(ISessionListener listener);
        void Disconnect();
        IMessage[] GetPendingMessages(int waitIntervalMillis);
        void Register(IConnection connection);
        void RegisterMessageClient(IMessageClient messageClient);
        void RemoveSessionDestroyedListener(ISessionListener listener);
        void Renew();
        void Renew(int clientLeaseTime);
        void Timeout();
        void Unregister(IConnection connection);
        void UnregisterMessageClient(IMessageClient messageClient);

        int ClientLeaseTime { get; }

        ICollection Connections { get; }

        string Id { get; }

        ICollection Scopes { get; }

        object SyncRoot { get; }
    }
}

