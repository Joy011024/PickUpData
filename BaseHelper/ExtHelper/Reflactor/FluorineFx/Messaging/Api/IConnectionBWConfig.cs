namespace FluorineFx.Messaging.Api
{
    using System;

    public interface IConnectionBWConfig : IBandwidthConfigure
    {
        long DownstreamBandwidth { get; }

        long UpstreamBandwidth { get; set; }
    }
}

