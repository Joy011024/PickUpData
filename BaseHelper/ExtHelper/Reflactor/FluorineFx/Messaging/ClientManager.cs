namespace FluorineFx.Messaging
{
    using FluorineFx;
    using FluorineFx.Context;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Messages;
    using log4net;
    using System;
    using System.Collections;
    using System.Web;
    using System.Web.Caching;

    public class ClientManager : IClientRegistry
    {
        private Hashtable _clients;
        private MessageBroker _messageBroker;
        private static object _objLock = new object();
        private static Hashtable _sessionCreatedListeners = new Hashtable();
        private static readonly ILog log = LogManager.GetLogger(typeof(ClientManager));

        private ClientManager()
        {
        }

        internal ClientManager(MessageBroker messageBroker)
        {
            this._messageBroker = messageBroker;
            this._clients = new Hashtable();
        }

        public static void AddSessionCreatedListener(ISessionListener listener)
        {
            if (listener != null)
            {
                lock (_sessionCreatedListeners.SyncRoot)
                {
                    _sessionCreatedListeners[listener] = null;
                }
            }
        }

        public IClient GetClient(IMessage message)
        {
            if (message.HeaderExists("DSId"))
            {
                string header = message.GetHeader("DSId") as string;
                return this.GetClient(header);
            }
            return null;
        }

        public IClient GetClient(string id)
        {
            lock (_objLock)
            {
                if (this._clients.ContainsKey(id))
                {
                    HttpRuntime.Cache.Get(id);
                    return (this._clients[id] as Client);
                }
                if (((id == null) || (id == "nil")) || (id == string.Empty))
                {
                    id = Guid.NewGuid().ToString("N");
                }
                Client client = new Client(this, id);
                int clientLeaseTime = 1;
                log.Debug(string.Format("Creating new Client {0}", id));
                this.Renew(client, clientLeaseTime);
                this.NotifyCreated(client);
                return client;
            }
        }

        internal string GetNextId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public bool HasClient(string id)
        {
            if (id == null)
            {
                return false;
            }
            lock (_objLock)
            {
                return this._clients.ContainsKey(id);
            }
        }

        public IClient LookupClient(string clientId)
        {
            if (clientId == null)
            {
                return null;
            }
            lock (_objLock)
            {
                Client client = null;
                if (this._clients.Contains(clientId))
                {
                    client = this._clients[clientId] as Client;
                    HttpRuntime.Cache.Get(clientId);
                }
                return client;
            }
        }

        protected void NotifyCreated(IClient client)
        {
            lock (_sessionCreatedListeners.SyncRoot)
            {
                foreach (ISessionListener listener in _sessionCreatedListeners.Keys)
                {
                    listener.SessionCreated(client);
                }
            }
        }

        internal void RemovedCallback(string key, object value, CacheItemRemovedReason callbackReason)
        {
            if (callbackReason == CacheItemRemovedReason.Expired)
            {
                lock (_objLock)
                {
                    if (this._clients.Contains(key))
                    {
                        try
                        {
                            IClient client = this.LookupClient(key);
                            if (client != null)
                            {
                                if (log.get_IsDebugEnabled())
                                {
                                    log.Debug(__Res.GetString("SubscriptionManager_CacheExpired", new object[] { client.Id }));
                                }
                                client.Timeout();
                            }
                        }
                        catch (Exception exception)
                        {
                            if (log.get_IsErrorEnabled())
                            {
                                log.Error(__Res.GetString("SubscriptionManager_CacheExpired", new object[] { string.Empty }), exception);
                            }
                        }
                    }
                }
            }
        }

        public static void RemoveSessionCreatedListener(ISessionListener listener)
        {
            if (listener != null)
            {
                lock (_sessionCreatedListeners.SyncRoot)
                {
                    if (_sessionCreatedListeners.Contains(listener))
                    {
                        _sessionCreatedListeners.Remove(listener);
                    }
                }
            }
        }

        internal Client RemoveSubscriber(Client client)
        {
            lock (_objLock)
            {
                this.RemoveSubscriber(client.Id);
                return client;
            }
        }

        internal Client RemoveSubscriber(string clientId)
        {
            lock (_objLock)
            {
                if (log.get_IsDebugEnabled())
                {
                    log.Debug(__Res.GetString("SubscriptionManager_Remove", new object[] { clientId }));
                }
                Client client = this._clients[clientId] as Client;
                HttpRuntime.Cache.Remove(clientId);
                this._clients.Remove(clientId);
                return client;
            }
        }

        internal void Renew(Client client, int clientLeaseTime)
        {
            lock (_objLock)
            {
                this._clients[client.Id] = client;
                HttpRuntime.Cache.Remove(client.Id);
                if (client.ClientLeaseTime < clientLeaseTime)
                {
                    client.SetClientLeaseTime(clientLeaseTime);
                    log.Debug(string.Format("Renew Client {0} clientLeaseTime {1}", client.Id, clientLeaseTime));
                }
                if (clientLeaseTime == 0)
                {
                    client.SetClientLeaseTime(0);
                    log.Debug(string.Format("Renew Client {0} clientLeaseTime {1}", client.Id, clientLeaseTime));
                }
                if (client.ClientLeaseTime != 0)
                {
                    HttpRuntime.Cache.Insert(client.Id, client, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, client.ClientLeaseTime, 0), CacheItemPriority.NotRemovable, new CacheItemRemovedCallback(this.RemovedCallback));
                }
            }
        }
    }
}

