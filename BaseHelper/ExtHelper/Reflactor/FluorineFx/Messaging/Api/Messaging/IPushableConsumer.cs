namespace FluorineFx.Messaging.Api.Messaging
{
    using FluorineFx.Messaging.Messages;
    using System;

    [CLSCompliant(false)]
    public interface IPushableConsumer : IConsumer, IMessageComponent
    {
        void PushMessage(IPipe pipe, IMessage message);
    }
}

