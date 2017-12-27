namespace FluorineFx.Scheduling
{
    using System;

    public enum SchedulerInstruction
    {
        NoInstruction,
        ReExecuteJob,
        SetTriggerComplete,
        DeleteTrigger,
        SetAllJobTriggersComplete,
        SetAllJobTriggersError,
        SetTriggerError
    }
}

