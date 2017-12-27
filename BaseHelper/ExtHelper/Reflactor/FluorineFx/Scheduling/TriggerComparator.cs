namespace FluorineFx.Scheduling
{
    using System;
    using System.Collections;

    internal class TriggerComparator : IComparer
    {
        public virtual int Compare(object obj1, object obj2)
        {
            TriggerWrapper wrapper = (TriggerWrapper) obj1;
            TriggerWrapper wrapper2 = (TriggerWrapper) obj2;
            int num = wrapper.Trigger.CompareTo(wrapper2.Trigger);
            if (num != 0)
            {
                return num;
            }
            num = wrapper2.Trigger.Priority - wrapper.Trigger.Priority;
            if (num != 0)
            {
                return num;
            }
            return wrapper.Trigger.Name.CompareTo(wrapper2.Trigger.Name);
        }

        public override bool Equals(object obj)
        {
            return (obj is TriggerComparator);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

