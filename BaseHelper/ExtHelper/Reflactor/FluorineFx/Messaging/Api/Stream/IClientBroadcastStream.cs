namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Statistics;
    using System;

    [CLSCompliant(false)]
    public interface IClientBroadcastStream : IClientStream, IBWControllable, IBroadcastStream, IStream
    {
        void StartPublishing();

        IClientBroadcastStreamStatistics Statistics { get; }
    }
}

