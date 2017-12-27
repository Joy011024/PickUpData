namespace FluorineFx.Scheduling
{
    using System;

    internal enum InternalTriggerState
    {
        Waiting,
        Acquired,
        Executing,
        Complete,
        Paused,
        Blocked,
        PausedAndBlocked,
        Error
    }
}

