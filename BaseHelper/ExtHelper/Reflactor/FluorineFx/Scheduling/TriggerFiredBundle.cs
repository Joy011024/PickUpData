namespace FluorineFx.Scheduling
{
    using FluorineFx.Util;
    using System;

    [Serializable]
    internal class TriggerFiredBundle
    {
        private DateTime? _fireTimeUtc;
        private readonly IScheduledJob _job;
        private readonly bool _jobIsRecovering;
        private DateTime? _nextFireTimeUtc;
        private DateTime? _prevFireTimeUtc;
        private DateTime? _scheduledFireTimeUtc;
        private readonly FluorineFx.Scheduling.Trigger _trigger;

        public TriggerFiredBundle(IScheduledJob job, FluorineFx.Scheduling.Trigger trigger, bool jobIsRecovering, DateTime? fireTimeUtc, DateTime? scheduledFireTimeUtc, DateTime? prevFireTimeUtc, DateTime? nextFireTimeUtc)
        {
            this._job = job;
            this._trigger = trigger;
            this._jobIsRecovering = jobIsRecovering;
            this._fireTimeUtc = DateTimeUtils.AssumeUniversalTime(fireTimeUtc);
            this._scheduledFireTimeUtc = DateTimeUtils.AssumeUniversalTime(scheduledFireTimeUtc);
            this._prevFireTimeUtc = DateTimeUtils.AssumeUniversalTime(prevFireTimeUtc);
            this._nextFireTimeUtc = DateTimeUtils.AssumeUniversalTime(nextFireTimeUtc);
        }

        public virtual DateTime? FireTimeUtc
        {
            get
            {
                return this._fireTimeUtc;
            }
        }

        public virtual IScheduledJob Job
        {
            get
            {
                return this._job;
            }
        }

        public virtual DateTime? NextFireTimeUtc
        {
            get
            {
                return this._nextFireTimeUtc;
            }
        }

        public virtual DateTime? PrevFireTimeUtc
        {
            get
            {
                return this._prevFireTimeUtc;
            }
        }

        public virtual bool Recovering
        {
            get
            {
                return this._jobIsRecovering;
            }
        }

        public virtual DateTime? ScheduledFireTimeUtc
        {
            get
            {
                return this._scheduledFireTimeUtc;
            }
        }

        public virtual FluorineFx.Scheduling.Trigger Trigger
        {
            get
            {
                return this._trigger;
            }
        }
    }
}

