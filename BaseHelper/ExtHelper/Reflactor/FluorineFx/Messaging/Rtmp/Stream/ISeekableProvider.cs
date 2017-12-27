namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Api.Messaging;
    using System;

    [CLSCompliant(false)]
    public interface ISeekableProvider : IProvider, IMessageComponent
    {
        int Seek(int ts);
    }
}

