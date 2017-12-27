namespace FluorineFx.Scheduling
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    public class JobExecutionException : SchedulerException
    {
        private bool refire;
        private bool unscheduleAllTriggs;
        private bool unscheduleTrigg;

        public JobExecutionException()
        {
            this.refire = false;
            this.unscheduleTrigg = false;
            this.unscheduleAllTriggs = false;
        }

        public JobExecutionException(bool refireImmediately)
        {
            this.refire = false;
            this.unscheduleTrigg = false;
            this.unscheduleAllTriggs = false;
            this.refire = refireImmediately;
        }

        public JobExecutionException(Exception cause) : base(cause)
        {
            this.refire = false;
            this.unscheduleTrigg = false;
            this.unscheduleAllTriggs = false;
        }

        public JobExecutionException(string msg) : base(msg)
        {
            this.refire = false;
            this.unscheduleTrigg = false;
            this.unscheduleAllTriggs = false;
        }

        public JobExecutionException(Exception cause, bool refireImmediately) : base(cause)
        {
            this.refire = false;
            this.unscheduleTrigg = false;
            this.unscheduleAllTriggs = false;
            this.refire = refireImmediately;
        }

        public JobExecutionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.refire = false;
            this.unscheduleTrigg = false;
            this.unscheduleAllTriggs = false;
        }

        public JobExecutionException(string msg, Exception cause) : base(msg, cause)
        {
            this.refire = false;
            this.unscheduleTrigg = false;
            this.unscheduleAllTriggs = false;
        }

        public JobExecutionException(string msg, Exception cause, bool refireImmediately) : base(msg, cause)
        {
            this.refire = false;
            this.unscheduleTrigg = false;
            this.unscheduleAllTriggs = false;
            this.refire = refireImmediately;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Parameters: refire = {0}, unscheduleFiringTrigger = {1}, unscheduleAllTriggers = {2} \n {3}", new object[] { this.RefireImmediately, this.UnscheduleFiringTrigger, this.UnscheduleAllTriggers, base.ToString() });
        }

        public virtual bool RefireImmediately
        {
            get
            {
                return this.refire;
            }
            set
            {
                this.refire = value;
            }
        }

        public virtual bool UnscheduleAllTriggers
        {
            get
            {
                return this.unscheduleAllTriggs;
            }
            set
            {
                this.unscheduleAllTriggs = value;
            }
        }

        public virtual bool UnscheduleFiringTrigger
        {
            get
            {
                return this.unscheduleTrigg;
            }
            set
            {
                this.unscheduleTrigg = value;
            }
        }
    }
}

