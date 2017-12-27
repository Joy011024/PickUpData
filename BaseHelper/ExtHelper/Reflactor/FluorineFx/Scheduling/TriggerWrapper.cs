namespace FluorineFx.Scheduling
{
    using System;

    [CLSCompliant(false)]
    public class TriggerWrapper
    {
        private InternalTriggerState _state = InternalTriggerState.Waiting;
        public FluorineFx.Scheduling.Trigger _trigger;

        internal TriggerWrapper(FluorineFx.Scheduling.Trigger trigger)
        {
            this._trigger = trigger;
        }

        public override bool Equals(object obj)
        {
            if (obj is TriggerWrapper)
            {
                TriggerWrapper wrapper = (TriggerWrapper) obj;
                if (wrapper.Name.Equals(this.Name))
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this._trigger.Name.GetHashCode();
        }

        public string JobName
        {
            get
            {
                return this._trigger.JobName;
            }
        }

        public string Name
        {
            get
            {
                return this._trigger.Name;
            }
        }

        internal InternalTriggerState State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        public FluorineFx.Scheduling.Trigger Trigger
        {
            get
            {
                return this._trigger;
            }
        }
    }
}

