namespace FluorineFx.Messaging.Config
{
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class ChannelSettingsCollection : CollectionBase
    {
        private Hashtable _channelDictionary = new Hashtable();

        public int Add(ChannelSettings value)
        {
            this._channelDictionary[value.Id] = value;
            return base.List.Add(value);
        }

        public bool Contains(ChannelSettings value)
        {
            return base.List.Contains(value);
        }

        public int IndexOf(ChannelSettings value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, ChannelSettings value)
        {
            this._channelDictionary[value.Id] = value;
            base.List.Insert(index, value);
        }

        public void Remove(ChannelSettings value)
        {
            this._channelDictionary.Remove(value.Id);
            base.List.Remove(value);
        }

        public ChannelSettings this[int index]
        {
            get
            {
                return (base.List[index] as ChannelSettings);
            }
            set
            {
                base.List[index] = value;
            }
        }

        public ChannelSettings this[string key]
        {
            get
            {
                return (this._channelDictionary[key] as ChannelSettings);
            }
            set
            {
                this._channelDictionary[key] = value;
            }
        }
    }
}

