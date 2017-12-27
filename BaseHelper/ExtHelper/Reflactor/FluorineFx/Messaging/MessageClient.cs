namespace FluorineFx.Messaging
{
    using FluorineFx;
    using FluorineFx.Context;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Services;
    using FluorineFx.Messaging.Services.Messaging;
    using log4net;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Text;

    public sealed class MessageClient : IMessageClient
    {
        private byte[] _binaryId;
        private IClient _client;
        private IMessageConnection _connection;
        private string _endpoint;
        private bool _isDisconnecting;
        private static Hashtable _messageClientCreatedListeners;
        private Hashtable _messageClientDestroyedListeners;
        private string _messageClientId;
        private MessageDestination _messageDestination;
        private ArrayList _messageQueue;
        private FluorineFx.Messaging.Services.Messaging.Selector _selector;
        private SubscriptionManager _subscriptionManager;
        private FluorineFx.Messaging.Services.Messaging.Subtopic _subtopic;
        private static object _syncLock = new object();
        private static readonly ILog log = LogManager.GetLogger(typeof(MessageClient));

        private MessageClient()
        {
        }

        internal MessageClient(IClient client, SubscriptionManager subscriptionManager, string messageClientId, string endpoint, MessageDestination messageDestination)
        {
            this._client = client;
            this._subscriptionManager = subscriptionManager;
            this._messageClientId = messageClientId;
            Debug.Assert(messageDestination != null);
            this._messageDestination = messageDestination;
            this._endpoint = endpoint;
            this._connection = FluorineContext.Current.Connection as IMessageConnection;
            if (this._connection != null)
            {
                this._connection.RegisterMessageClient(this);
            }
            if (_messageClientCreatedListeners != null)
            {
                foreach (IMessageClientListener listener in _messageClientCreatedListeners.Keys)
                {
                    listener.MessageClientCreated(this);
                }
            }
        }

        internal void AddMessage(IMessage message)
        {
            lock (this.SyncRoot)
            {
                this.MessageQueue.Add(message);
            }
        }

        public static void AddMessageClientCreatedListener(IMessageClientListener listener)
        {
            lock (typeof(MessageClient))
            {
                if (_messageClientCreatedListeners == null)
                {
                    _messageClientCreatedListeners = new Hashtable(1);
                }
                _messageClientCreatedListeners[listener] = null;
            }
        }

        public void AddMessageClientDestroyedListener(IMessageClientListener listener)
        {
            if (this._messageClientDestroyedListeners == null)
            {
                this._messageClientDestroyedListeners = new Hashtable(1);
            }
            this._messageClientDestroyedListeners[listener] = null;
        }

        internal void Disconnect()
        {
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("MessageClient_Disconnect", new object[] { this.ClientId }));
            }
            this.SetIsDisconnecting(true);
            this.Unsubscribe(false);
        }

        public byte[] GetBinaryId()
        {
            if (this._binaryId == null)
            {
                this._binaryId = new UTF8Encoding().GetBytes(this._messageClientId);
            }
            return this._binaryId;
        }

        internal IMessage[] GetPendingMessages()
        {
            lock (this.SyncRoot)
            {
                IMessage[] messageArray = this.MessageQueue.ToArray(typeof(IMessage)) as IMessage[];
                this.MessageQueue.Clear();
                return messageArray;
            }
        }

        public static void RemoveMessageClientCreatedListener(IMessageClientListener listener)
        {
            lock (typeof(MessageClient))
            {
                if ((_messageClientCreatedListeners != null) && _messageClientCreatedListeners.Contains(listener))
                {
                    _messageClientCreatedListeners.Remove(listener);
                }
            }
        }

        public void RemoveMessageClientDestroyedListener(IMessageClientListener listener)
        {
            if ((this._messageClientDestroyedListeners != null) && this._messageClientDestroyedListeners.Contains(listener))
            {
                this._messageClientDestroyedListeners.Remove(listener);
            }
        }

        public void Renew()
        {
            this._subscriptionManager.GetSubscriber(this._messageClientId);
        }

        internal void SetIsDisconnecting(bool value)
        {
            this._isDisconnecting = value;
        }

        internal void Timeout()
        {
            try
            {
                if (!this.IsDisconnecting && (this._messageDestination != null))
                {
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("MessageClient_Timeout", new object[] { this.ClientId }));
                    }
                    CommandMessage message = new CommandMessage {
                        destination = this.Destination.Id,
                        clientId = this.ClientId,
                        operation = 10
                    };
                    message.headers["DSId"] = this._client.Id;
                    MessageService service = this._messageDestination.Service as MessageService;
                    object[] subscribers = new object[] { message.clientId };
                    service.PushMessageToClients(subscribers, message);
                    this.Unsubscribe(true);
                }
            }
            catch (Exception exception)
            {
                if (log.get_IsErrorEnabled())
                {
                    log.Error(__Res.GetString("MessageClient_Timeout", new object[] { this.ClientId }), exception);
                }
            }
        }

        internal void Unsubscribe()
        {
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("MessageClient_Unsubscribe", new object[] { this.ClientId }));
            }
            if (this._messageClientDestroyedListeners != null)
            {
                foreach (IMessageClientListener listener in this._messageClientDestroyedListeners.Keys)
                {
                    listener.MessageClientDestroyed(this);
                }
            }
            if (this._messageDestination != null)
            {
                this._messageDestination.RemoveSubscriber(this.ClientId);
            }
            this._client.UnregisterMessageClient(this);
            this._messageDestination = null;
        }

        private void Unsubscribe(bool timeout)
        {
            MessageService service = this._messageDestination.Service as MessageService;
            CommandMessage message = new CommandMessage {
                destination = this.Destination.Id,
                operation = 1,
                clientId = this.ClientId
            };
            if (timeout)
            {
                message.headers[CommandMessage.SessionInvalidatedHeader] = true;
                message.headers[CommandMessage.FluorineMessageClientTimeoutHeader] = true;
                message.headers["DSId"] = this._client.Id;
            }
            service.ServiceMessage(message);
        }

        public string ClientId
        {
            get
            {
                return this._messageClientId;
            }
        }

        internal MessageDestination Destination
        {
            get
            {
                return this._messageDestination;
            }
        }

        public string DestinationId
        {
            get
            {
                return this._messageDestination.Id;
            }
        }

        public string Endpoint
        {
            get
            {
                return this._endpoint;
            }
        }

        public bool IsDisconnecting
        {
            get
            {
                return this._isDisconnecting;
            }
        }

        internal IMessageConnection MessageConnection
        {
            get
            {
                return this._connection;
            }
        }

        internal ArrayList MessageQueue
        {
            get
            {
                if (this._messageQueue == null)
                {
                    lock (this.SyncRoot)
                    {
                        if (this._messageQueue == null)
                        {
                            this._messageQueue = new ArrayList();
                        }
                    }
                }
                return this._messageQueue;
            }
        }

        internal FluorineFx.Messaging.Services.Messaging.Selector Selector
        {
            get
            {
                return this._selector;
            }
            set
            {
                this._selector = value;
            }
        }

        public FluorineFx.Messaging.Services.Messaging.Subtopic Subtopic
        {
            get
            {
                return this._subtopic;
            }
            set
            {
                this._subtopic = value;
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

