namespace FluorineFx.Messaging.Api.Messaging
{
    using FluorineFx.Messaging.Messages;
    using System;

    [CLSCompliant(false)]
    public interface IPullableProvider : IProvider, IMessageComponent
    {
        IMessage PullMessage(IPipe pipe);
        IMessage PullMessage(IPipe pipe, long wait);
    }
}

