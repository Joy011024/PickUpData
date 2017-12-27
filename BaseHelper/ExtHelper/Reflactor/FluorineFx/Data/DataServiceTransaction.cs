namespace FluorineFx.Data
{
    using FluorineFx;
    using FluorineFx.Context;
    using FluorineFx.Data.Messages;
    using FluorineFx.Exceptions;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Messages;
    using log4net;
    using System;
    using System.Collections;
    using System.Threading;

    public class DataServiceTransaction
    {
        private string _clientId;
        private Hashtable _clientUpdateCollectionMessages;
        private string _correlationId;
        private DataService _dataService;
        private static int _idCounter;
        private ArrayList _outgoingMessages;
        private ArrayList _processedMessageBatches;
        private ArrayList _pushMessages;
        private ArrayList _refreshFills;
        private bool _rollbackOnly;
        private bool _sendMessagesToPeers;
        private FluorineFx.Data.TransactionState _transactionState = FluorineFx.Data.TransactionState.Active;
        private Hashtable _updateCollectionMessages;
        private static readonly ILog log = LogManager.GetLogger(typeof(DataServiceTransaction));

        private DataServiceTransaction(DataService dataService)
        {
            this._dataService = dataService;
            this._sendMessagesToPeers = true;
            this._rollbackOnly = false;
            this._outgoingMessages = new ArrayList();
            this._processedMessageBatches = new ArrayList(1);
            this._updateCollectionMessages = new Hashtable(new ListHashCodeProvider());
            this._clientUpdateCollectionMessages = new Hashtable(new ListHashCodeProvider());
        }

        internal void AddClientUpdateCollection(UpdateCollectionMessage updateCollectionMessage)
        {
            this._clientUpdateCollectionMessages[updateCollectionMessage.collectionId] = updateCollectionMessage;
        }

        internal void AddProcessedMessageBatch(MessageBatch messageBatch)
        {
            this._processedMessageBatches.Add(messageBatch);
        }

        public static DataServiceTransaction Begin()
        {
            return Begin((string) null);
        }

        internal static DataServiceTransaction Begin(DataService dataService)
        {
            DataServiceTransaction dataServiceTransaction = new DataServiceTransaction(dataService);
            SetCurrentDataServiceTransaction(dataServiceTransaction);
            return dataServiceTransaction;
        }

        public static DataServiceTransaction Begin(string serverId)
        {
            if (CurrentDataServiceTransaction == null)
            {
                DataService serviceByMessageType = MessageBroker.GetMessageBroker(serverId).GetServiceByMessageType("flex.data.messages.DataMessage") as DataService;
                return Begin(serviceByMessageType);
            }
            return CurrentDataServiceTransaction;
        }

        public void Commit()
        {
            if (this._rollbackOnly)
            {
                this.Rollback();
            }
            else
            {
                try
                {
                    int num;
                    IMessage message2;
                    UpdateCollectionMessage message4;
                    this.ProcessRefreshFills();
                    this._pushMessages = new ArrayList();
                    for (num = 0; num < this._processedMessageBatches.Count; num++)
                    {
                        MessageBatch messageBatch = this._processedMessageBatches[num] as MessageBatch;
                        if ((messageBatch.Messages != null) && (messageBatch.Messages.Count > 0))
                        {
                            DataDestination destination = this._dataService.GetDestination(messageBatch.IncomingMessage) as DataDestination;
                            try
                            {
                                destination.SequenceManager.ManageMessageBatch(messageBatch, this);
                            }
                            catch (Exception exception)
                            {
                                ErrorMessage errorMessage = new MessageException(exception).GetErrorMessage();
                                errorMessage.correlationId = messageBatch.IncomingMessage.messageId;
                                errorMessage.destination = messageBatch.IncomingMessage.destination;
                                messageBatch.Messages.Clear();
                                messageBatch.Messages.Add(errorMessage);
                            }
                            for (int i = 0; i < messageBatch.Messages.Count; i++)
                            {
                                message2 = messageBatch.Messages[i] as IMessage;
                                if (!(message2 is ErrorMessage))
                                {
                                    this._pushMessages.Add(message2);
                                }
                            }
                        }
                        this._outgoingMessages.AddRange(messageBatch.Messages);
                    }
                    for (num = 0; num < this._pushMessages.Count; num++)
                    {
                        message2 = this._pushMessages[num] as IMessage;
                        DataMessage message3 = message2 as DataMessage;
                        if (message3 != null)
                        {
                            this.PushMessage(this.GetSubscribers(message2), message2);
                        }
                    }
                    foreach (DictionaryEntry entry in this._clientUpdateCollectionMessages)
                    {
                        message4 = entry.Value as UpdateCollectionMessage;
                        this._outgoingMessages.Add(message4);
                        this.PushMessage(this.GetSubscribers(message4), message4);
                    }
                    foreach (DictionaryEntry entry in this._updateCollectionMessages)
                    {
                        message4 = entry.Value as UpdateCollectionMessage;
                        this._outgoingMessages.Add(message4);
                        this.PushMessage(this.GetSubscribers(message4), message4);
                    }
                }
                finally
                {
                    this._transactionState = FluorineFx.Data.TransactionState.Committed;
                }
            }
        }

        public void CreateItem(string destination, object item)
        {
            DataMessage message = new DataMessage();
            DataDestination destination2 = this._dataService.GetDestination(destination) as DataDestination;
            message.operation = 0;
            message.body = item;
            message.destination = destination;
            if (this._clientId != null)
            {
                message.clientId = this._clientId;
            }
            else
            {
                message.clientId = "srv:" + Guid.NewGuid().ToString("D");
            }
            message.identity = Identity.GetIdentity(item, destination2);
            message.messageId = "srv:" + Guid.NewGuid().ToString("D") + ":" + _idCounter.ToString();
            Interlocked.Increment(ref _idCounter);
            ArrayList messages = new ArrayList(1);
            messages.Add(message);
            MessageBatch messageBatch = new MessageBatch(message, messages);
            this.AddProcessedMessageBatch(messageBatch);
        }

        private UpdateCollectionMessage CreateUpdateCollectionMessage(DataDestination dataDestination, Sequence sequence)
        {
            UpdateCollectionMessage message = new UpdateCollectionMessage {
                clientId = this.ClientId,
                updateMode = 1,
                collectionId = sequence.Parameters,
                destination = dataDestination.Id,
                correlationId = this.CorrelationId,
                messageId = "srv:" + Guid.NewGuid().ToString("D") + ":" + _idCounter.ToString()
            };
            Interlocked.Increment(ref _idCounter);
            return message;
        }

        public void DeleteItem(string destination, object item)
        {
            DataMessage message = new DataMessage();
            DataDestination destination2 = this._dataService.GetDestination(destination) as DataDestination;
            message.operation = 4;
            message.body = item;
            message.destination = destination;
            if (this._clientId != null)
            {
                message.clientId = this._clientId;
            }
            else
            {
                message.clientId = "srv:" + Guid.NewGuid().ToString("D");
            }
            message.identity = Identity.GetIdentity(item, destination2);
            message.messageId = "srv:" + Guid.NewGuid().ToString("D") + ":" + _idCounter.ToString();
            Interlocked.Increment(ref _idCounter);
            ArrayList messages = new ArrayList(1);
            messages.Add(message);
            MessageBatch messageBatch = new MessageBatch(message, messages);
            this.AddProcessedMessageBatch(messageBatch);
        }

        public void DeleteItemWithId(string destination, Hashtable identity)
        {
            DataMessage message = new DataMessage();
            DataDestination destination2 = this._dataService.GetDestination(destination) as DataDestination;
            message.operation = 4;
            message.body = null;
            message.destination = destination;
            if (this._clientId != null)
            {
                message.clientId = this._clientId;
            }
            else
            {
                message.clientId = "srv:" + Guid.NewGuid().ToString("D");
            }
            message.identity = identity;
            message.messageId = "srv:" + Guid.NewGuid().ToString("D") + ":" + _idCounter.ToString();
            Interlocked.Increment(ref _idCounter);
            ArrayList messages = new ArrayList(1);
            messages.Add(message);
            MessageBatch messageBatch = new MessageBatch(message, messages);
            this.AddProcessedMessageBatch(messageBatch);
        }

        internal void GenerateUpdateCollectionMessage(int updateType, DataDestination dataDestination, Sequence sequence, int position, Identity identity)
        {
            UpdateCollectionMessage message = this.CreateUpdateCollectionMessage(dataDestination, sequence);
            message.AddItemIdentityChange(updateType, position, identity);
            if (message.collectionId != null)
            {
                this._updateCollectionMessages[message.collectionId] = message;
            }
            else
            {
                this._updateCollectionMessages[new object[0]] = message;
            }
        }

        internal IList GetOutgoingMessages()
        {
            return this._outgoingMessages;
        }

        private ICollection GetSubscribers(IMessage message)
        {
            MessageDestination destination = this._dataService.GetDestination(message) as MessageDestination;
            IList subscribers = destination.SubscriptionManager.GetSubscribers();
            subscribers.Remove(message.clientId);
            return subscribers;
        }

        private void ProcessRefreshFills()
        {
            for (int i = 0; (this._refreshFills != null) && (i < this._refreshFills.Count); i++)
            {
                RefreshFillData data = this._refreshFills[i] as RefreshFillData;
                DataDestination destination = this._dataService.GetDestination(data.Destination) as DataDestination;
                if (destination == null)
                {
                    throw new FluorineException(__Res.GetString("Destination_NotFound", new object[] { data.Destination }));
                }
                ICollection sequences = destination.SequenceManager.GetSequences(data.Parameters);
                if (sequences != null)
                {
                    lock (destination.SequenceManager.SyncRoot)
                    {
                        foreach (Sequence sequence in sequences)
                        {
                            DataMessage message = new DataMessage {
                                operation = 1
                            };
                            if (sequence.Parameters != null)
                            {
                                message.body = sequence.Parameters;
                            }
                            else
                            {
                                message.body = new object[0];
                            }
                            if (this._clientId != null)
                            {
                                message.clientId = this._clientId;
                            }
                            else
                            {
                                message.clientId = "srv:" + Guid.NewGuid().ToString("D");
                            }
                            IList result = destination.ServiceAdapter.Invoke(message) as IList;
                            if (result.Count > 0)
                            {
                                Sequence sequence2 = destination.SequenceManager.CreateSequence(message.clientId as string, result, sequence.Parameters, this);
                            }
                        }
                    }
                }
            }
        }

        private void PushMessage(ICollection subscribers, IMessage message)
        {
            this._dataService.PushMessageToClients(subscribers, message);
        }

        public void RefreshFill(string destination, IList fillParameters)
        {
            if (this._refreshFills == null)
            {
                this._refreshFills = new ArrayList(1);
            }
            this._refreshFills.Add(new RefreshFillData(destination, fillParameters));
        }

        public void Rollback()
        {
            try
            {
            }
            finally
            {
                this._transactionState = FluorineFx.Data.TransactionState.RolledBack;
            }
        }

        private static void SetCurrentDataServiceTransaction(DataServiceTransaction dataServiceTransaction)
        {
            if (FluorineContext.Current != null)
            {
                FluorineContext.Current.Items.Add("__@fluorinedataservicetransaction", dataServiceTransaction);
            }
            else
            {
                WebSafeCallContext.SetData("__@fluorinedataservicetransaction", dataServiceTransaction);
            }
        }

        public void SetRollbackOnly()
        {
            this._rollbackOnly = true;
        }

        public void UpdateItem(string destination, object newVersion, object previousVersion, string[] changes)
        {
            DataMessage message = new DataMessage();
            DataDestination destination2 = this._dataService.GetDestination(destination) as DataDestination;
            object[] objArray = new object[3];
            objArray[0] = changes;
            objArray[2] = newVersion;
            objArray[1] = previousVersion;
            message.operation = 3;
            message.body = objArray;
            message.destination = destination;
            if (this._clientId != null)
            {
                message.clientId = this._clientId;
            }
            else
            {
                message.clientId = "srv:" + Guid.NewGuid().ToString("D");
            }
            message.identity = Identity.GetIdentity(newVersion, destination2);
            message.messageId = "srv:" + Guid.NewGuid().ToString("D") + ":" + _idCounter.ToString();
            Interlocked.Increment(ref _idCounter);
            ArrayList messages = new ArrayList(1);
            messages.Add(message);
            MessageBatch batch = new MessageBatch(message, messages);
            this._processedMessageBatches.Add(batch);
        }

        internal string ClientId
        {
            get
            {
                return this._clientId;
            }
            set
            {
                this._clientId = value;
            }
        }

        internal string CorrelationId
        {
            get
            {
                return this._correlationId;
            }
            set
            {
                this._correlationId = value;
            }
        }

        public static DataServiceTransaction CurrentDataServiceTransaction
        {
            get
            {
                if (FluorineContext.Current != null)
                {
                    return (FluorineContext.Current.Items["__@fluorinedataservicetransaction"] as DataServiceTransaction);
                }
                return (WebSafeCallContext.GetData("__@fluorinedataservicetransaction") as DataServiceTransaction);
            }
        }

        public bool SendMessagesToPeers
        {
            get
            {
                return this._sendMessagesToPeers;
            }
            set
            {
                this._sendMessagesToPeers = value;
            }
        }

        public FluorineFx.Data.TransactionState TransactionState
        {
            get
            {
                return this._transactionState;
            }
        }

        private sealed class RefreshFillData
        {
            private string _destination;
            private IList _parameters;

            public RefreshFillData(string destination, IList parameters)
            {
                this._destination = destination;
                this._parameters = parameters;
            }

            public string Destination
            {
                get
                {
                    return this._destination;
                }
            }

            public IList Parameters
            {
                get
                {
                    return this._parameters;
                }
            }
        }
    }
}

