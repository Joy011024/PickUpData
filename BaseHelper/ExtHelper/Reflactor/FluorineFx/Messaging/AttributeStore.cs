namespace FluorineFx.Messaging
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Util;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    [CLSCompliant(false)]
    public class AttributeStore : DisposableBase, IAttributeStore
    {
        protected Dictionary<string, object> _attributes = new Dictionary<string, object>();

        public void CopyTo(object[] array, int index)
        {
            lock (((ICollection) this._attributes).SyncRoot)
            {
                this._attributes.Values.CopyTo(array, index);
            }
        }

        public virtual object GetAttribute(string name)
        {
            if (name != null)
            {
                lock (((ICollection) this._attributes).SyncRoot)
                {
                    if (this._attributes.ContainsKey(name))
                    {
                        return this._attributes[name];
                    }
                }
            }
            return null;
        }

        public virtual object GetAttribute(string name, object defaultValue)
        {
            if (name == null)
            {
                return null;
            }
            if (defaultValue == null)
            {
                throw new NullReferenceException("The default value may not be null.");
            }
            lock (((ICollection) this._attributes).SyncRoot)
            {
                if (this._attributes.ContainsKey(name))
                {
                    return this._attributes[name];
                }
                this._attributes[name] = defaultValue;
                return null;
            }
        }

        public virtual ICollection GetAttributeNames()
        {
            lock (((ICollection) this._attributes).SyncRoot)
            {
                return this._attributes.Keys;
            }
        }

        public virtual bool HasAttribute(string name)
        {
            if (name == null)
            {
                return false;
            }
            lock (((ICollection) this._attributes).SyncRoot)
            {
                return this._attributes.ContainsKey(name);
            }
        }

        public virtual bool RemoveAttribute(string name)
        {
            lock (((ICollection) this._attributes).SyncRoot)
            {
                if (this.HasAttribute(name))
                {
                    this._attributes.Remove(name);
                    return true;
                }
                return false;
            }
        }

        public virtual void RemoveAttributes()
        {
            lock (((ICollection) this._attributes).SyncRoot)
            {
                this._attributes.Clear();
            }
        }

        public virtual bool SetAttribute(string name, object value)
        {
            if (name == null)
            {
                return false;
            }
            lock (((ICollection) this._attributes).SyncRoot)
            {
                object obj2 = null;
                if (this._attributes.ContainsKey(name))
                {
                    obj2 = this._attributes[name];
                }
                this._attributes[name] = value;
                return (((obj2 == null) || (value == obj2)) || !value.Equals(obj2));
            }
        }

        public virtual void SetAttributes(IAttributeStore values)
        {
            lock (((ICollection) this._attributes).SyncRoot)
            {
                foreach (string str in values.GetAttributeNames())
                {
                    object attribute = values.GetAttribute(str);
                    this.SetAttribute(str, attribute);
                }
            }
        }

        public virtual void SetAttributes(IDictionary<string, object> values)
        {
            lock (((ICollection) this._attributes).SyncRoot)
            {
                foreach (KeyValuePair<string, object> pair in values)
                {
                    this.SetAttribute(pair.Key, pair.Value);
                }
            }
        }

        public int AttributesCount
        {
            get
            {
                lock (((ICollection) this._attributes).SyncRoot)
                {
                    return this._attributes.Count;
                }
            }
        }

        public bool IsEmpty
        {
            get
            {
                lock (((ICollection) this._attributes).SyncRoot)
                {
                    return (this._attributes.Count == 0);
                }
            }
        }

        public object this[string name]
        {
            get
            {
                return this.GetAttribute(name);
            }
            set
            {
                this.SetAttribute(name, value);
            }
        }
    }
}

