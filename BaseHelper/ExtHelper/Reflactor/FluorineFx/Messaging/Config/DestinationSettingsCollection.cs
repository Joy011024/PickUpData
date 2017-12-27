namespace FluorineFx.Messaging.Config
{
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class DestinationSettingsCollection : CollectionBase
    {
        private Hashtable _destinationDictionary = new Hashtable();

        public int Add(DestinationSettings value)
        {
            this._destinationDictionary[value.Id] = value;
            return base.List.Add(value);
        }

        public bool Contains(DestinationSettings value)
        {
            return base.List.Contains(value);
        }

        public bool ContainsKey(string key)
        {
            return this._destinationDictionary.ContainsKey(key);
        }

        public int IndexOf(DestinationSettings value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, DestinationSettings value)
        {
            this._destinationDictionary[value.Id] = value;
            base.List.Insert(index, value);
        }

        public void Remove(DestinationSettings value)
        {
            this._destinationDictionary.Remove(value.Id);
            base.List.Remove(value);
        }

        public DestinationSettings this[int index]
        {
            get
            {
                return (base.List[index] as DestinationSettings);
            }
            set
            {
                base.List[index] = value;
            }
        }

        public DestinationSettings this[string key]
        {
            get
            {
                return (this._destinationDictionary[key] as DestinationSettings);
            }
            set
            {
                this._destinationDictionary[key] = value;
            }
        }
    }
}

