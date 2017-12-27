namespace FluorineFx.Messaging.Rtmp.Persistence
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Persistence;
    using System;
    using System.Collections;

    internal class MemoryStore : IPersistenceStore
    {
        protected Hashtable _objects = new Hashtable();
        protected IScope _scope;
        private object _syncLock = new object();
        public const string PersistenceNoName = "__null__";

        public MemoryStore(IScope scope)
        {
            this._scope = scope;
        }

        protected string GetObjectId(IPersistable obj)
        {
            string name = obj.GetType().Name;
            if (!obj.Path.StartsWith("/"))
            {
                name = name + "/";
            }
            name = name + obj.Path;
            if (!name.EndsWith("/"))
            {
                name = name + "/";
            }
            string str2 = obj.Name;
            if (str2 == null)
            {
                str2 = "__null__";
            }
            if (str2.StartsWith("/"))
            {
                str2 = str2.Substring(1);
            }
            return (name + str2);
        }

        protected string GetObjectName(string id)
        {
            string str = id.Substring(id.LastIndexOf('/') + 1);
            if (str.Equals("__null__"))
            {
                str = null;
            }
            return str;
        }

        public ICollection GetObjectNames()
        {
            return this._objects.Keys;
        }

        protected string GetObjectPath(string id, string name)
        {
            id = id.Substring(id.IndexOf('/') + 1);
            if (id.StartsWith("/"))
            {
                id = id.Substring(1);
            }
            if (id.LastIndexOf(name) == -1)
            {
                return id;
            }
            return id.Substring(0, id.LastIndexOf(name) - 1);
        }

        public ICollection GetObjects()
        {
            return this._objects.Values;
        }

        public virtual bool Load(IPersistable obj)
        {
            return obj.IsPersistent;
        }

        public virtual IPersistable Load(string name)
        {
            return (this._objects[name] as IPersistable);
        }

        public virtual bool Remove(IPersistable obj)
        {
            return this.Remove(this.GetObjectId(obj));
        }

        public virtual bool Remove(string name)
        {
            if (!this._objects.ContainsKey(name))
            {
                return false;
            }
            IPersistable persistable = this._objects[name] as IPersistable;
            this._objects.Remove(name);
            persistable.IsPersistent = false;
            return true;
        }

        public virtual bool Save(IPersistable obj)
        {
            this._objects[this.GetObjectId(obj)] = obj;
            obj.IsPersistent = true;
            return true;
        }

        public object SyncRoot
        {
            get
            {
                return this._syncLock;
            }
        }
    }
}

