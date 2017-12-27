namespace FluorineFx.Messaging.Rtmp.SO
{
    using FluorineFx;
    using FluorineFx.Collections;
    using FluorineFx.Configuration;
    using FluorineFx.Invocation;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Persistence;
    using FluorineFx.Messaging.Api.SO;
    using FluorineFx.Messaging.Rtmp;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;

    internal class SharedObjectScope : BasicScope, ISharedObject, IBasicScope, ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener, IEventObservable, IPersistable, IEnumerable, ISharedObjectSecurityService, IService
    {
        private Hashtable _handlers;
        private CopyOnWriteArray _securityHandlers;
        private CopyOnWriteArray _serverListeners;
        protected SharedObject _so;
        private static readonly ILog log = LogManager.GetLogger(typeof(SharedObjectScope));

        public SharedObjectScope(IScope parent, string name, bool persistent, IPersistenceStore store) : base(parent, SharedObjectService.ScopeType, name, persistent)
        {
            this._serverListeners = new CopyOnWriteArray();
            this._handlers = new Hashtable();
            this._securityHandlers = new CopyOnWriteArray();
            string contextPath = parent.ContextPath;
            if (!contextPath.StartsWith("/"))
            {
                contextPath = "/" + contextPath;
            }
            this._so = store.Load(name) as SharedObject;
            if (this._so == null)
            {
                this._so = new SharedObject(base._attributes, name, contextPath, persistent, store);
                store.Save(this._so);
            }
            else
            {
                this._so.Name = name;
                this._so.Path = parent.ContextPath;
                this._so.Store = store;
            }
        }

        public override void AddEventListener(IEventListener listener)
        {
            base.AddEventListener(listener);
            this._so.Register(listener);
            foreach (ISharedObjectListener listener2 in this._serverListeners)
            {
                listener2.OnSharedObjectConnect(this);
            }
        }

        public void AddSharedObjectListener(ISharedObjectListener listener)
        {
            this._serverListeners.Add(listener);
        }

        public void BeginUpdate()
        {
            Monitor.Enter(base.SyncRoot);
            this._so.BeginUpdate();
        }

        public void BeginUpdate(IEventListener source)
        {
            Monitor.Enter(base.SyncRoot);
            this._so.BeginUpdate(source);
        }

        public bool Clear()
        {
            bool flag;
            this.BeginUpdate();
            try
            {
                flag = this._so.Clear();
            }
            finally
            {
                this.EndUpdate();
            }
            if (flag)
            {
                foreach (ISharedObjectListener listener in this._serverListeners)
                {
                    listener.OnSharedObjectClear(this);
                }
            }
            return flag;
        }

        public void Close()
        {
            lock (base.SyncRoot)
            {
                this._so.Close();
                this._so = null;
            }
        }

        public override void DispatchEvent(IEvent evt)
        {
            if (!((evt.EventType == EventType.SHARED_OBJECT) && (evt is ISharedObjectMessage)))
            {
                base.DispatchEvent(evt);
            }
            else
            {
                ISharedObjectMessage message = (ISharedObjectMessage) evt;
                if (message.HasSource)
                {
                    this.BeginUpdate(message.Source);
                }
                else
                {
                    this.BeginUpdate();
                }
                try
                {
                    foreach (ISharedObjectEvent event2 in message.Events)
                    {
                        IEventListener source;
                        switch (event2.Type)
                        {
                            case SharedObjectEventType.SERVER_CONNECT:
                            {
                                if (this.IsConnectionAllowed())
                                {
                                    break;
                                }
                                this._so.ReturnError("SharedObject.NoReadAccess");
                                continue;
                            }
                            case SharedObjectEventType.SERVER_DISCONNECT:
                            {
                                if (message.HasSource)
                                {
                                    source = message.Source;
                                    if (!(source is RtmpConnection))
                                    {
                                        goto Label_012E;
                                    }
                                    (source as RtmpConnection).UnregisterBasicScope(this);
                                }
                                continue;
                            }
                            case SharedObjectEventType.SERVER_SET_ATTRIBUTE:
                            {
                                if (this.IsWriteAllowed(event2.Key, event2.Value))
                                {
                                    goto Label_0179;
                                }
                                this._so.ReturnAttributeValue(event2.Key);
                                this._so.ReturnError("SharedObject.NoWriteAccess");
                                continue;
                            }
                            case SharedObjectEventType.SERVER_DELETE_ATTRIBUTE:
                            {
                                if (this.IsDeleteAllowed(event2.Key))
                                {
                                    goto Label_01C8;
                                }
                                this._so.ReturnAttributeValue(event2.Key);
                                this._so.ReturnError("SharedObject.NoWriteAccess");
                                continue;
                            }
                            case SharedObjectEventType.SERVER_SEND_MESSAGE:
                            {
                                if (this.IsSendAllowed(event2.Key, event2.Value as IList))
                                {
                                    this.SendMessage(event2.Key, event2.Value as IList);
                                }
                                continue;
                            }
                            default:
                                goto Label_020F;
                        }
                        if (message.HasSource)
                        {
                            source = message.Source;
                            if (source is RtmpConnection)
                            {
                                (source as RtmpConnection).RegisterBasicScope(this);
                            }
                            else
                            {
                                this.AddEventListener(source);
                            }
                        }
                        continue;
                    Label_012E:
                        this.RemoveEventListener(source);
                        continue;
                    Label_0179:
                        this.SetAttribute(event2.Key, event2.Value);
                        continue;
                    Label_01C8:
                        this.RemoveAttribute(event2.Key);
                        continue;
                    Label_020F:
                        log.Warn("Unknown SO event: " + event2.Type.ToString());
                    }
                }
                finally
                {
                    this.EndUpdate();
                }
            }
        }

        public void EndUpdate()
        {
            this._so.EndUpdate();
            Monitor.Exit(base.SyncRoot);
        }

        public override object GetAttribute(string name)
        {
            lock (base.SyncRoot)
            {
                return this._so.GetAttribute(name);
            }
        }

        public override ICollection GetAttributeNames()
        {
            lock (base.SyncRoot)
            {
                return this._so.GetAttributeNames();
            }
        }

        private IEnumerator GetSecurityHandlers()
        {
            ISharedObjectSecurityService scopeService = ScopeUtils.GetScopeService(this.Parent, typeof(ISharedObjectSecurityService)) as ISharedObjectSecurityService;
            if (scopeService == null)
            {
                return null;
            }
            return scopeService.GetSharedObjectSecurity();
        }

        public object GetServiceHandler(string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }
            return this._handlers[name];
        }

        public ICollection GetServiceHandlerNames()
        {
            return new ReadOnlyCollection(this._handlers.Keys);
        }

        public IEnumerator GetSharedObjectSecurity()
        {
            return this._securityHandlers.GetEnumerator();
        }

        public override bool HasAttribute(string name)
        {
            lock (base.SyncRoot)
            {
                return this._so.HasAttribute(name);
            }
        }

        protected bool IsConnectionAllowed()
        {
            ISharedObjectSecurity current;
            using (IEnumerator enumerator2 = this._securityHandlers.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    current = (ISharedObjectSecurity) enumerator2.Current;
                    if (!current.IsConnectionAllowed(this))
                    {
                        return false;
                    }
                }
            }
            IEnumerator securityHandlers = this.GetSecurityHandlers();
            if (securityHandlers != null)
            {
                while (securityHandlers.MoveNext())
                {
                    current = securityHandlers.Current as ISharedObjectSecurity;
                    if (!current.IsConnectionAllowed(this))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected bool IsDeleteAllowed(string key)
        {
            ISharedObjectSecurity current;
            using (IEnumerator enumerator2 = this._securityHandlers.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    current = (ISharedObjectSecurity) enumerator2.Current;
                    if (!current.IsDeleteAllowed(this, key))
                    {
                        return false;
                    }
                }
            }
            IEnumerator securityHandlers = this.GetSecurityHandlers();
            if (securityHandlers != null)
            {
                while (securityHandlers.MoveNext())
                {
                    current = securityHandlers.Current as ISharedObjectSecurity;
                    if (!current.IsDeleteAllowed(this, key))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected bool IsSendAllowed(string message, IList arguments)
        {
            ISharedObjectSecurity current;
            using (IEnumerator enumerator2 = this._securityHandlers.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    current = (ISharedObjectSecurity) enumerator2.Current;
                    if (!current.IsSendAllowed(this, message, arguments))
                    {
                        return false;
                    }
                }
            }
            IEnumerator securityHandlers = this.GetSecurityHandlers();
            if (securityHandlers != null)
            {
                while (securityHandlers.MoveNext())
                {
                    current = securityHandlers.Current as ISharedObjectSecurity;
                    if (!current.IsSendAllowed(this, message, arguments))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected bool IsWriteAllowed(string key, object value)
        {
            ISharedObjectSecurity current;
            using (IEnumerator enumerator2 = this._securityHandlers.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    current = (ISharedObjectSecurity) enumerator2.Current;
                    if (!current.IsWriteAllowed(this, key, value))
                    {
                        return false;
                    }
                }
            }
            IEnumerator securityHandlers = this.GetSecurityHandlers();
            if (securityHandlers != null)
            {
                while (securityHandlers.MoveNext())
                {
                    current = securityHandlers.Current as ISharedObjectSecurity;
                    if (!current.IsWriteAllowed(this, key, value))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void RegisterServiceHandler(object handler)
        {
            this.RegisterServiceHandler("", handler);
        }

        public void RegisterServiceHandler(string name, object handler)
        {
            if (name == null)
            {
                name = string.Empty;
            }
            this._handlers.Add(name, handler);
        }

        public void RegisterSharedObjectSecurity(ISharedObjectSecurity handler)
        {
            this._securityHandlers.Add(handler);
        }

        public override bool RemoveAttribute(string name)
        {
            bool flag = false;
            this.BeginUpdate();
            try
            {
                flag = this._so.RemoveAttribute(name);
            }
            finally
            {
                this.EndUpdate();
            }
            if (flag)
            {
                foreach (ISharedObjectListener listener in this._serverListeners)
                {
                    listener.OnSharedObjectDelete(this, name);
                }
            }
            return flag;
        }

        public override void RemoveAttributes()
        {
            this.BeginUpdate();
            try
            {
                this._so.RemoveAttributes();
            }
            finally
            {
                this.EndUpdate();
            }
            foreach (ISharedObjectListener listener in this._serverListeners)
            {
                listener.OnSharedObjectClear(this);
            }
        }

        public override void RemoveEventListener(IEventListener listener)
        {
            this._so.Unregister(listener);
            base.RemoveEventListener(listener);
            if (!(this._so.IsPersistentObject || ((this._so.Listeners != null) && (this._so.Listeners.Count != 0))))
            {
                this.Parent.RemoveChildScope(this);
            }
            foreach (ISharedObjectListener listener2 in this._serverListeners)
            {
                listener2.OnSharedObjectDisconnect(this);
            }
        }

        public void RemoveSharedObjectListener(ISharedObjectListener listener)
        {
            this._serverListeners.Remove(listener);
        }

        public void SendMessage(string handler, IList arguments)
        {
            string str;
            string str2;
            this.BeginUpdate();
            try
            {
                this._so.SendMessage(handler, arguments);
            }
            finally
            {
                this.EndUpdate();
            }
            int length = handler.LastIndexOf(".");
            if (length != -1)
            {
                str = handler.Substring(0, length);
                str2 = handler.Substring(length + 1);
            }
            else
            {
                str = string.Empty;
                str2 = handler;
            }
            object serviceHandler = this.GetServiceHandler(str);
            if ((serviceHandler == null) && base.HasParent)
            {
                IScopeContext context = this.Parent.Context;
                try
                {
                    serviceHandler = ObjectFactory.CreateInstance(this._so.Name + "." + str);
                }
                catch (Exception)
                {
                    log.Debug(__Res.GetString("Type_InitError", new object[] { this._so.Name + "." + str }));
                }
            }
            if (serviceHandler != null)
            {
                MethodInfo methodInfo = MethodHandler.GetMethod(serviceHandler.GetType(), str2, arguments);
                if (methodInfo != null)
                {
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    object[] array = new object[parameters.Length];
                    arguments.CopyTo(array, 0);
                    TypeHelper.NarrowValues(array, parameters);
                    try
                    {
                        object obj3 = new InvocationHandler(methodInfo).Invoke(serviceHandler, array);
                    }
                    catch (Exception exception)
                    {
                        log.Error("Error while invoking method " + str2 + " on shared object handler " + handler, exception);
                    }
                }
            }
            foreach (ISharedObjectListener listener in this._serverListeners)
            {
                listener.OnSharedObjectSend(this, handler, arguments);
            }
        }

        public override bool SetAttribute(string name, object value)
        {
            bool flag = false;
            this.BeginUpdate();
            try
            {
                flag = this._so.SetAttribute(name, value);
            }
            finally
            {
                this.EndUpdate();
            }
            if (flag)
            {
                foreach (ISharedObjectListener listener in this._serverListeners)
                {
                    listener.OnSharedObjectUpdate(this, name, value);
                }
            }
            return flag;
        }

        public override void SetAttributes(IAttributeStore values)
        {
            this.BeginUpdate();
            try
            {
                this._so.SetAttributes(values);
            }
            finally
            {
                this.EndUpdate();
            }
            foreach (ISharedObjectListener listener in this._serverListeners)
            {
                listener.OnSharedObjectUpdate(this, values);
            }
        }

        public override void SetAttributes(IDictionary<string, object> values)
        {
            this.BeginUpdate();
            try
            {
                this._so.SetAttributes(values);
            }
            finally
            {
                this.EndUpdate();
            }
            foreach (ISharedObjectListener listener in this._serverListeners)
            {
                listener.OnSharedObjectUpdate(this, values);
            }
        }

        public void Shutdown()
        {
        }

        public void Start(ConfigurationSection configuration)
        {
        }

        public void UnregisterServiceHandler()
        {
            this.UnregisterServiceHandler(string.Empty);
        }

        public void UnregisterServiceHandler(string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }
            this._handlers.Remove(name);
        }

        public void UnregisterSharedObjectSecurity(ISharedObjectSecurity handler)
        {
            this._securityHandlers.Remove(handler);
        }

        public bool IsLocked
        {
            get
            {
                bool flag = Monitor.TryEnter(base.SyncRoot);
                if (flag)
                {
                    Monitor.Exit(base.SyncRoot);
                }
                return flag;
            }
        }

        public override bool IsPersistent
        {
            get
            {
                return this._so.IsPersistent;
            }
            set
            {
                this._so.IsPersistent = value;
            }
        }

        public bool IsPersistentObject
        {
            get
            {
                return this._so.IsPersistentObject;
            }
        }

        public override long LastModified
        {
            get
            {
                return this._so.LastModified;
            }
        }

        public override string Name
        {
            get
            {
                return this._so.Name;
            }
            set
            {
                this._so.Name = value;
            }
        }

        public override string Path
        {
            get
            {
                return this._so.Path;
            }
            set
            {
                this._so.Path = value;
            }
        }

        public override IPersistenceStore Store
        {
            get
            {
                return this._so.Store;
            }
            set
            {
                this._so.Store = value;
            }
        }

        public override string Type
        {
            get
            {
                return this._so.Type;
            }
        }

        public int Version
        {
            get
            {
                return this._so.Version;
            }
        }
    }
}

