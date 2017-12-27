namespace FluorineFx.Messaging.Config
{
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class FactorySettingsCollection : CollectionBase
    {
        public int Add(FactorySettings value)
        {
            return base.List.Add(value);
        }

        public bool Contains(FactorySettings value)
        {
            return base.List.Contains(value);
        }

        public int IndexOf(FactorySettings value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, FactorySettings value)
        {
            base.List.Insert(index, value);
        }

        public void Remove(FactorySettings value)
        {
            base.List.Remove(value);
        }

        public FactorySettings this[int index]
        {
            get
            {
                return (base.List[index] as FactorySettings);
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}

