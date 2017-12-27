namespace FluorineFx.Messaging
{
    using FluorineFx.Collections.Generic;
    using FluorineFx.Context;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Persistence;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class Scope : BasicScope, IScope, IBasicScope, ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener, IEventObservable, IPersistable, IEnumerable, IServiceContainer, IServiceProvider
    {
        private bool _autoStart;
        private Dictionary<string, IBasicScope> _children;
        private Dictionary<IClient, CopyOnWriteArraySet<IConnection>> _clients;
        private IScopeContext _context;
        private bool _enabled;
        private IScopeHandler _handler;
        private bool _running;
        protected ServiceContainer _serviceContainer;
        private static ILog log = LogManager.GetLogger(typeof(Scope));
        private static string ScopeType = "scope";
        public static string Separator = ":";

        protected Scope() : this(string.Empty)
        {
        }

        protected Scope(string name) : this(name, null)
        {
        }

        protected Scope(string name, IServiceProvider serviceProvider) : base(null, ScopeType, name, false)
        {
            this._autoStart = true;
            this._enabled = true;
            this._running = false;
            this._children = new Dictionary<string, IBasicScope>();
            this._clients = new Dictionary<IClient, CopyOnWriteArraySet<IConnection>>();
            this._serviceContainer = new ServiceContainer(serviceProvider);
        }

        public bool AddChildScope(IBasicScope scope)
        {
            if (this.HasHandler && !this.Handler.AddChildScope(scope))
            {
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    log.Debug(string.Concat(new object[] { "Failed to add child scope: ", scope, " to ", this }));
                }
                return false;
            }
            if ((scope is IScope) && (this.HasHandler && !this.Handler.Start((IScope) scope)))
            {
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    log.Debug(string.Concat(new object[] { "Failed to start child scope: ", scope, " in ", this }));
                }
                return false;
            }
            if ((log != null) && log.get_IsDebugEnabled())
            {
                log.Debug(string.Concat(new object[] { "Add child scope: ", scope, " to ", this }));
            }
            this._children[scope.Type + Separator + scope.Name] = scope;
            return true;
        }

        public void AddService(Type serviceType, object service)
        {
            this._serviceContainer.AddService(serviceType, service);
        }

        public void AddService(Type serviceType, object service, bool promote)
        {
            this._serviceContainer.AddService(serviceType, service, promote);
        }

        public bool Connect(IConnection connection)
        {
            return this.Connect(connection, null);
        }

        public bool Connect(IConnection connection, object[] parameters)
        {
            if (!(!base.HasParent || this.Parent.Connect(connection, parameters)))
            {
                return false;
            }
            if (!(!this.HasHandler || this.Handler.Connect(connection, this, parameters)))
            {
                return false;
            }
            IClient client = connection.Client;
            if (!connection.IsConnected)
            {
                return false;
            }
            if (!(!this.HasHandler || this.Handler.Join(client, this)))
            {
                return false;
            }
            if (!connection.IsConnected)
            {
                return false;
            }
            CopyOnWriteArraySet<IConnection> set = null;
            if (this._clients.ContainsKey(client))
            {
                set = this._clients[client];
            }
            else
            {
                set = new CopyOnWriteArraySet<IConnection>();
                this._clients[client] = set;
            }
            set.Add(connection);
            this.AddEventListener(connection);
            return true;
        }

        public bool CreateChildScope(string name)
        {
            Scope scope = new Scope(name, this._serviceContainer) {
                Parent = this
            };
            return this.AddChildScope(scope);
        }

        public void Disconnect(IConnection connection)
        {
            IClient key = connection.Client;
            if (this._clients.ContainsKey(key))
            {
                Exception exception;
                CopyOnWriteArraySet<IConnection> set = this._clients[key];
                set.Remove(connection);
                IScopeHandler handler = null;
                if (this.HasHandler)
                {
                    handler = this.Handler;
                    try
                    {
                        handler.Disconnect(connection, this);
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        if ((log != null) && log.get_IsErrorEnabled())
                        {
                            log.Error(string.Concat(new object[] { "Error while executing \"disconnect\" for connection ", connection, " on handler ", handler }), exception);
                        }
                    }
                }
                if (set.Count == 0)
                {
                    this._clients.Remove(key);
                    if (handler != null)
                    {
                        try
                        {
                            handler.Leave(key, this);
                        }
                        catch (Exception exception2)
                        {
                            exception = exception2;
                            if ((log != null) && log.get_IsErrorEnabled())
                            {
                                log.Error(string.Concat(new object[] { "Error while executing \"leave\" for client ", key, " on handler ", handler }), exception);
                            }
                        }
                    }
                }
                this.RemoveEventListener(connection);
            }
            if (base.HasParent)
            {
                this.Parent.Disconnect(connection);
            }
        }

        protected override void Free()
        {
            if (base.HasParent)
            {
                this.Parent.RemoveChildScope(this);
            }
            if (this.HasHandler)
            {
                this.Handler.Stop(this);
            }
        }

        public IBasicScope GetBasicScope(string type, string name)
        {
            string key = type + Separator + name;
            if (this._children.ContainsKey(key))
            {
                return this._children[key];
            }
            return null;
        }

        public IEnumerator GetBasicScopeNames(string type)
        {
            if (type == null)
            {
                return this._children.Keys.GetEnumerator();
            }
            return new PrefixFilteringStringEnumerator(this._children.Keys, type + Separator);
        }

        public ICollection GetClients()
        {
            return this._clients.Keys;
        }

        public IEnumerator GetConnections()
        {
            return new ConnectionIterator(this);
        }

        public IScopeContext GetContext()
        {
            if (!(this.HasContext || !base.HasParent))
            {
                return base._parent.Context;
            }
            return this._context;
        }

        public override IEnumerator GetEnumerator()
        {
            return this._children.Values.GetEnumerator();
        }

        public IResource GetResource(string path)
        {
            if (this.HasContext)
            {
                return this._context.GetResource(path);
            }
            return this.Context.GetResource(this.ContextPath + '/' + path);
        }

        public IScope GetScope(string name)
        {
            string key = ScopeType + Separator + name;
            if (this._children.ContainsKey(key))
            {
                return (this._children[key] as IScope);
            }
            return null;
        }

        public ICollection GetScopeNames()
        {
            return this._children.Keys;
        }

        public virtual object GetService(Type serviceType)
        {
            return this._serviceContainer.GetService(serviceType);
        }

        public bool HasChildScope(string name)
        {
            return this._children.ContainsKey(ScopeType + Separator + name);
        }

        public bool HasChildScope(string type, string name)
        {
            return this._children.ContainsKey(type + Separator + name);
        }

        public void Init()
        {
            if (((!base.HasParent || this.Parent.HasChildScope(this.Name)) || this.Parent.AddChildScope(this)) && this.AutoStart)
            {
                this.Start();
            }
        }

        public ICollection LookupConnections(IClient client)
        {
            if (this._clients.ContainsKey(client))
            {
                return this._clients[client];
            }
            return null;
        }

        public void RemoveChildScope(IBasicScope scope)
        {
            if ((scope is IScope) && this.HasHandler)
            {
                this.Handler.Stop((IScope) scope);
            }
            string key = scope.Type + Separator + scope.Name;
            if (this._children.ContainsKey(key))
            {
                this._children.Remove(key);
            }
            if (this.HasHandler)
            {
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    log.Debug("Remove child scope");
                }
                this.Handler.RemoveChildScope(scope);
            }
        }

        public void RemoveService(Type serviceType)
        {
            this._serviceContainer.RemoveService(serviceType);
        }

        public void RemoveService(Type serviceType, bool promote)
        {
            this._serviceContainer.RemoveService(serviceType, promote);
        }

        public bool Start()
        {
            lock (base.SyncRoot)
            {
                bool flag = false;
                if (this.IsEnabled && !this.IsRunning)
                {
                    if (this.HasHandler)
                    {
                        try
                        {
                            if (this._handler != null)
                            {
                                flag = this._handler.Start(this);
                            }
                        }
                        catch (Exception exception)
                        {
                            log.Error("Could not start scope " + this, exception);
                        }
                    }
                    else
                    {
                        log.Debug(string.Format("Scope {0} has no handler, allowing start.", this));
                        flag = true;
                    }
                    this._running = flag;
                }
                return flag;
            }
        }

        public void Stop()
        {
            lock (base.SyncRoot)
            {
                if ((this.IsEnabled && this.IsRunning) && this.HasHandler)
                {
                    try
                    {
                        if (this._handler != null)
                        {
                            this._handler.Stop(this);
                        }
                    }
                    catch (Exception exception)
                    {
                        log.Error("Could not stop scope " + this, exception);
                    }
                }
                this._serviceContainer.Shutdown();
                this._running = false;
            }
        }

        public void Uninit()
        {
            foreach (IBasicScope scope in this._children.Values)
            {
                if (scope is Scope)
                {
                    ((Scope) scope).Uninit();
                }
            }
            this.Stop();
            if (base.HasParent && this.Parent.HasChildScope(this.Name))
            {
                this.Parent.RemoveChildScope(this);
            }
        }

        public bool AutoStart
        {
            get
            {
                return this._autoStart;
            }
            set
            {
                this._autoStart = value;
            }
        }

        public IScopeContext Context
        {
            get
            {
                if (!(this.HasContext || !base.HasParent))
                {
                    return this.Parent.Context;
                }
                return this._context;
            }
            set
            {
                this._context = value;
            }
        }

        public virtual string ContextPath
        {
            get
            {
                if (this.HasContext)
                {
                    return string.Empty;
                }
                if (base.HasParent)
                {
                    return (this.Parent.ContextPath + "/" + this.Name);
                }
                return null;
            }
        }

        public IScopeHandler Handler
        {
            get
            {
                if (this._handler != null)
                {
                    return this._handler;
                }
                if (base.HasParent)
                {
                    return this.Parent.Handler;
                }
                return null;
            }
            set
            {
                this._handler = value;
                if (this._handler is IScopeAware)
                {
                    (this._handler as IScopeAware).SetScope(this);
                }
            }
        }

        public bool HasContext
        {
            get
            {
                return (this._context != null);
            }
        }

        public bool HasHandler
        {
            get
            {
                return ((this._handler != null) || (base.HasParent && this.Parent.HasHandler));
            }
        }

        public bool IsEnabled
        {
            get
            {
                return this._enabled;
            }
            set
            {
                this._enabled = value;
            }
        }

        public bool IsRunning
        {
            get
            {
                return this._running;
            }
        }

        private sealed class ConnectionIterator : IEnumerator
        {
            private IEnumerator _connectionIterator;
            private IDictionaryEnumerator _setIterator;

            public ConnectionIterator(Scope scope)
            {
                this._setIterator = scope._clients.GetEnumerator();
            }

            public bool MoveNext()
            {
                if ((this._connectionIterator != null) && this._connectionIterator.MoveNext())
                {
                    return true;
                }
                if (this._setIterator.MoveNext())
                {
                    this._connectionIterator = (this._setIterator.Value as CopyOnWriteArraySet<IConnection>).GetEnumerator();
                    while (this._connectionIterator != null)
                    {
                        if (this._connectionIterator.MoveNext())
                        {
                            return true;
                        }
                        if (!this._setIterator.MoveNext())
                        {
                            return false;
                        }
                        this._connectionIterator = (this._setIterator.Value as CopyOnWriteArraySet<IConnection>).GetEnumerator();
                    }
                }
                return false;
            }

            public void Reset()
            {
                this._connectionIterator = null;
                this._setIterator.Reset();
            }

            public object Current
            {
                get
                {
                    return this._connectionIterator.Current;
                }
            }
        }

        private sealed class PrefixFilteringStringEnumerator : IEnumerator
        {
            private string _currentElement;
            private object[] _enumerable = null;
            private int _index;
            private string _prefix;

            internal PrefixFilteringStringEnumerator(ICollection enumerable, string prefix)
            {
                this._prefix = prefix;
                this._index = -1;
                this._enumerable = new object[enumerable.Count];
                enumerable.CopyTo(this._enumerable, 0);
            }

            public bool MoveNext()
            {
                while (this._index < (this._enumerable.Length - 1))
                {
                    this._index++;
                    string str = this._enumerable[this._index] as string;
                    if (str.StartsWith(this._prefix))
                    {
                        this._currentElement = str;
                        return true;
                    }
                }
                this._index = this._enumerable.Length;
                return false;
            }

            public void Reset()
            {
                this._currentElement = null;
                this._index = -1;
            }

            public string Current
            {
                get
                {
                    if (this._index == -1)
                    {
                        throw new InvalidOperationException("Enum not started.");
                    }
                    if (this._index >= this._enumerable.Length)
                    {
                        throw new InvalidOperationException("Enumeration ended.");
                    }
                    return this._currentElement;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    if (this._index == -1)
                    {
                        throw new InvalidOperationException("Enum not started.");
                    }
                    if (this._index >= this._enumerable.Length)
                    {
                        throw new InvalidOperationException("Enumeration ended.");
                    }
                    return this._currentElement;
                }
            }
        }
    }
}

