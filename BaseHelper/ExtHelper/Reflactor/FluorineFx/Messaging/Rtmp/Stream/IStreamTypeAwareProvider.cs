namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Api.Messaging;
    using System;

    [CLSCompliant(false)]
    public interface IStreamTypeAwareProvider : IProvider, IMessageComponent
    {
        bool HasVideo();
    }
}

