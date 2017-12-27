namespace FluorineFx.Messaging
{
    using FluorineFx;
    using FluorineFx.Collections.Generic;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using log4net;
    using System;
    using System.Collections;
    using System.Net;

    [CLSCompliant(false)]
    public abstract class BaseConnection : AttributeStore, IConnection, ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener
    {
        protected byte __fields;
        protected CopyOnWriteArraySet<IBasicScope> _basicScopes;
        protected IClient _client;
        protected string _connectionId;
        protected long _droppedMessages;
        protected FluorineFx.ObjectEncoding _objectEncoding;
        protected IDictionary _parameters;
        protected string _path;
        protected long _readMessages;
        private IScope _scope;
        private object _syncLock;
        protected long _writtenMessages;
        private static ILog log = LogManager.GetLogger(typeof(BaseConnection));

        public BaseConnection(string path, IDictionary parameters) : this(path, Guid.NewGuid().ToString("N").Remove(12, 1), parameters)
        {
        }

        internal BaseConnection(string path, string connectionId, IDictionary parameters)
        {
            this._syncLock = new object();
            this._basicScopes = new CopyOnWriteArraySet<IBasicScope>();
            this._connectionId = connectionId;
            this._objectEncoding = FluorineFx.ObjectEncoding.AMF0;
            this._path = path;
            this._parameters = parameters;
            this.SetIsClosed(false);
        }

        public virtual void Close()
        {
            lock (this.SyncRoot)
            {
                if (!this.IsClosed)
                {
                    Exception exception;
                    this.SetIsClosed(true);
                    log.Debug("Close, disconnect from scope, and children");
                    if (this._basicScopes != null)
                    {
                        try
                        {
                            foreach (IBasicScope scope in this._basicScopes)
                            {
                                this.UnregisterBasicScope(scope);
                            }
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            log.Error(__Res.GetString("Scope_UnregisterError"), exception);
                        }
                    }
                    if (this._scope != null)
                    {
                        try
                        {
                            this._scope.Disconnect(this);
                        }
                        catch (Exception exception2)
                        {
                            exception = exception2;
                            log.Error(__Res.GetString("Scope_DisconnectError", new object[] { this._scope }), exception);
                        }
                    }
                    if (this._client != null)
                    {
                        this._client.Unregister(this);
                        this._client = null;
                    }
                    this._scope = null;
                }
            }
        }

        public bool Connect(IScope scope)
        {
            return this.Connect(scope, null);
        }

        public virtual bool Connect(IScope scope, object[] parameters)
        {
            IScope scope2 = this._scope;
            this._scope = scope;
            if (this._scope.Connect(this, parameters))
            {
                if (scope2 != null)
                {
                    scope2.Disconnect(this);
                }
                return true;
            }
            this._scope = scope2;
            return false;
        }

        public virtual void DispatchEvent(IEvent evt)
        {
        }

        public virtual long GetPendingVideoMessages(int streamId)
        {
            return 0L;
        }

        public virtual bool HandleEvent(IEvent evt)
        {
            return this.Scope.HandleEvent(evt);
        }

        public void Initialize(IClient client)
        {
            if (this.Client != null)
            {
                this.Client.Unregister(this);
            }
            this._client = client;
            this._client.Register(this);
        }

        public virtual void NotifyEvent(IEvent evt)
        {
        }

        public virtual void Ping()
        {
        }

        public void RegisterBasicScope(IBasicScope basicScope)
        {
            this._basicScopes.Add(basicScope);
            basicScope.AddEventListener(this);
        }

        internal void SetIsClosed(bool value)
        {
            this.__fields = value ? ((byte) (this.__fields | 1)) : ((byte) (this.__fields & -2));
        }

        internal void SetIsClosing(bool value)
        {
            this.__fields = value ? ((byte) (this.__fields | 2)) : ((byte) (this.__fields & -3));
        }

        internal void SetIsFlexClient(bool value)
        {
            this.__fields = value ? ((byte) (this.__fields | 8)) : ((byte) (this.__fields & -9));
        }

        public virtual void Timeout()
        {
        }

        public void UnregisterBasicScope(IBasicScope basicScope)
        {
            this._basicScopes.Remove(basicScope);
            basicScope.RemoveEventListener(this);
        }

        public IEnumerator BasicScopes
        {
            get
            {
                return this._basicScopes.GetEnumerator();
            }
        }

        public IClient Client
        {
            get
            {
                return this._client;
            }
        }

        public virtual long ClientBytesRead
        {
            get
            {
                return 0L;
            }
        }

        public virtual int ClientLeaseTime
        {
            get
            {
                return 0;
            }
        }

        public string ConnectionId
        {
            get
            {
                return this._connectionId;
            }
        }

        public long DroppedMessages
        {
            get
            {
                return this._droppedMessages;
            }
        }

        public bool IsClosed
        {
            get
            {
                return ((this.__fields & 1) == 1);
            }
        }

        public bool IsClosing
        {
            get
            {
                return ((this.__fields & 2) == 2);
            }
        }

        public virtual bool IsConnected
        {
            get
            {
                return (this._scope != null);
            }
        }

        public bool IsFlexClient
        {
            get
            {
                return ((this.__fields & 8) == 8);
            }
        }

        public abstract int LastPingTime { get; }

        public FluorineFx.ObjectEncoding ObjectEncoding
        {
            get
            {
                return this._objectEncoding;
            }
        }

        public IDictionary Parameters
        {
            get
            {
                return this._parameters;
            }
        }

        public string Path
        {
            get
            {
                return this._path;
            }
        }

        public long PendingMessages
        {
            get
            {
                return 0L;
            }
        }

        public abstract long ReadBytes { get; }

        public long ReadMessages
        {
            get
            {
                return this._readMessages;
            }
        }

        public abstract IPEndPoint RemoteEndPoint { get; }

        public IScope Scope
        {
            get
            {
                return this._scope;
            }
        }

        public string SessionId
        {
            get
            {
                return this._connectionId;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this._syncLock;
            }
        }

        public abstract long WrittenBytes { get; }

        public long WrittenMessages
        {
            get
            {
                return this._writtenMessages;
            }
        }
    }
}

