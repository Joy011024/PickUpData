namespace FluorineFx.Messaging.Endpoints
{
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Messages;
    using System;

    public interface IEndpoint
    {
        MessageBroker GetMessageBroker();
        ChannelSettings GetSettings();
        void Push(IMessage message, MessageClient messageclient);
        void Service();
        IMessage ServiceMessage(IMessage message);
        void Start();
        void Stop();

        string Id { get; set; }

        bool IsSecure { get; }
    }
}

