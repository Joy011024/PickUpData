namespace FluorineFx.Messaging.Rtmp.Messaging
{
    using FluorineFx.Messaging.Api.Messaging;
    using System;

    [CLSCompliant(false)]
    public interface IFilter : IConsumer, IProvider, IMessageComponent
    {
    }
}

