namespace FluorineFx.Scheduling
{
    using FluorineFx.Messaging.Api;
    using System;

    public interface ISchedulingService : IScopeService, IService
    {
        string AddScheduledJob(int interval, IScheduledJob job);
        string AddScheduledJob(int interval, int repeatCount, IScheduledJob job);
        string AddScheduledOnceJob(DateTime date, IScheduledJob job);
        string AddScheduledOnceJob(long timeDelta, IScheduledJob job);
        string[] GetScheduledJobNames();
        bool RemoveScheduledJob(string jobName);
        DateTime ScheduleJob(IScheduledJob job, Trigger trigger);
    }
}

