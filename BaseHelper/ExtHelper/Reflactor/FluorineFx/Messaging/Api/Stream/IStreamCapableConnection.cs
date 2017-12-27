namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using System;
    using System.Collections;

    [CLSCompliant(false)]
    public interface IStreamCapableConnection : IConnection, ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener, IBWControllable
    {
        void DeleteStreamById(int streamId);
        long GetPendingVideoMessages(int streamId);
        IClientStream GetStreamByChannelId(int channelId);
        IClientStream GetStreamById(int streamId);
        ICollection GetStreams();
        IClientBroadcastStream NewBroadcastStream(int streamId);
        IPlaylistSubscriberStream NewPlaylistSubscriberStream(int streamId);
        ISingleItemSubscriberStream NewSingleItemSubscriberStream(int streamId);
        int ReserveStreamId();
        void UnreserveStreamId(int streamId);
    }
}

