namespace FluorineFx.Messaging.Rtmp.SO
{
    using System;

    public class SharedObjectEvent : ISharedObjectEvent
    {
        private string _key;
        private SharedObjectEventType _type;
        private object _value;

        public SharedObjectEvent(SharedObjectEventType type, string key, object value)
        {
            this._type = type;
            this._key = key;
            this._value = value;
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "SOEvent(", this._type, ", ", this._key, ", ", this._value, ")" });
        }

        public string Key
        {
            get
            {
                return this._key;
            }
        }

        public SharedObjectEventType Type
        {
            get
            {
                return this._type;
            }
        }

        public object Value
        {
            get
            {
                return this._value;
            }
        }
    }
}

