namespace FluorineFx.Messaging.Api
{
    using System;

    public interface IBandwidthConfigure
    {
        long[] GetChannelBandwidth();
        long[] GetChannelInitialBurst();
    }
}

