namespace FluorineFx.Messaging.Services.Messaging
{
    using FluorineFx;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Messages;
    using log4net;
    using System;
    using System.Collections;
    using System.Web;
    using System.Web.Caching;

    internal class SubscriptionManager
    {
        private MessageDestination _messageDestination;
        private static object _objLock = new object();
        private Hashtable _subscribers;
        private static readonly ILog log = LogManager.GetLogger(typeof(SubscriptionManager));

        public SubscriptionManager(MessageDestination messageDestination)
        {
            this._messageDestination = messageDestination;
            this._subscribers = new Hashtable();
        }

        private void AddSubscriber(MessageClient messageClient)
        {
            lock (_objLock)
            {
                if (!this._subscribers.Contains(messageClient.ClientId))
                {
                    this._subscribers[messageClient.ClientId] = messageClient;
                    int minutes = 20;
                    if (this._messageDestination.DestinationSettings.NetworkSettings != null)
                    {
                        minutes = this._messageDestination.DestinationSettings.NetworkSettings.SessionTimeout;
                    }
                    HttpRuntime.Cache.Insert(messageClient.ClientId, messageClient, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, minutes, 0), CacheItemPriority.NotRemovable, new CacheItemRemovedCallback(this.RemovedCallback));
                }
            }
        }

        public MessageClient AddSubscriber(IClient client, string messageClientId, string endpointId, MessageDestination messageDestination, Subtopic subtopic, Selector selector)
        {
            lock (_objLock)
            {
                if (subtopic != null)
                {
                    MessagingAdapter serviceAdapter = this._messageDestination.ServiceAdapter as MessagingAdapter;
                    if ((serviceAdapter != null) && !serviceAdapter.AllowSubscribe(subtopic))
                    {
                        ASObject extendedData = new ASObject();
                        extendedData["subtopic"] = subtopic.Value;
                        throw new MessageException(extendedData);
                    }
                }
                if (!this._subscribers.Contains(messageClientId))
                {
                    MessageClient messageClient = new MessageClient(client, messageDestination.SubscriptionManager, messageClientId, endpointId, messageDestination) {
                        Subtopic = subtopic,
                        Selector = selector
                    };
                    client.RegisterMessageClient(messageClient);
                    this.AddSubscriber(messageClient);
                    return messageClient;
                }
                return (this._subscribers[messageClientId] as MessageClient);
            }
        }

        public MessageClient GetSubscriber(string clientId)
        {
            if (clientId == null)
            {
                return null;
            }
            lock (_objLock)
            {
                MessageClient client = null;
                if (this._subscribers.Contains(clientId))
                {
                    client = this._subscribers[clientId] as MessageClient;
                    HttpRuntime.Cache.Get(clientId);
                }
                return client;
            }
        }

        public IList GetSubscribers()
        {
            lock (_objLock)
            {
                return new ArrayList(this._subscribers.Keys);
            }
        }

        public IList GetSubscribers(IMessage message)
        {
            return this.GetSubscribers(message, true);
        }

        public IList GetSubscribers(IMessage message, bool evalSelector)
        {
            lock (_objLock)
            {
                bool flag = true;
                if ((message.headers == null) || (message.headers.Count == 0))
                {
                    flag = false;
                }
                if (!flag)
                {
                    return this.GetSubscribers();
                }
                Subtopic subtopic = null;
                if (message.headers.ContainsKey("DSSubtopic"))
                {
                    subtopic = new Subtopic(message.headers["DSSubtopic"] as string);
                    MessagingAdapter serviceAdapter = this._messageDestination.ServiceAdapter as MessagingAdapter;
                    if ((serviceAdapter != null) && !serviceAdapter.AllowSend(subtopic))
                    {
                        return null;
                    }
                }
                ArrayList list = new ArrayList();
                foreach (MessageClient client in this._subscribers.Values)
                {
                    bool flag2 = true;
                    if ((subtopic != null) && (client.Subtopic != null))
                    {
                        flag2 = flag2 && subtopic.Matches(client.Subtopic);
                    }
                    if ((client.Selector != null) && evalSelector)
                    {
                        flag2 = flag2 && client.Selector.Evaluate(null, message.headers);
                    }
                    if (flag2)
                    {
                        list.Add(client.ClientId);
                    }
                }
                return list;
            }
        }

        public void RemovedCallback(string key, object value, CacheItemRemovedReason callbackReason)
        {
            if (callbackReason == CacheItemRemovedReason.Expired)
            {
                lock (_objLock)
                {
                    if (this._subscribers.Contains(key))
                    {
                        try
                        {
                            MessageClient subscriber = this.GetSubscriber(key);
                            if (subscriber != null)
                            {
                                if (log.get_IsDebugEnabled())
                                {
                                    log.Debug(__Res.GetString("SubscriptionManager_CacheExpired", new object[] { subscriber.ClientId }));
                                }
                                if (this._messageDestination != null)
                                {
                                    MessageBroker messageBroker = this._messageDestination.Service.GetMessageBroker();
                                    subscriber.Timeout();
                                }
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

        public MessageClient RemoveSubscriber(MessageClient messageClient)
        {
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("SubscriptionManager_Remove", new object[] { messageClient.ClientId }));
            }
            lock (_objLock)
            {
                this.RemoveSubscriber(messageClient.ClientId);
                return messageClient;
            }
        }

        public MessageClient RemoveSubscriber(string clientId)
        {
            lock (_objLock)
            {
                MessageClient client = this._subscribers[clientId] as MessageClient;
                HttpRuntime.Cache.Remove(clientId);
                this._subscribers.Remove(clientId);
                return client;
            }
        }
    }
}

