namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api;
    using System;

    [CLSCompliant(false)]
    public interface ISingleItemSubscriberStream : ISubscriberStream, IClientStream, IStream, IBWControllable
    {
        IPlayItem PlayItem { set; }
    }
}

