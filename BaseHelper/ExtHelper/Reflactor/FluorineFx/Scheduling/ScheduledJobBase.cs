namespace FluorineFx.Scheduling
{
    using System;
    using System.Collections;

    public abstract class ScheduledJobBase : IScheduledJob
    {
        private Hashtable _jobDataMap;
        private string _name;

        public ScheduledJobBase()
        {
            this._name = "job" + Guid.NewGuid().ToString("N");
        }

        public ScheduledJobBase(string name)
        {
            this._name = name;
        }

        public abstract void Execute(ScheduledJobContext context);
        public override string ToString()
        {
            return this._name;
        }

        public Hashtable JobDataMap
        {
            get
            {
                if (this._jobDataMap == null)
                {
                    this._jobDataMap = new Hashtable();
                }
                return this._jobDataMap;
            }
            set
            {
                this._jobDataMap = value;
            }
        }

        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if ((value == null) || (value.Trim().Length == 0))
                {
                    throw new ArgumentException("Job name cannot be empty.");
                }
                this._name = value;
            }
        }
    }
}

