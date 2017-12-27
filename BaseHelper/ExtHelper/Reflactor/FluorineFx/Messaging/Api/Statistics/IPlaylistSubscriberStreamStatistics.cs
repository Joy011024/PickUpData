namespace FluorineFx.Messaging.Api.Statistics
{
    using System;

    public interface IPlaylistSubscriberStreamStatistics : IStreamStatistics, IStatisticsBase
    {
        long BytesSent { get; }

        int ClientBufferDuration { get; }

        double EstimatedBufferFill { get; }
    }
}

