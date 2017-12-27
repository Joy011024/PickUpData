namespace FluorineFx.Messaging.Api
{
    using System;

    public interface IMessageClient
    {
        byte[] GetBinaryId();
        void Renew();

        string ClientId { get; }

        bool IsDisconnecting { get; }

        object SyncRoot { get; }
    }
}

