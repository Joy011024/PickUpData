namespace FluorineFx.Messaging.Services
{
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Messages;
    using System;

    [CLSCompliant(false)]
    public interface IService
    {
        [CLSCompliant(false)]
        void CheckSecurity(Destination destination);
        void CheckSecurity(IMessage message);
        bool DoAuthorization(string[] roles);
        [CLSCompliant(false)]
        Destination GetDestination(IMessage message);
        [CLSCompliant(false)]
        Destination GetDestination(string id);
        [CLSCompliant(false)]
        Destination[] GetDestinations();
        MessageBroker GetMessageBroker();
        bool IsSupportedMessage(IMessage message);
        bool IsSupportedMessageType(string messageClassName);
        object ServiceMessage(IMessage message);
        void Start();
        void Stop();

        string id { get; }
    }
}

