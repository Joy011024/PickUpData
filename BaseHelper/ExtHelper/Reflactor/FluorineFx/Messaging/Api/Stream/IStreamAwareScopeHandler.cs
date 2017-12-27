namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using System;

    [CLSCompliant(false)]
    public interface IStreamAwareScopeHandler : IScopeHandler, IEventHandler
    {
        void StreamBroadcastClose(IBroadcastStream stream);
        void StreamBroadcastStart(IBroadcastStream stream);
        void StreamPlaylistItemPlay(IPlaylistSubscriberStream stream, IPlayItem item, bool isLive);
        void StreamPlaylistItemStop(IPlaylistSubscriberStream stream, IPlayItem item);
        void StreamPlaylistVODItemPause(IPlaylistSubscriberStream stream, IPlayItem item, int position);
        void StreamPlaylistVODItemResume(IPlaylistSubscriberStream stream, IPlayItem item, int position);
        void StreamPlaylistVODItemSeek(IPlaylistSubscriberStream stream, IPlayItem item, int position);
        void StreamPublishStart(IBroadcastStream stream);
        void StreamRecordStart(IBroadcastStream stream);
        void StreamSubscriberClose(ISubscriberStream stream);
        void StreamSubscriberStart(ISubscriberStream stream);
    }
}

