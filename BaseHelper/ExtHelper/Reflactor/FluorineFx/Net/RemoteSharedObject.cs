namespace FluorineFx.Net
{
    using FluorineFx;
    using FluorineFx.Invocation;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Rtmp;
    using FluorineFx.Messaging.Rtmp.SO;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;

    [CLSCompliant(false)]
    public class RemoteSharedObject : AttributeStore
    {
        private NetConnection _connection;
        private bool _initialSyncReceived;
        private long _lastModified;
        private bool _modified;
        private string _name;
        private FluorineFx.ObjectEncoding _objectEncoding;
        private SharedObjectMessage _ownerMessage;
        private string _path;
        private bool _persistentSO;
        private bool _secure;
        private IEventListener _source;
        private int _updateCounter;
        private int _version;
        private static readonly ILog log = LogManager.GetLogger(typeof(RemoteSharedObject));
        private static Dictionary<string, RemoteSharedObject> SharedObjects = new Dictionary<string, RemoteSharedObject>();

        private event ConnectHandler _connectHandler;

        private event DisconnectHandler _disconnectHandler;

        private event NetStatusHandler _netStatusHandler;

        private event SyncHandler _syncHandler;

        public event NetStatusHandler NetStatus
        {
            add
            {
                this._netStatusHandler += value;
            }
            remove
            {
                this._netStatusHandler -= value;
            }
        }

        public event ConnectHandler OnConnect
        {
            add
            {
                this._connectHandler += value;
            }
            remove
            {
                this._connectHandler -= value;
            }
        }

        public event DisconnectHandler OnDisconnect
        {
            add
            {
                this._disconnectHandler += value;
            }
            remove
            {
                this._disconnectHandler -= value;
            }
        }

        public event SyncHandler Sync
        {
            add
            {
                this._syncHandler += value;
            }
            remove
            {
                this._syncHandler -= value;
            }
        }

        public RemoteSharedObject()
        {
            this._name = string.Empty;
            this._path = string.Empty;
            this._persistentSO = false;
            this._version = 1;
            this._updateCounter = 0;
            this._modified = false;
            this._lastModified = -1L;
            this._source = null;
        }

        private RemoteSharedObject(string name, string remotePath, object persistence, bool secure)
        {
            this._name = string.Empty;
            this._path = string.Empty;
            this._persistentSO = false;
            this._version = 1;
            this._updateCounter = 0;
            this._modified = false;
            this._lastModified = -1L;
            this._source = null;
            this._name = name;
            this._path = remotePath;
            bool flag = false;
            this._persistentSO = !flag.Equals(persistence);
            this._secure = secure;
            this._objectEncoding = FluorineFx.ObjectEncoding.AMF0;
            this._initialSyncReceived = false;
            this._ownerMessage = new SharedObjectMessage(null, null, -1, false);
        }

        public void BeginUpdate()
        {
            this._updateCounter++;
        }

        public void Close()
        {
            if ((this._initialSyncReceived && (this._connection != null)) && this._connection.Connected)
            {
                SharedObjectMessage message;
                if (this._connection.ObjectEncoding == FluorineFx.ObjectEncoding.AMF0)
                {
                    message = new SharedObjectMessage(this._name, this._version, this._persistentSO);
                }
                else
                {
                    message = new FlexSharedObjectMessage(this._name, this._version, this._persistentSO);
                }
                SharedObjectEvent sharedObjectEvent = new SharedObjectEvent(SharedObjectEventType.SERVER_DISCONNECT, null, null);
                message.AddEvent(sharedObjectEvent);
                this._connection.NetConnectionClient.Write(message);
                base.RemoveAttributes();
                this._ownerMessage.Events.Clear();
            }
            this._initialSyncReceived = false;
        }

        public void Connect(NetConnection connection)
        {
            this.Connect(connection, null);
        }

        public void Connect(NetConnection connection, string parameters)
        {
            SharedObjectMessage message;
            if (this._initialSyncReceived)
            {
                throw new InvalidOperationException("SharedObject already connected");
            }
            ValidationUtils.ArgumentNotNull(connection, "connection");
            ValidationUtils.ArgumentNotNull(connection.Uri, "connection");
            ValidationUtils.ArgumentConditionTrue(connection.Uri.Scheme == "rtmp", "connection", "NetConnection object must use the Real-Time Messaging Protocol (RTMP)");
            ValidationUtils.ArgumentConditionTrue(connection.Connected, "connection", "NetConnection object must be connected");
            this._connection = connection;
            this._initialSyncReceived = false;
            if (connection.ObjectEncoding == FluorineFx.ObjectEncoding.AMF0)
            {
                message = new SharedObjectMessage(this._name, this._version, this._persistentSO);
            }
            else
            {
                message = new FlexSharedObjectMessage(this._name, this._version, this._persistentSO);
            }
            SharedObjectEvent sharedObjectEvent = new SharedObjectEvent(SharedObjectEventType.SERVER_CONNECT, null, null);
            message.AddEvent(sharedObjectEvent);
            this._connection.NetConnectionClient.Write(message);
        }

        internal static void Dispatch(SharedObjectMessage message)
        {
            RemoteSharedObject obj2 = null;
            lock (((ICollection) SharedObjects).SyncRoot)
            {
                if (SharedObjects.ContainsKey(message.Name))
                {
                    obj2 = SharedObjects[message.Name];
                }
            }
            if (obj2 != null)
            {
                try
                {
                    obj2.DispatchSharedObjectMessage(message);
                }
                catch (Exception exception)
                {
                    obj2.RaiseNetStatus(exception);
                }
            }
        }

        internal static void DispatchDisconnect(NetConnection connection)
        {
            lock (((ICollection) SharedObjects).SyncRoot)
            {
                foreach (RemoteSharedObject obj2 in SharedObjects.Values)
                {
                    if (obj2._connection == connection)
                    {
                        obj2.RaiseDisconnect();
                    }
                }
            }
        }

        internal void DispatchSharedObjectMessage(SharedObjectMessage message)
        {
            List<ASObject> list = null;
            List<ASObject> list2 = null;
            foreach (ISharedObjectEvent event2 in message.Events)
            {
                ASObject obj2;
                switch (event2.Type)
                {
                    case SharedObjectEventType.SERVER_SEND_MESSAGE:
                    case SharedObjectEventType.CLIENT_SEND_MESSAGE:
                    {
                        string key = event2.Key;
                        IList arguments = event2.Value as IList;
                        MethodInfo methodInfo = MethodHandler.GetMethod(base.GetType(), key, arguments);
                        if (methodInfo != null)
                        {
                            ParameterInfo[] parameters = methodInfo.GetParameters();
                            object[] array = new object[parameters.Length];
                            arguments.CopyTo(array, 0);
                            TypeHelper.NarrowValues(array, parameters);
                            try
                            {
                                object obj3 = new InvocationHandler(methodInfo).Invoke(this, array);
                            }
                            catch (Exception exception)
                            {
                                log.Error("Error while invoking method " + key + " on shared object", exception);
                            }
                        }
                        break;
                    }
                    case SharedObjectEventType.CLIENT_CLEAR_DATA:
                        obj2 = new ASObject();
                        obj2["code"] = "clear";
                        if (list == null)
                        {
                            list = new List<ASObject>();
                        }
                        list.Add(obj2);
                        base._attributes.Clear();
                        break;

                    case SharedObjectEventType.CLIENT_DELETE_ATTRIBUTE:
                    case SharedObjectEventType.CLIENT_DELETE_DATA:
                        base._attributes.Remove(event2.Key);
                        obj2 = new ASObject();
                        obj2["code"] = "delete";
                        obj2["name"] = event2.Key;
                        if (list == null)
                        {
                            list = new List<ASObject>();
                        }
                        list.Add(obj2);
                        break;

                    case SharedObjectEventType.CLIENT_INITIAL_DATA:
                        if (message.Version > 0)
                        {
                            this._version = message.Version;
                        }
                        base._attributes.Clear();
                        this._initialSyncReceived = true;
                        this.RaiseOnConnect();
                        break;

                    case SharedObjectEventType.CLIENT_STATUS:
                        obj2 = new ASObject();
                        obj2["level"] = event2.Value;
                        obj2["code"] = event2.Key;
                        if (list2 == null)
                        {
                            list2 = new List<ASObject>();
                        }
                        list2.Add(obj2);
                        break;

                    case SharedObjectEventType.CLIENT_UPDATE_DATA:
                    case SharedObjectEventType.CLIENT_UPDATE_ATTRIBUTE:
                        obj2 = new ASObject();
                        obj2["code"] = "change";
                        obj2["name"] = event2.Key;
                        obj2["oldValue"] = this.GetAttribute(event2.Key);
                        base._attributes[event2.Key] = event2.Value;
                        if (list == null)
                        {
                            list = new List<ASObject>();
                        }
                        list.Add(obj2);
                        break;
                }
            }
            if ((list != null) && (list.Count > 0))
            {
                this.RaiseSync(list.ToArray());
            }
            if (list2 != null)
            {
                foreach (ASObject obj2 in list2)
                {
                    this.RaiseNetStatus(obj2);
                }
            }
        }

        public void EndUpdate()
        {
            this._updateCounter--;
            if (this._updateCounter == 0)
            {
                this.NotifyModified();
            }
        }

        public sealed override object GetAttribute(string name, object value)
        {
            if (name == null)
            {
                return null;
            }
            if (!this.HasAttribute(name))
            {
                this.SetAttribute(name, value);
            }
            return this.GetAttribute(name);
        }

        public static RemoteSharedObject GetRemote(string name, string remotePath, object persistence)
        {
            return GetRemote(typeof(RemoteSharedObject), name, remotePath, persistence, false);
        }

        public static RemoteSharedObject GetRemote(string name, string remotePath, object persistence, bool secure)
        {
            return GetRemote(typeof(RemoteSharedObject), name, remotePath, persistence, secure);
        }

        public static RemoteSharedObject GetRemote(Type type, string name, string remotePath, object persistence)
        {
            return GetRemote(type, name, remotePath, persistence, false);
        }

        private static RemoteSharedObject GetRemote(Type type, string name, string remotePath, object persistence, bool secure)
        {
            lock (((ICollection) SharedObjects).SyncRoot)
            {
                if (SharedObjects.ContainsKey(name))
                {
                    return SharedObjects[name];
                }
                RemoteSharedObject obj2 = Activator.CreateInstance(type) as RemoteSharedObject;
                ValidationUtils.ArgumentConditionTrue(obj2 != null, "type", "Expecting a RemoteSharedObject type");
                obj2._name = name;
                obj2._path = remotePath;
                bool flag2 = false;
                obj2._persistentSO = !flag2.Equals(persistence);
                obj2._secure = secure;
                obj2._objectEncoding = FluorineFx.ObjectEncoding.AMF0;
                obj2._initialSyncReceived = false;
                obj2._ownerMessage = new SharedObjectMessage(null, null, -1, false);
                SharedObjects[name] = obj2;
                return obj2;
            }
        }

        private void NotifyModified()
        {
            if (this._updateCounter <= 0)
            {
                if (this._modified)
                {
                    this._modified = false;
                    this.UpdateVersion();
                    this._lastModified = Environment.TickCount;
                }
                this.SendUpdates();
            }
        }

        internal void RaiseDisconnect()
        {
            this._initialSyncReceived = false;
            if (this._disconnectHandler != null)
            {
                this._disconnectHandler(this, new EventArgs());
            }
        }

        internal void RaiseNetStatus(ASObject info)
        {
            if (this._netStatusHandler != null)
            {
                this._netStatusHandler(this, new NetStatusEventArgs(info));
            }
        }

        internal void RaiseNetStatus(Exception exception)
        {
            if (this._netStatusHandler != null)
            {
                this._netStatusHandler(this, new NetStatusEventArgs(exception));
            }
        }

        internal void RaiseOnConnect()
        {
            if (this._connectHandler != null)
            {
                this._connectHandler(this, new EventArgs());
            }
        }

        internal void RaiseSync(ASObject[] changeList)
        {
            if (this._syncHandler != null)
            {
                this._syncHandler(this, new SyncEventArgs(changeList));
            }
        }

        public sealed override bool RemoveAttribute(string name)
        {
            if (base.RemoveAttribute(name))
            {
                this._modified = true;
                this._ownerMessage.AddEvent(SharedObjectEventType.SERVER_DELETE_ATTRIBUTE, name, null);
                this.NotifyModified();
                return true;
            }
            return false;
        }

        public sealed override void RemoveAttributes()
        {
            foreach (string str in this.GetAttributeNames())
            {
                this._ownerMessage.AddEvent(SharedObjectEventType.SERVER_DELETE_ATTRIBUTE, str, null);
            }
            base.RemoveAttributes();
            this._modified = true;
            this.NotifyModified();
        }

        public void Send(string handler, params object[] arguments)
        {
            this._ownerMessage.AddEvent(SharedObjectEventType.SERVER_SEND_MESSAGE, handler, arguments);
            this._modified = true;
            this.NotifyModified();
        }

        private void SendUpdates()
        {
            if (this._ownerMessage.Events.Count != 0)
            {
                if (this._connection.NetConnectionClient.Connection != null)
                {
                    SharedObjectMessage message;
                    RtmpConnection connection = this._connection.NetConnectionClient.Connection as RtmpConnection;
                    RtmpChannel channel = connection.GetChannel(3);
                    if (connection.ObjectEncoding == FluorineFx.ObjectEncoding.AMF0)
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
        }

        public sealed override bool SetAttribute(string name, object value)
        {
            this._ownerMessage.AddEvent(SharedObjectEventType.SERVER_SET_ATTRIBUTE, name, value);
            if (value == null)
            {
                this.RemoveAttribute(name);
                return true;
            }
            if (base.SetAttribute(name, value))
            {
                this._modified = true;
                this.NotifyModified();
                return true;
            }
            this.NotifyModified();
            return false;
        }

        public sealed override void SetAttributes(IAttributeStore values)
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

        public sealed override void SetAttributes(IDictionary<string, object> values)
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

        public void SetDirty(string propertyName)
        {
            this._source = this._connection.NetConnectionClient.Connection;
            this._ownerMessage.AddEvent(SharedObjectEventType.SERVER_SET_ATTRIBUTE, propertyName, this.GetAttribute(propertyName));
            this._modified = true;
            this.NotifyModified();
        }

        public void SetProperty(string propertyName, object value)
        {
            this.SetAttribute(propertyName, value);
        }

        private void UpdateVersion()
        {
            this._version++;
        }

        public bool Connected
        {
            get
            {
                return this._initialSyncReceived;
            }
        }

        public bool IsPersistentObject
        {
            get
            {
                return this._persistentSO;
            }
        }

        public FluorineFx.ObjectEncoding ObjectEncoding
        {
            get
            {
                return this._objectEncoding;
            }
            set
            {
                this._objectEncoding = value;
            }
        }
    }
}

