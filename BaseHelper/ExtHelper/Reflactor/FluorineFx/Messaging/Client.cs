namespace FluorineFx.Messaging
{
    using FluorineFx.Collections;
    using FluorineFx.Context;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Messages;
    using log4net;
    using System;
    using System.Collections;
    using System.Threading;

    internal class Client : AttributeStore, IClient, IAttributeStore
    {
        private int _clientLeaseTime;
        private ClientManager _clientManager;
        protected CopyOnWriteDictionary _connectionToScope = new CopyOnWriteDictionary();
        private string _id;
        private CopyOnWriteArray _messageClients;
        private bool _polling;
        private Hashtable _sessionDestroyedListeners;
        private static object _syncLock = new object();
        private static readonly ILog log = LogManager.GetLogger(typeof(Client));

        internal Client(ClientManager clientManager, string id)
        {
            this._clientManager = clientManager;
            this._id = id;
            this._clientLeaseTime = 1;
            this._polling = false;
        }

        public void AddSessionDestroyedListener(ISessionListener listener)
        {
            if (listener != null)
            {
                lock (this.SyncRoot)
                {
                    if (this._sessionDestroyedListeners == null)
                    {
                        this._sessionDestroyedListeners = new Hashtable(1);
                    }
                    this._sessionDestroyedListeners[listener] = null;
                }
            }
        }

        public void Disconnect()
        {
            this.Disconnect(false);
        }

        public void Disconnect(bool timeout)
        {
            lock (this.SyncRoot)
            {
                IConnection current = null;
                if ((this.Connections != null) && (this.Connections.Count > 0))
                {
                    IEnumerator enumerator = this.Connections.GetEnumerator();
                    enumerator.MoveNext();
                    current = enumerator.Current as IConnection;
                }
                if (FluorineContext.Current == null)
                {
                    _TimeoutContext context = new _TimeoutContext(current, this);
                    WebSafeCallContext.SetData("__@fluorinecontext", context);
                }
                this._clientManager.RemoveSubscriber(this);
                if (this._sessionDestroyedListeners != null)
                {
                    foreach (ISessionListener listener in this._sessionDestroyedListeners.Keys)
                    {
                        listener.SessionDestroyed(this);
                    }
                }
                if (this._messageClients != null)
                {
                    foreach (MessageClient client in this._messageClients)
                    {
                        if (timeout)
                        {
                            client.Timeout();
                        }
                        else
                        {
                            client.Disconnect();
                        }
                    }
                    this._messageClients.Clear();
                }
                foreach (IConnection connection2 in this.Connections)
                {
                    if (timeout)
                    {
                        connection2.Timeout();
                    }
                    connection2.Close();
                }
            }
        }

        public IMessage[] GetPendingMessages(int waitIntervalMillis)
        {
            ArrayList list = new ArrayList();
            this._polling = true;
            do
            {
                this._clientManager.LookupClient(this._id);
                foreach (MessageClient client in this.MessageClients)
                {
                    client.Renew();
                    list.AddRange(client.GetPendingMessages());
                }
                if (waitIntervalMillis == 0)
                {
                    this._polling = false;
                    return (list.ToArray(typeof(IMessage)) as IMessage[]);
                }
                if (list.Count > 0)
                {
                    this._polling = false;
                    return (list.ToArray(typeof(IMessage)) as IMessage[]);
                }
                Thread.Sleep(500);
                waitIntervalMillis -= 500;
                if (waitIntervalMillis <= 0)
                {
                    this._polling = false;
                }
            }
            while (this._polling);
            return (list.ToArray(typeof(IMessage)) as IMessage[]);
        }

        public void Register(IConnection connection)
        {
            this._connectionToScope.Add(connection, connection.Scope);
        }

        public void RegisterMessageClient(IMessageClient messageClient)
        {
            if (!this.MessageClients.Contains(messageClient))
            {
                this.MessageClients.Add(messageClient);
            }
        }

        public void RemoveSessionDestroyedListener(ISessionListener listener)
        {
            if (listener != null)
            {
                lock (this.SyncRoot)
                {
                    if ((this._sessionDestroyedListeners != null) && this._sessionDestroyedListeners.Contains(listener))
                    {
                        this._sessionDestroyedListeners.Remove(listener);
                    }
                }
            }
        }

        public void Renew()
        {
            this._clientManager.LookupClient(this._id);
        }

        public void Renew(int clientLeaseTime)
        {
            this._clientManager.Renew(this, clientLeaseTime);
        }

        internal void SetClientLeaseTime(int value)
        {
            this._clientLeaseTime = value;
        }

        public void Timeout()
        {
            if (log.get_IsDebugEnabled())
            {
                log.Debug(string.Format("Timeout Client {0}", this.Id));
            }
            this.Disconnect(true);
        }

        public override string ToString()
        {
            return ("Client " + this._id.ToString());
        }

        public void Unregister(IConnection connection)
        {
            this._connectionToScope.Remove(connection);
            if (this._connectionToScope.Count == 0)
            {
                this.Disconnect();
            }
        }

        public void UnregisterMessageClient(IMessageClient messageClient)
        {
            if (!messageClient.IsDisconnecting)
            {
                if (this.MessageClients.Contains(messageClient))
                {
                    this.MessageClients.Remove(messageClient);
                }
                if (this.MessageClients.Count == 0)
                {
                    this.Disconnect();
                }
            }
        }

        public int ClientLeaseTime
        {
            get
            {
                return this._clientLeaseTime;
            }
        }

        public ICollection Connections
        {
            get
            {
                return this._connectionToScope.Keys;
            }
        }

        public string Id
        {
            get
            {
                return this._id;
            }
        }

        internal IList MessageClients
        {
            get
            {
                if (this._messageClients == null)
                {
                    lock (this.SyncRoot)
                    {
                        if (this._messageClients == null)
                        {
                            this._messageClients = new CopyOnWriteArray();
                        }
                    }
                }
                return this._messageClients;
            }
        }

        public ICollection Scopes
        {
            get
            {
                return this._connectionToScope.Values;
            }
        }

        public object SyncRoot
        {
            get
            {
                return _syncLock;
            }
        }
    }
}

