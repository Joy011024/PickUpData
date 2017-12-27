namespace FluorineFx.Messaging.Config
{
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class ServiceSettingsCollection : CollectionBase
    {
        private Hashtable _serviceDictionary = new Hashtable();

        public int Add(ServiceSettings value)
        {
            this._serviceDictionary[value.Id] = value;
            return base.List.Add(value);
        }

        public bool Contains(ServiceSettings value)
        {
            return base.List.Contains(value);
        }

        public int IndexOf(ServiceSettings value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, ServiceSettings value)
        {
            this._serviceDictionary[value.Id] = value;
            base.List.Insert(index, value);
        }

        public void Remove(ServiceSettings value)
        {
            this._serviceDictionary.Remove(value.Id);
            base.List.Remove(value);
        }

        public ServiceSettings this[int index]
        {
            get
            {
                return (base.List[index] as ServiceSettings);
            }
            set
            {
                base.List[index] = value;
            }
        }

        public ServiceSettings this[string key]
        {
            get
            {
                return (this._serviceDictionary[key] as ServiceSettings);
            }
            set
            {
                this._serviceDictionary[key] = value;
            }
        }
    }
}

