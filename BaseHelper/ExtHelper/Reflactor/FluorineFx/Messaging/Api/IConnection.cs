namespace FluorineFx.Messaging.Api
{
    using FluorineFx;
    using FluorineFx.Messaging.Api.Event;
    using System;
    using System.Collections;
    using System.Net;

    public interface IConnection : ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener
    {
        void Close();
        bool Connect(IScope scope);
        bool Connect(IScope scope, object[] args);
        void Initialize(IClient client);
        void Ping();
        void Timeout();
        string ToString();

        IEnumerator BasicScopes { get; }

        IClient Client { get; }

        long ClientBytesRead { get; }

        int ClientLeaseTime { get; }

        string ConnectionId { get; }

        long DroppedMessages { get; }

        bool IsConnected { get; }

        bool IsFlexClient { get; }

        int LastPingTime { get; }

        FluorineFx.ObjectEncoding ObjectEncoding { get; }

        IDictionary Parameters { get; }

        string Path { get; }

        long PendingMessages { get; }

        long ReadBytes { get; }

        long ReadMessages { get; }

        IPEndPoint RemoteEndPoint { get; }

        IScope Scope { get; }

        string SessionId { get; }

        object SyncRoot { get; }

        long WrittenBytes { get; }

        long WrittenMessages { get; }
    }
}

