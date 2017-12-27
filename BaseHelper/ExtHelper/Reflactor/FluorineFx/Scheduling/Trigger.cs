namespace FluorineFx.Scheduling
{
    using FluorineFx.Util;
    using System;

    public class Trigger : IComparable
    {
        private bool _complete;
        private DateTime? _endTimeUtc;
        private string _jobName;
        private int _misfireInstruction;
        private string _name;
        private DateTime? _nextFireTimeUtc;
        private DateTime? _previousFireTimeUtc;
        private int _priority;
        private int _repeatCount;
        private long _repeatInterval;
        private DateTime _startTimeUtc;
        private int _timesTriggered;
        public const int DefaultPriority = 5;
        public const int RepeatIndefinitely = -1;

        public Trigger(string name, string jobName) : this(name, jobName, DateTime.UtcNow, null, 0, 0L)
        {
        }

        public Trigger(string name, string jobName, DateTime startTimeUtc) : this(name, jobName, startTimeUtc, null, 0, 0L)
        {
        }

        public Trigger(string name, string jobName, int repeatCount, long repeatInterval) : this(name, jobName, DateTime.UtcNow, null, repeatCount, repeatInterval)
        {
        }

        public Trigger(string name, string jobName, DateTime startTimeUtc, DateTime? endTimeUtc, int repeatCount, long repeatInterval)
        {
            this._nextFireTimeUtc = null;
            this._previousFireTimeUtc = null;
            this._repeatCount = 0;
            this._timesTriggered = 0;
            this._complete = false;
            this._priority = 5;
            this._misfireInstruction = 0;
            this._name = name;
            this._jobName = jobName;
            this.StartTimeUtc = startTimeUtc;
            this.EndTimeUtc = endTimeUtc;
            this.RepeatInterval = repeatInterval;
            this.RepeatCount = repeatCount;
        }

        public int CompareTo(object obj)
        {
            Trigger trigger = (Trigger) obj;
            DateTime? nextFireTimeUtc = this.GetNextFireTimeUtc();
            DateTime? nullable2 = trigger.GetNextFireTimeUtc();
            if (nextFireTimeUtc.HasValue || nullable2.HasValue)
            {
                if (!nextFireTimeUtc.HasValue)
                {
                    return 1;
                }
                if (!nullable2.HasValue)
                {
                    return -1;
                }
                if (nextFireTimeUtc.Value < nullable2.Value)
                {
                    return -1;
                }
                if (nextFireTimeUtc.Value > nullable2.Value)
                {
                    return 1;
                }
            }
            return 0;
        }

        public DateTime? ComputeFirstFireTimeUtc()
        {
            this._nextFireTimeUtc = new DateTime?(this.StartTimeUtc);
            return this._nextFireTimeUtc;
        }

        public virtual int ComputeNumTimesFiredBetween(DateTime? startTimeUtc, DateTime? endTimeUtc)
        {
            startTimeUtc = DateTimeUtils.AssumeUniversalTime(startTimeUtc);
            endTimeUtc = DateTimeUtils.AssumeUniversalTime(endTimeUtc);
            TimeSpan span = endTimeUtc.Value - startTimeUtc.Value;
            long totalMilliseconds = (long) span.TotalMilliseconds;
            return (int) (totalMilliseconds / this._repeatInterval);
        }

        public virtual SchedulerInstruction ExecutionComplete(ScheduledJobContext context, JobExecutionException result)
        {
            if ((result != null) && result.RefireImmediately)
            {
                return SchedulerInstruction.ReExecuteJob;
            }
            if ((result != null) && result.UnscheduleFiringTrigger)
            {
                return SchedulerInstruction.SetTriggerComplete;
            }
            if ((result != null) && result.UnscheduleAllTriggers)
            {
                return SchedulerInstruction.SetAllJobTriggersComplete;
            }
            if (((result == null) || result.RefireImmediately) && !this.GetMayFireAgain())
            {
                return SchedulerInstruction.DeleteTrigger;
            }
            return SchedulerInstruction.NoInstruction;
        }

        public DateTime? GetFireTimeAfter(DateTime? afterTimeUtc)
        {
            afterTimeUtc = DateTimeUtils.AssumeUniversalTime(afterTimeUtc);
            if (this._complete)
            {
                return null;
            }
            if ((this._timesTriggered > this._repeatCount) && (this._repeatCount != -1))
            {
                return null;
            }
            if (!afterTimeUtc.HasValue)
            {
                afterTimeUtc = new DateTime?(DateTime.UtcNow);
            }
            if ((this._repeatCount == 0) && (afterTimeUtc.Value.CompareTo(this.StartTimeUtc) >= 0))
            {
                return null;
            }
            DateTime startTimeUtc = this.StartTimeUtc;
            DateTime time2 = afterTimeUtc.Value;
            DateTime? endTimeUtc = this.EndTimeUtc;
            DateTime time3 = !endTimeUtc.HasValue ? DateTime.MaxValue : (endTimeUtc = this.EndTimeUtc).Value;
            if (time3 <= time2)
            {
                return null;
            }
            if (time2 < startTimeUtc)
            {
                return new DateTime?(startTimeUtc);
            }
            TimeSpan span = (TimeSpan) (time2 - startTimeUtc);
            long num = (((long) span.TotalMilliseconds) / this._repeatInterval) + 1L;
            if ((num > this._repeatCount) && (this._repeatCount != -1))
            {
                return null;
            }
            DateTime time4 = startTimeUtc.AddMilliseconds((double) (num * this._repeatInterval));
            if (time3 <= time4)
            {
                return null;
            }
            return new DateTime?(time4);
        }

        public virtual DateTime? GetFireTimeBefore(DateTime? endUtc)
        {
            endUtc = DateTimeUtils.AssumeUniversalTime(endUtc);
            if (endUtc.Value < this.StartTimeUtc)
            {
                return null;
            }
            int num = this.ComputeNumTimesFiredBetween(new DateTime?(this.StartTimeUtc), endUtc);
            return new DateTime?(this.StartTimeUtc.AddMilliseconds((double) (num * this._repeatInterval)));
        }

        public bool GetMayFireAgain()
        {
            return this.GetNextFireTimeUtc().HasValue;
        }

        public DateTime? GetNextFireTimeUtc()
        {
            return this._nextFireTimeUtc;
        }

        public DateTime? GetPreviousFireTimeUtc()
        {
            return this._previousFireTimeUtc;
        }

        public void SetNextFireTime(DateTime? fireTimeUtc)
        {
            this._nextFireTimeUtc = DateTimeUtils.AssumeUniversalTime(fireTimeUtc);
        }

        public virtual void SetPreviousFireTime(DateTime? fireTimeUtc)
        {
            this._previousFireTimeUtc = DateTimeUtils.AssumeUniversalTime(fireTimeUtc);
        }

        public void Triggered()
        {
            this._timesTriggered++;
            this._previousFireTimeUtc = this._nextFireTimeUtc;
            this._nextFireTimeUtc = this.GetFireTimeAfter(this._nextFireTimeUtc);
        }

        public void UpdateAfterMisfire()
        {
            DateTime? fireTimeAfter;
            int num2;
            DateTime utcNow;
            DateTime? endTimeUtc;
            int misfireInstruction = this.MisfireInstruction;
            if (misfireInstruction == 0)
            {
                if (this.RepeatCount == 0)
                {
                    misfireInstruction = 1;
                }
                else if (this.RepeatCount == -1)
                {
                    misfireInstruction = 4;
                }
                else
                {
                    misfireInstruction = 2;
                }
            }
            else if ((misfireInstruction == 1) && (this.RepeatCount != 0))
            {
                misfireInstruction = 3;
            }
            switch (misfireInstruction)
            {
                case 5:
                    fireTimeAfter = this.GetFireTimeAfter(new DateTime?(DateTime.UtcNow));
                    this.SetNextFireTime(fireTimeAfter);
                    break;

                case 4:
                    fireTimeAfter = this.GetFireTimeAfter(new DateTime?(DateTime.UtcNow));
                    if (fireTimeAfter.HasValue)
                    {
                        num2 = this.ComputeNumTimesFiredBetween(this._nextFireTimeUtc, fireTimeAfter);
                        this.TimesTriggered += num2;
                    }
                    this.SetNextFireTime(fireTimeAfter);
                    break;

                case 2:
                    utcNow = DateTime.UtcNow;
                    if ((this._repeatCount != 0) && (this._repeatCount != -1))
                    {
                        this.RepeatCount -= this.TimesTriggered;
                        this.TimesTriggered = 0;
                    }
                    endTimeUtc = this.EndTimeUtc;
                    if (endTimeUtc.HasValue && ((endTimeUtc = this.EndTimeUtc).Value < utcNow))
                    {
                        this.SetNextFireTime(null);
                    }
                    else
                    {
                        this.StartTimeUtc = utcNow;
                        this.SetNextFireTime(new DateTime?(utcNow));
                    }
                    break;

                case 1:
                    this.SetNextFireTime(new DateTime?(DateTime.UtcNow));
                    break;

                default:
                    if (misfireInstruction == 3)
                    {
                        utcNow = DateTime.UtcNow;
                        num2 = this.ComputeNumTimesFiredBetween(this._nextFireTimeUtc, new DateTime?(utcNow));
                        if ((this._repeatCount != 0) && (this._repeatCount != -1))
                        {
                            int num3 = this.RepeatCount - (this.TimesTriggered + num2);
                            if (num3 <= 0)
                            {
                                num3 = 0;
                            }
                            this.RepeatCount = num3;
                            this.TimesTriggered = 0;
                        }
                        endTimeUtc = this.EndTimeUtc;
                        if (endTimeUtc.HasValue && ((endTimeUtc = this.EndTimeUtc).Value < utcNow))
                        {
                            this.SetNextFireTime(null);
                        }
                        else
                        {
                            this.StartTimeUtc = utcNow;
                            this.SetNextFireTime(new DateTime?(utcNow));
                        }
                    }
                    break;
            }
        }

        public DateTime? EndTimeUtc
        {
            get
            {
                return this._endTimeUtc;
            }
            set
            {
                this._endTimeUtc = value;
                DateTime startTimeUtc = this.StartTimeUtc;
                if (value.HasValue && (startTimeUtc > value.Value))
                {
                    throw new ArgumentException("End time cannot be before start time");
                }
                this._endTimeUtc = DateTimeUtils.AssumeUniversalTime(value);
            }
        }

        public DateTime? FinalFireTimeUtc
        {
            get
            {
                DateTime? nullable2;
                if (this._repeatCount == 0)
                {
                    return new DateTime?(this.StartTimeUtc);
                }
                if (!((this._repeatCount != -1) || (nullable2 = this.EndTimeUtc).HasValue))
                {
                    return null;
                }
                if (!((this._repeatCount != -1) || (nullable2 = this.EndTimeUtc).HasValue))
                {
                    return null;
                }
                if (this._repeatCount != -1)
                {
                    DateTime time = this.StartTimeUtc.AddMilliseconds((double) (this._repeatCount * this._repeatInterval));
                    if (!(this.EndTimeUtc.HasValue && (time >= this.EndTimeUtc.Value)))
                    {
                        return new DateTime?(time);
                    }
                }
                return this.GetFireTimeBefore(this.EndTimeUtc);
            }
        }

        public bool HasMillisecondPrecision
        {
            get
            {
                return true;
            }
        }

        public string JobName
        {
            get
            {
                return this._jobName;
            }
            set
            {
                this._jobName = value;
            }
        }

        public virtual int MisfireInstruction
        {
            get
            {
                return this._misfireInstruction;
            }
            set
            {
                this._misfireInstruction = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public int Priority
        {
            get
            {
                return this._priority;
            }
            set
            {
                this._priority = value;
            }
        }

        public int RepeatCount
        {
            get
            {
                return this._repeatCount;
            }
            set
            {
                if ((value < 0) && (value != -1))
                {
                    throw new ArgumentException("Repeat count must be >= 0, use the constant RepeatIndefinitely for infinite.");
                }
                this._repeatCount = value;
            }
        }

        public long RepeatInterval
        {
            get
            {
                return this._repeatInterval;
            }
            set
            {
                if (value < 0L)
                {
                    throw new ArgumentException("Repeat interval must be >= 0");
                }
                this._repeatInterval = value;
            }
        }

        public DateTime StartTimeUtc
        {
            get
            {
                return this._startTimeUtc;
            }
            set
            {
                if (this.EndTimeUtc.HasValue && (this.EndTimeUtc.Value < value))
                {
                    throw new ArgumentException("End time cannot be before start time");
                }
                if (this.HasMillisecondPrecision)
                {
                    this._startTimeUtc = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
                }
                else
                {
                    this._startTimeUtc = value;
                }
                this._startTimeUtc = DateTimeUtils.AssumeUniversalTime(this._startTimeUtc);
            }
        }

        public virtual int TimesTriggered
        {
            get
            {
                return this._timesTriggered;
            }
            set
            {
                this._timesTriggered = value;
            }
        }
    }
}

