namespace FluorineFx.Messaging
{
    using FluorineFx.IO;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Persistence;
    using System;
    using System.Collections.Generic;

    internal class PersistableAttributeStore : AttributeStore, IPersistable
    {
        protected long _lastModified = -1L;
        protected string _name;
        protected string _path;
        protected bool _persistent = true;
        protected IPersistenceStore _store = null;
        protected string _type;

        public PersistableAttributeStore(string type, string name, string path, bool persistent)
        {
            this._name = name;
            this._path = path;
            this._type = type;
            this._persistent = persistent;
        }

        public void Deserialize(AMFReader reader)
        {
        }

        protected void OnModified()
        {
            this._lastModified = Environment.TickCount;
            if (this._store != null)
            {
                this._store.Save(this);
            }
        }

        public override bool RemoveAttribute(string name)
        {
            bool flag = base.RemoveAttribute(name);
            if (!(!flag || name.StartsWith("_transient")))
            {
                this.OnModified();
            }
            return flag;
        }

        public override void RemoveAttributes()
        {
            base.RemoveAttributes();
            this.OnModified();
        }

        public void Serialize(AMFWriter writer)
        {
        }

        public override bool SetAttribute(string name, object value)
        {
            bool flag = base.SetAttribute(name, value);
            if (!(!flag || name.StartsWith("_transient")))
            {
                this.OnModified();
            }
            return flag;
        }

        public override void SetAttributes(IAttributeStore values)
        {
            base.SetAttributes(values);
            this.OnModified();
        }

        public override void SetAttributes(IDictionary<string, object> values)
        {
            base.SetAttributes(values);
            this.OnModified();
        }

        public virtual bool IsPersistent
        {
            get
            {
                return this._persistent;
            }
            set
            {
                this._persistent = value;
            }
        }

        public virtual long LastModified
        {
            get
            {
                return this._lastModified;
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
                this._name = value;
            }
        }

        public virtual string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                this._path = value;
            }
        }

        public virtual IPersistenceStore Store
        {
            get
            {
                return this._store;
            }
            set
            {
                this._store = value;
                if (this._store != null)
                {
                    this._store.Load(this);
                }
            }
        }

        public virtual string Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }
    }
}

