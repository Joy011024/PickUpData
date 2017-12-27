namespace FluorineFx.Scheduling
{
    using System;
    using System.Collections;

    public interface IScheduledJob
    {
        void Execute(ScheduledJobContext context);

        Hashtable JobDataMap { get; set; }

        string Name { get; set; }
    }
}

