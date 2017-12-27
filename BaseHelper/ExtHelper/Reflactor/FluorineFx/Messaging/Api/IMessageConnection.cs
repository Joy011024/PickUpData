namespace FluorineFx.Messaging.Api
{
    using FluorineFx.Messaging.Messages;
    using System;

    internal interface IMessageConnection
    {
        bool IsClientRegistered(string clientId);
        void Push(IMessage message, IMessageClient messageClient);
        void RegisterMessageClient(IMessageClient client);
        void RemoveMessageClient(string clientId);
        void RemoveMessageClients();

        int ClientCount { get; }
    }
}

