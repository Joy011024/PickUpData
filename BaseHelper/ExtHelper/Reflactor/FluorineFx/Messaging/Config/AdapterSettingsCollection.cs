namespace FluorineFx.Messaging.Config
{
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class AdapterSettingsCollection : CollectionBase
    {
        private Hashtable _adapterDictionary = new Hashtable();

        public int Add(AdapterSettings value)
        {
            this._adapterDictionary[value.Id] = value;
            return base.List.Add(value);
        }

        public bool Contains(AdapterSettings value)
        {
            return base.List.Contains(value);
        }

        public int IndexOf(AdapterSettings value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, AdapterSettings value)
        {
            this._adapterDictionary[value.Id] = value;
            base.List.Insert(index, value);
        }

        public void Remove(AdapterSettings value)
        {
            this._adapterDictionary.Remove(value.Id);
            base.List.Remove(value);
        }

        public AdapterSettings this[int index]
        {
            get
            {
                return (base.List[index] as AdapterSettings);
            }
            set
            {
                base.List[index] = value;
            }
        }

        public AdapterSettings this[string key]
        {
            get
            {
                return (this._adapterDictionary[key] as AdapterSettings);
            }
            set
            {
                this._adapterDictionary[key] = value;
            }
        }
    }
}

