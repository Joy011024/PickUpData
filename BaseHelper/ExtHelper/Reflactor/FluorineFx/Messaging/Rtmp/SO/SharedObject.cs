namespace FluorineFx.Messaging.Rtmp.SO
{
    using FluorineFx;
    using FluorineFx.Collections;
    using FluorineFx.IO;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Persistence;
    using FluorineFx.Messaging.Rtmp;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class SharedObject : AttributeStore, IPersistable
    {
        private long _creationTime;
        protected Hashtable _hashes;
        protected long _lastModified;
        protected CopyOnWriteArray _listeners;
        protected bool _modified;
        protected string _name;
        protected SharedObjectMessage _ownerMessage;
        protected string _path;
        protected bool _persistent;
        protected bool _persistentSO;
        protected IEventListener _source;
        protected IPersistenceStore _storage;
        private List<ISharedObjectEvent> _syncEvents;
        protected int _updateCounter;
        protected int _version;
        private static readonly ILog log = LogManager.GetLogger(typeof(SharedObject));
        [NonSerialized]
        public const string TYPE = "SharedObject";

        public SharedObject()
        {
            this._name = string.Empty;
            this._path = string.Empty;
            this._lastModified = -1L;
            this._persistent = false;
            this._persistentSO = false;
            this._hashes = new Hashtable();
            this._storage = null;
            this._version = 1;
            this._updateCounter = 0;
            this._modified = false;
            this._syncEvents = new List<ISharedObjectEvent>();
            this._listeners = new CopyOnWriteArray();
            this._source = null;
            this._ownerMessage = new SharedObjectMessage(null, null, -1, false);
            this._persistentSO = false;
            this._creationTime = Environment.TickCount;
        }

        public SharedObject(IDictionary<string, object> data, string name, string path, bool persistent)
        {
            this._name = string.Empty;
            this._path = string.Empty;
            this._lastModified = -1L;
            this._persistent = false;
            this._persistentSO = false;
            this._hashes = new Hashtable();
            this._storage = null;
            this._version = 1;
            this._updateCounter = 0;
            this._modified = false;
            this._syncEvents = new List<ISharedObjectEvent>();
            this._listeners = new CopyOnWriteArray();
            this._source = null;
            base.SetAttributes(data);
            this._name = name;
            this._path = path;
            this._persistentSO = persistent;
            this._ownerMessage = new SharedObjectMessage(null, name, 0, persistent);
            this._creationTime = Environment.TickCount;
        }

        public SharedObject(IDictionary<string, object> data, string name, string path, bool persistent, IPersistenceStore storage) : this(data, name, path, persistent)
        {
            this.Store = storage;
        }

        public void BeginUpdate()
        {
            this.BeginUpdate(this._source);
        }

        public void BeginUpdate(IEventListener listener)
        {
            this._source = listener;
            this._updateCounter++;
        }

        protected void CheckRelease()
        {
            if (!this.IsPersistentObject && (this._listeners.Count == 0))
            {
                log.Info(__Res.GetString("SharedObject_Delete", new object[] { this._name }));
                if ((this._storage != null) && !this._storage.Remove(this))
                {
                    log.Error(__Res.GetString("SharedObject_DeleteError"));
                }
                this.Close();
            }
        }

        public bool Clear()
        {
            base.RemoveAttributes();
            this._ownerMessage.AddEvent(SharedObjectEventType.CLIENT_CLEAR_DATA, this._name, null);
            this.NotifyModified();
            return true;
        }

        public virtual void Close()
        {
            base.RemoveAttributes();
            this._listeners.Clear();
            this._syncEvents.Clear();
            this._ownerMessage.Events.Clear();
        }

        public void Deserialize(AMFReader reader)
        {
            this._name = reader.ReadData() as string;
            this._path = reader.ReadData() as string;
            base._attributes.Clear();
            base._attributes = new Dictionary<string, object>(reader.ReadData() as IDictionary<string, object>);
            this._persistent = true;
            this._persistentSO = true;
            this._ownerMessage.SetName(this._name);
            this._ownerMessage.SetIsPersistent(true);
        }

        public void EndUpdate()
        {
            this._updateCounter--;
            if (this._updateCounter == 0)
            {
                this.NotifyModified();
                this._source = null;
            }
        }

        public override object GetAttribute(string name, object value)
        {
            if (name == null)
            {
                return null;
            }
            object attribute = base.GetAttribute(name, value);
            if (attribute == null)
            {
                this._modified = true;
                this._ownerMessage.AddEvent(SharedObjectEventType.CLIENT_UPDATE_DATA, name, value);
                this._syncEvents.Add(new SharedObjectEvent(SharedObjectEventType.CLIENT_UPDATE_DATA, name, value));
                this.NotifyModified();
                attribute = value;
            }
            return attribute;
        }

        protected void NotifyModified()
        {
            if (this._updateCounter <= 0)
            {
                if (this._modified)
                {
                    this._modified = false;
                    this.UpdateVersion();
                    this._lastModified = Environment.TickCount;
                }
                if ((this._modified && (this._storage != null)) && !this._storage.Save(this))
                {
                    log.Error(__Res.GetString("SharedObject_StoreError"));
                }
                this.SendUpdates();
            }
        }

        public virtual void Register(IEventListener listener)
        {
            this._listeners.Add(listener);
            this._ownerMessage.AddEvent(SharedObjectEventType.CLIENT_INITIAL_DATA, null, null);
            if (!this.IsPersistentObject)
            {
                this._ownerMessage.AddEvent(SharedObjectEventType.CLIENT_CLEAR_DATA, null, null);
            }
            if (!base.IsEmpty)
            {
                this._ownerMessage.AddEvent(new SharedObjectEvent(SharedObjectEventType.CLIENT_UPDATE_DATA, null, base._attributes));
            }
            this.NotifyModified();
        }

        public override bool RemoveAttribute(string name)
        {
            this._ownerMessage.AddEvent(SharedObjectEventType.CLIENT_DELETE_DATA, name, null);
            if (base.RemoveAttribute(name))
            {
                this._modified = true;
                this._syncEvents.Add(new SharedObjectEvent(SharedObjectEventType.CLIENT_DELETE_DATA, name, null));
                this.NotifyModified();
                return true;
            }
            this.NotifyModified();
            return false;
        }

        public override void RemoveAttributes()
        {
            ICollection attributeNames = this.GetAttributeNames();
            foreach (string str in attributeNames)
            {
                this._ownerMessage.AddEvent(SharedObjectEventType.CLIENT_DELETE_DATA, str, null);
                this._syncEvents.Add(new SharedObjectEvent(SharedObjectEventType.CLIENT_DELETE_DATA, str, null));
            }
            base.RemoveAttributes();
            this._modified = true;
            this.NotifyModified();
        }

        internal void ReturnAttributeValue(string name)
        {
            this._ownerMessage.AddEvent(SharedObjectEventType.CLIENT_UPDATE_DATA, name, this.GetAttribute(name));
        }

        internal void ReturnError(string message)
        {
            this._ownerMessage.AddEvent(SharedObjectEventType.CLIENT_STATUS, message, "error");
        }

        public void SendMessage(string handler, IList arguments)
        {
            this._ownerMessage.AddEvent(SharedObjectEventType.CLIENT_SEND_MESSAGE, handler, arguments);
            this._syncEvents.Add(new SharedObjectEvent(SharedObjectEventType.CLIENT_SEND_MESSAGE, handler, arguments));
            this._modified = true;
            this.NotifyModified();
        }

        private void SendUpdates()
        {
            RtmpConnection connection;
            RtmpChannel channel;
            if (this._ownerMessage.Events.Count != 0)
            {
                if (this._source != null)
                {
                    SharedObjectMessage message;
                    connection = this._source as RtmpConnection;
                    channel = connection.GetChannel(3);
                    if (connection.ObjectEncoding == ObjectEncoding.AMF0)
                    {
                        message = new SharedObjectMessage(null, this._name, this._version, this.IsPersistentObject);
                    }
                    else
                    {
                        message = new FlexSharedObjectMessage(null, this._name, this._version, this.IsPersistentObject);
                    }
                    message.AddEvents(this._ownerMessage.Events);
                    if (channel != null)
                    {
                        channel.Write(message);
                    }
                    else
                    {
                        log.Warn(__Res.GetString("Channel_NotFound"));
                    }
                }
                this._ownerMessage.Events.Clear();
            }
            if (this._syncEvents.Count != 0)
            {
                foreach (IEventListener listener in this._listeners)
                {
                    if (listener != this._source)
                    {
                        if (!(listener is RtmpConnection))
                        {
                            log.Warn(__Res.GetString("SharedObject_SyncConnError"));
                        }
                        else
                        {
                            SharedObjectMessage message2;
                            connection = listener as RtmpConnection;
                            if (connection.ObjectEncoding == ObjectEncoding.AMF0)
                            {
                                message2 = new SharedObjectMessage(null, this._name, this._version, this.IsPersistentObject);
                            }
                            else
                            {
                                message2 = new FlexSharedObjectMessage(null, this._name, this._version, this.IsPersistentObject);
                            }
                            message2.AddEvents(this._syncEvents);
                            channel = connection.GetChannel(3);
                            log.Debug(__Res.GetString("SharedObject_Sync", new object[] { channel }));
                            channel.Write(message2);
                        }
                    }
                }
                this._syncEvents.Clear();
            }
        }

        public void Serialize(AMFWriter writer)
        {
            writer.WriteString(this.Name);
            writer.WriteString(this.Path);
            writer.WriteData(ObjectEncoding.AMF0, base._attributes);
        }

        public override bool SetAttribute(string name, object value)
        {
            this._ownerMessage.AddEvent(SharedObjectEventType.CLIENT_UPDATE_ATTRIBUTE, name, null);
            if ((value == null) && base.RemoveAttribute(name))
            {
                this._modified = true;
                this._syncEvents.Add(new SharedObjectEvent(SharedObjectEventType.CLIENT_DELETE_DATA, name, null));
                this.NotifyModified();
                return true;
            }
            if ((value != null) && base.SetAttribute(name, value))
            {
                this._modified = true;
                this._syncEvents.Add(new SharedObjectEvent(SharedObjectEventType.CLIENT_UPDATE_DATA, name, value));
                this.NotifyModified();
                return true;
            }
            this.NotifyModified();
            return false;
        }

        public override void SetAttributes(IAttributeStore values)
        {
            if (values != null)
            {
                this.BeginUpdate();
                try
                {
                    foreach (string str in values.GetAttributeNames())
                    {
                        this.SetAttribute(str, values.GetAttribute(str));
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public override void SetAttributes(IDictionary<string, object> values)
        {
            if (values != null)
            {
                this.BeginUpdate();
                try
                {
                    foreach (KeyValuePair<string, object> pair in values)
                    {
                        this.SetAttribute(pair.Key, pair.Value);
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public virtual void Unregister(IEventListener listener)
        {
            this._listeners.Remove(listener);
            this.CheckRelease();
        }

        private void UpdateVersion()
        {
            this._version++;
        }

        public bool IsPersistent
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

        public bool IsPersistentObject
        {
            get
            {
                return this._persistentSO;
            }
            set
            {
                this._persistentSO = value;
            }
        }

        public long LastModified
        {
            get
            {
                return this._lastModified;
            }
            set
            {
                this._lastModified = value;
            }
        }

        [Transient]
        public ICollection Listeners
        {
            get
            {
                return ((this._listeners == null) ? null : new ReadOnlyList(this._listeners));
            }
        }

        public string Name
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

        public string Path
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

        [Transient]
        public IPersistenceStore Store
        {
            get
            {
                return this._storage;
            }
            set
            {
                this._storage = value;
            }
        }

        [Transient]
        public string Type
        {
            get
            {
                return "SharedObject";
            }
        }

        [Transient]
        internal int UpdateCounter
        {
            get
            {
                return this._updateCounter;
            }
        }

        public int Version
        {
            get
            {
                return this._version;
            }
            set
            {
                this._version = value;
            }
        }
    }
}

