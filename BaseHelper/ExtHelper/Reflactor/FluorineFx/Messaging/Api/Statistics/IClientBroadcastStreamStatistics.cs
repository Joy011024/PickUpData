namespace FluorineFx.Messaging.Api.Statistics
{
    using System;

    public interface IClientBroadcastStreamStatistics : IStreamStatistics, IStatisticsBase
    {
        int ActiveSubscribers { get; }

        long BytesReceived { get; }

        int MaxSubscribers { get; }

        string PublishedName { get; }

        string SaveFilename { get; }

        int TotalSubscribers { get; }
    }
}

