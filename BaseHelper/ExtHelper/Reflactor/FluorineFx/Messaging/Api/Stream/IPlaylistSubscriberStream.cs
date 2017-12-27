namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Statistics;
    using System;

    [CLSCompliant(false)]
    public interface IPlaylistSubscriberStream : ISubscriberStream, IClientStream, IStream, IBWControllable, IPlaylist
    {
        IPlaylistSubscriberStreamStatistics Statistics { get; }
    }
}

