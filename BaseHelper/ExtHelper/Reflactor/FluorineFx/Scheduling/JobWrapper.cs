namespace FluorineFx.Scheduling
{
    using System;

    internal class JobWrapper
    {
        private IScheduledJob _job;

        internal JobWrapper(IScheduledJob job)
        {
            this._job = job;
        }

        public override bool Equals(object obj)
        {
            if (obj is JobWrapper)
            {
                JobWrapper wrapper = (JobWrapper) obj;
                if (wrapper._job.Equals(this._job))
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this._job.Name.GetHashCode();
        }

        public IScheduledJob Job
        {
            get
            {
                return this._job;
            }
            set
            {
                this._job = value;
            }
        }

        public string Name
        {
            get
            {
                return this._job.Name;
            }
        }
    }
}

