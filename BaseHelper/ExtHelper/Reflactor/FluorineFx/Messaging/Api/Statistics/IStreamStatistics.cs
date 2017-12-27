namespace FluorineFx.Messaging.Api.Statistics
{
    using System;

    public interface IStreamStatistics : IStatisticsBase
    {
        int CurrentTimestamp { get; }
    }
}

