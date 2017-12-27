namespace FluorineFx.Data
{
    using FluorineFx;
    using FluorineFx.Collections;
    using FluorineFx.Data.Messages;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;

    internal class SequenceManager
    {
        private Hashtable _clientIdToSequenceHash;
        private DataDestination _dataDestination;
        private Hashtable _itemIdToItemHash;
        private Hashtable _itemIdToSequenceIdMapHash;
        private static object _objLock = new object();
        private Hashtable _parametersToSequenceIdHash;
        private CopyOnWriteDictionary _sequenceIdToSequenceHash;
        private static readonly ILog log = LogManager.GetLogger(typeof(SequenceManager));

        public SequenceManager(DataDestination dataDestination)
        {
            this._dataDestination = dataDestination;
            this._sequenceIdToSequenceHash = new CopyOnWriteDictionary();
            this._parametersToSequenceIdHash = new Hashtable(new ListHashCodeProvider());
            this._itemIdToSequenceIdMapHash = new Hashtable();
            this._clientIdToSequenceHash = new Hashtable();
            this._itemIdToItemHash = new Hashtable();
        }

        public int AddIdentityToSequence(Sequence sequence, Identity identity, DataServiceTransaction dataServiceTransaction)
        {
            return this.AddIdentityToSequence(sequence, -1, identity, dataServiceTransaction);
        }

        public int AddIdentityToSequence(Sequence sequence, int position, Identity identity, DataServiceTransaction dataServiceTransaction)
        {
            lock (_objLock)
            {
                if ((position == -1) || (position > sequence.Size))
                {
                    position = sequence.Add(identity);
                }
                else
                {
                    sequence.Insert(position, identity);
                }
                IDictionary dictionary = this._itemIdToSequenceIdMapHash[identity] as IDictionary;
                if (dictionary == null)
                {
                    dictionary = new Hashtable();
                    this._itemIdToSequenceIdMapHash[identity] = dictionary;
                }
                dictionary[sequence.Id] = sequence;
                if (dataServiceTransaction != null)
                {
                    dataServiceTransaction.GenerateUpdateCollectionMessage(0, this._dataDestination, sequence, position, identity);
                }
                return position;
            }
        }

        private void ApplyUpdateCollectionMessage(Sequence sequence, UpdateCollectionMessage updateCollectionMessage)
        {
            IList body = updateCollectionMessage.body as IList;
            for (int i = 0; i < body.Count; i++)
            {
                UpdateCollectionRange range = body[i] as UpdateCollectionRange;
                int num2 = 0;
                for (int j = 0; j < range.identities.Length; j++)
                {
                    Identity identity = range.identities[j] as Identity;
                    if (identity == null)
                    {
                        identity = new Identity(range.identities[j] as IDictionary);
                    }
                    if (range.updateType == 0)
                    {
                        this.AddIdentityToSequence(sequence, range.position + num2, identity, null);
                        num2++;
                    }
                    if (range.updateType == 1)
                    {
                        this.RemoveIdentityFromSequence(sequence, identity, range.position, null);
                    }
                }
            }
        }

        public Sequence CreateSequence(string clientId, IList result, IList parameters, DataServiceTransaction dataServiceTransaction)
        {
            Sequence sequence = null;
            Identity[] identityArray = new Identity[result.Count];
            lock (_objLock)
            {
                int num;
                Identity identity;
                ArrayList list;
                for (num = 0; num < identityArray.Length; num++)
                {
                    if (result[num] != null)
                    {
                        identity = Identity.GetIdentity(result[num], this._dataDestination);
                        identityArray[num] = identity;
                        if (!this._itemIdToItemHash.ContainsKey(identity))
                        {
                            this._itemIdToItemHash.Add(identity, new ItemWrapper(result[num]));
                        }
                        else
                        {
                            ItemWrapper wrapper = this._itemIdToItemHash[identity] as ItemWrapper;
                            wrapper.Instance = result[num];
                        }
                    }
                }
                if (parameters != null)
                {
                    if (this._parametersToSequenceIdHash.Contains(parameters))
                    {
                        sequence = this._parametersToSequenceIdHash[parameters] as Sequence;
                    }
                }
                else
                {
                    IDictionary dictionary = this._itemIdToSequenceIdMapHash[identityArray[0]] as IDictionary;
                    if (dictionary != null)
                    {
                        foreach (Sequence sequence2 in dictionary.Values)
                        {
                            if (sequence2.Parameters == null)
                            {
                                sequence = sequence2;
                                break;
                            }
                        }
                    }
                }
                if (sequence == null)
                {
                    sequence = new Sequence {
                        Id = sequence.GetHashCode()
                    };
                    object[] array = null;
                    if (parameters != null)
                    {
                        array = new object[parameters.Count];
                        parameters.CopyTo(array, 0);
                        sequence.Parameters = array;
                        this._parametersToSequenceIdHash[parameters] = sequence;
                    }
                    for (num = 0; num < identityArray.Length; num++)
                    {
                        identity = identityArray[num];
                        this.AddIdentityToSequence(sequence, identity, dataServiceTransaction);
                    }
                    this._sequenceIdToSequenceHash[sequence.Id] = sequence;
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("SequenceManager_CreateSeq", new object[] { sequence.Id, clientId }));
                    }
                }
                else
                {
                    for (num = 0; num < identityArray.Length; num++)
                    {
                        identity = identityArray[num];
                        Identity identity2 = null;
                        if (num < sequence.Count)
                        {
                            identity2 = sequence[num];
                        }
                        if (!identity.Equals(identity2) && !sequence.Contains(identity))
                        {
                            int num2 = this.AddIdentityToSequence(sequence, identity, dataServiceTransaction);
                        }
                    }
                }
                sequence.AddSubscriber(clientId);
                if (this._clientIdToSequenceHash.Contains(clientId))
                {
                    list = this._clientIdToSequenceHash[clientId] as ArrayList;
                }
                else
                {
                    list = new ArrayList();
                    this._clientIdToSequenceHash[clientId] = list;
                }
                if (!list.Contains(sequence))
                {
                    list.Add(sequence);
                }
            }
            return sequence;
        }

        private UpdateCollectionMessage CreateUpdateCollectionMessage(DataMessage dataMessage, Sequence sequence, Identity identity, int position, int updateMode)
        {
            UpdateCollectionMessage message = new UpdateCollectionMessage {
                collectionId = sequence.Parameters,
                destination = dataMessage.destination,
                replace = false,
                updateMode = updateMode,
                messageId = "srv:" + Guid.NewGuid().ToString("D") + ":0",
                correlationId = dataMessage.correlationId
            };
            UpdateCollectionRange range = new UpdateCollectionRange {
                identities = new object[] { identity },
                updateType = 0,
                position = position
            };
            object[] objArray = new object[] { range };
            message.body = objArray;
            return message;
        }

        internal void Dump(DumpContext dumpContext)
        {
            dumpContext.AppendLine("SequenceManager, Items count = " + this._itemIdToItemHash.Count.ToString() + ", Subscribers = " + this._clientIdToSequenceHash.Count.ToString());
            Sequence[] sequences = this.GetSequences();
            if (sequences.Length > 0)
            {
                dumpContext.AppendLine("Sequences");
                foreach (Sequence sequence in sequences)
                {
                    dumpContext.Indent();
                    sequence.Dump(dumpContext);
                    dumpContext.Unindent();
                }
            }
        }

        private ItemWrapper GetItem(Identity identity)
        {
            lock (_objLock)
            {
                return (this._itemIdToItemHash[identity] as ItemWrapper);
            }
        }

        public PagedMessage GetPage(DataMessage dataMessage)
        {
            int sequenceId = (int) dataMessage.headers["sequenceId"];
            Sequence sequence = this.GetSequence(sequenceId);
            if (sequence != null)
            {
                return this.GetPagedMessage(dataMessage, sequence);
            }
            DataServiceException exception = new DataServiceException(string.Format("Sequence {0} in destination {1} was not found", sequenceId, dataMessage.destination));
            throw exception;
        }

        public PagedMessage GetPagedMessage(DataMessage dataMessage, Sequence sequence)
        {
            int num = (int) dataMessage.headers["pageSize"];
            int num2 = 0;
            if (dataMessage.headers.ContainsKey("pageIndex"))
            {
                num2 = (int) dataMessage.headers["pageIndex"];
            }
            num2 = Math.Max(0, num2);
            int num3 = (int) Math.Ceiling((double) (((double) sequence.Size) / ((double) num)));
            int num4 = num2 * num;
            int num5 = Math.Min(num4 + num, sequence.Size);
            PagedMessage message = new PagedMessage {
                pageIndex = num2,
                pageCount = num3,
                sequenceSize = sequence.Size,
                sequenceId = sequence.Id
            };
            object[] objArray = new object[num5 - num4];
            lock (_objLock)
            {
                for (int i = num4; i < num5; i++)
                {
                    Identity key = sequence[i];
                    if (this._itemIdToItemHash.Contains(key))
                    {
                        objArray[i - num4] = (this._itemIdToItemHash[key] as ItemWrapper).Instance;
                    }
                }
            }
            message.body = objArray;
            message.destination = dataMessage.destination;
            message.dataMessage = dataMessage;
            return message;
        }

        public SequencedMessage GetPageItems(DataMessage dataMessage)
        {
            int sequenceId = (int) dataMessage.headers["sequenceId"];
            Sequence sequence = this.GetSequence(sequenceId);
            if (sequence != null)
            {
                IList list = dataMessage.headers["DSids"] as IList;
                SequencedMessage message = new SequencedMessage();
                object[] objArray = new object[list.Count];
                lock (_objLock)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        Identity identity = new Identity(list[i] as IDictionary);
                        ItemWrapper item = this.GetItem(identity);
                        objArray[i] = item.Instance;
                    }
                    message.destination = dataMessage.destination;
                    message.sequenceId = sequence.Id;
                    message.sequenceSize = sequence.Size;
                    message.sequenceProxies = null;
                    message.body = objArray;
                }
                return message;
            }
            DataServiceException exception = new DataServiceException(string.Format("Sequence {0} in destination {1} was not found", sequenceId, dataMessage.destination));
            throw exception;
        }

        public Sequence GetSequence(IList fillParameters)
        {
            lock (_objLock)
            {
                if (fillParameters != null)
                {
                    if (this._parametersToSequenceIdHash.Contains(fillParameters))
                    {
                        return (this._parametersToSequenceIdHash[fillParameters] as Sequence);
                    }
                }
                else
                {
                    IDictionary dictionary = this._itemIdToSequenceIdMapHash[new Identity(fillParameters)] as IDictionary;
                    if (dictionary != null)
                    {
                        foreach (Sequence sequence in dictionary.Values)
                        {
                            if (sequence.Parameters == null)
                            {
                                return sequence;
                            }
                        }
                    }
                }
                return null;
            }
        }

        private Sequence GetSequence(int sequenceId)
        {
            lock (_objLock)
            {
                return (this._sequenceIdToSequenceHash[sequenceId] as Sequence);
            }
        }

        public SequencedMessage GetSequencedMessage(DataMessage dataMessage, Sequence sequence)
        {
            if ((dataMessage.headers != null) && dataMessage.headers.ContainsKey("pageSize"))
            {
                return this.GetPagedMessage(dataMessage, sequence);
            }
            SequencedMessage message = new SequencedMessage {
                destination = dataMessage.destination,
                sequenceId = sequence.Id,
                sequenceSize = sequence.Size
            };
            object[] objArray = new object[sequence.Count];
            lock (_objLock)
            {
                for (int i = 0; i < sequence.Count; i++)
                {
                    ItemWrapper item = this.GetItem(sequence[i]);
                    if (item != null)
                    {
                        objArray[i] = item.Instance;
                    }
                }
            }
            message.body = objArray;
            message.sequenceProxies = null;
            message.dataMessage = dataMessage;
            message.messageId = dataMessage.messageId;
            message.clientId = dataMessage.clientId;
            message.correlationId = dataMessage.messageId;
            return message;
        }

        private Sequence[] GetSequences()
        {
            lock (_objLock)
            {
                ArrayList list = new ArrayList(this._sequenceIdToSequenceHash.Values);
                return (list.ToArray(typeof(Sequence)) as Sequence[]);
            }
        }

        public ICollection GetSequences(IList fillParameters)
        {
            lock (_objLock)
            {
                if (fillParameters != null)
                {
                    Sequence sequence = this._parametersToSequenceIdHash[fillParameters] as Sequence;
                    if (sequence != null)
                    {
                        ArrayList list = new ArrayList(1);
                        list.Add(sequence);
                        return list;
                    }
                }
                else
                {
                    return this._sequenceIdToSequenceHash.Values;
                }
            }
            return null;
        }

        public void ManageMessageBatch(MessageBatch messageBatch, DataServiceTransaction dataServiceTransaction)
        {
            int num;
            Sequence sequence;
            DataMessage dataMessage;
            SequencedMessage sequencedMessage;
            DataMessage incomingMessage = messageBatch.IncomingMessage;
            for (num = 0; num < messageBatch.Messages.Count; num++)
            {
                IMessage message2 = messageBatch.Messages[num] as IMessage;
                if (message2 is UpdateCollectionMessage)
                {
                    UpdateCollectionMessage updateCollectionMessage = message2 as UpdateCollectionMessage;
                    IList collectionId = updateCollectionMessage.collectionId;
                    sequence = this._dataDestination.SequenceManager.GetSequence(collectionId);
                    if (sequence != null)
                    {
                        this.ApplyUpdateCollectionMessage(sequence, updateCollectionMessage);
                    }
                }
            }
            for (num = 0; num < messageBatch.Messages.Count; num++)
            {
                dataMessage = messageBatch.Messages[num] as DataMessage;
                if ((dataMessage != null) && (dataMessage.operation == 11))
                {
                    IList result = new ArrayList();
                    result.Add(dataMessage.body);
                    sequence = this.CreateSequence(dataMessage.clientId as string, result, null, dataServiceTransaction);
                    sequencedMessage = this.GetSequencedMessage(dataMessage, sequence);
                    messageBatch.Messages[num] = sequencedMessage;
                }
            }
            for (num = 0; num < messageBatch.Messages.Count; num++)
            {
                if (messageBatch.Messages[num] is DataMessage)
                {
                    dataMessage = messageBatch.Messages[num] as DataMessage;
                    this.SyncSequenceChanges(dataMessage, dataServiceTransaction);
                }
                if (messageBatch.Messages[num] is SequencedMessage)
                {
                    sequencedMessage = messageBatch.Messages[num] as SequencedMessage;
                    dataMessage = sequencedMessage.dataMessage;
                    this.SyncSequenceChanges(dataMessage, dataServiceTransaction);
                }
            }
        }

        public AcknowledgeMessage ManageSequence(DataMessage dataMessage, IList items)
        {
            return this.ManageSequence(dataMessage, items, null);
        }

        public AcknowledgeMessage ManageSequence(DataMessage dataMessage, IList items, DataServiceTransaction dataServiceTransaction)
        {
            Sequence sequence;
            switch (dataMessage.operation)
            {
                case 1:
                    sequence = this.CreateSequence(dataMessage.clientId as string, items, dataMessage.body as IList, dataServiceTransaction);
                    return this.GetSequencedMessage(dataMessage, sequence);

                case 2:
                case 12:
                    sequence = this.CreateSequence(dataMessage.clientId as string, items, null, dataServiceTransaction);
                    return this.GetSequencedMessage(dataMessage, sequence);
            }
            if ((log != null) && log.get_IsErrorEnabled())
            {
                log.Error(__Res.GetString("SequenceManager_Unknown", new object[] { dataMessage.operation }));
            }
            return null;
        }

        public Sequence RefreshSequence(Sequence sequence, DataMessage dataMessage, object item, DataServiceTransaction dataServiceTransaction)
        {
            if (sequence.Parameters != null)
            {
                DotNetAdapter serviceAdapter = this._dataDestination.ServiceAdapter as DotNetAdapter;
                if (serviceAdapter != null)
                {
                    Identity identity;
                    bool isCreate = (dataMessage.operation == 0) || (dataMessage.operation == 11);
                    switch (serviceAdapter.RefreshFill(sequence.Parameters, item, isCreate))
                    {
                        case 0:
                            return sequence;

                        case 1:
                        {
                            IList parameters = sequence.Parameters;
                            DataMessage message = new DataMessage {
                                clientId = dataMessage.clientId,
                                operation = 1,
                                body = (parameters != null) ? ((object) parameters) : ((object) new object[0])
                            };
                            IList result = this._dataDestination.ServiceAdapter.Invoke(message) as IList;
                            return this.CreateSequence(dataMessage.clientId as string, result, parameters, dataServiceTransaction);
                        }
                        case 2:
                            identity = Identity.GetIdentity(item, this._dataDestination);
                            if (!sequence.Contains(identity))
                            {
                                this.AddIdentityToSequence(sequence, identity, dataServiceTransaction);
                            }
                            this._itemIdToItemHash[identity] = new ItemWrapper(item);
                            return sequence;

                        case 3:
                            identity = Identity.GetIdentity(item, this._dataDestination);
                            if (sequence.Contains(identity))
                            {
                                this.RemoveIdentityFromSequence(sequence, identity, dataServiceTransaction);
                            }
                            return sequence;
                    }
                }
            }
            return sequence;
        }

        public void ReleaseCollectionOperation(DataMessage dataMessage)
        {
            lock (_objLock)
            {
                int sequenceId = (int) dataMessage.headers["sequenceId"];
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    log.Debug(__Res.GetString("SequenceManager_ReleaseCollection", new object[] { sequenceId, dataMessage.clientId }));
                }
                Sequence sequence = this.GetSequence(sequenceId);
                IList body = dataMessage.body as IList;
                this.RemoveSubscriberFromSequence(dataMessage.clientId as string, sequence);
            }
        }

        public void ReleaseItemOperation(DataMessage dataMessage)
        {
            int sequenceId = (int) dataMessage.headers["sequenceId"];
            Sequence sequence = this.GetSequence(sequenceId);
        }

        public void RemoveIdentityFromSequence(Sequence sequence, Identity identity, DataServiceTransaction dataServiceTransaction)
        {
            this.RemoveIdentityFromSequence(sequence, identity, sequence.IndexOf(identity), dataServiceTransaction);
        }

        public void RemoveIdentityFromSequence(Sequence sequence, Identity identity, int position, DataServiceTransaction dataServiceTransaction)
        {
            if (position != -1)
            {
                lock (_objLock)
                {
                    IDictionary dictionary = this._itemIdToSequenceIdMapHash[identity] as IDictionary;
                    if (dictionary != null)
                    {
                        dictionary.Remove(sequence.Id);
                        if (dictionary.Count == 0)
                        {
                            this._itemIdToItemHash.Remove(identity);
                            this._itemIdToSequenceIdMapHash.Remove(identity);
                        }
                        if (sequence[position].Equals(identity))
                        {
                            sequence.RemoveAt(position);
                        }
                        else
                        {
                            sequence.Remove(identity);
                        }
                        if (dataServiceTransaction != null)
                        {
                            dataServiceTransaction.GenerateUpdateCollectionMessage(1, this._dataDestination, sequence, position, identity);
                        }
                    }
                    else
                    {
                        this._itemIdToItemHash.Remove(identity);
                        sequence.Remove(identity);
                    }
                }
            }
        }

        private void RemoveSequence(int sequenceId)
        {
            lock (_objLock)
            {
                Sequence sequence = this.GetSequence(sequenceId);
                if (sequence != null)
                {
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("SequenceManager_Remove", new object[] { sequence.Id }));
                    }
                    for (int i = sequence.Count - 1; i >= 0; i--)
                    {
                        Identity identity = sequence[i];
                        this.RemoveIdentityFromSequence(sequence, identity, i, null);
                    }
                    if (sequence.Parameters != null)
                    {
                        this._parametersToSequenceIdHash.Remove(sequence.Parameters);
                    }
                    this._sequenceIdToSequenceHash.Remove(sequenceId);
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("SequenceManager_RemoveStatus", new object[] { this._dataDestination.Id, this._sequenceIdToSequenceHash.Count }));
                    }
                }
            }
        }

        public void RemoveSubscriber(string clientId)
        {
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("SequenceManager_RemoveSubscriber", new object[] { clientId }));
            }
            lock (_objLock)
            {
                if (this._clientIdToSequenceHash.Contains(clientId))
                {
                    ArrayList list = this._clientIdToSequenceHash[clientId] as ArrayList;
                    for (int i = 0; i < list.Count; i++)
                    {
                        Sequence sequence = list[i] as Sequence;
                        sequence.RemoveSubscriber(clientId);
                        if (sequence.SubscriberCount == 0)
                        {
                            if (log.get_IsDebugEnabled())
                            {
                                log.Debug(__Res.GetString("SequenceManager_RemoveEmptySeq", new object[] { sequence.Id }));
                            }
                            this.RemoveSequence(sequence.Id);
                        }
                    }
                    this._clientIdToSequenceHash.Remove(clientId);
                }
            }
        }

        public void RemoveSubscriberFromSequence(string clientId, Sequence sequence)
        {
            if (sequence != null)
            {
                if (log.get_IsDebugEnabled())
                {
                    log.Debug(__Res.GetString("SequenceManager_RemoveSubscriberSeq", new object[] { clientId, sequence.Id }));
                }
                lock (_objLock)
                {
                    if (this._clientIdToSequenceHash.Contains(clientId))
                    {
                        ArrayList list = this._clientIdToSequenceHash[clientId] as ArrayList;
                        for (int i = 0; i < list.Count; i++)
                        {
                            Sequence sequence2 = list[i] as Sequence;
                            if (sequence == sequence2)
                            {
                                sequence.RemoveSubscriber(clientId);
                                if (sequence.SubscriberCount == 0)
                                {
                                    if (log.get_IsDebugEnabled())
                                    {
                                        log.Debug(__Res.GetString("SequenceManager_RemoveEmptySeq", new object[] { sequence.Id }));
                                    }
                                    this.RemoveSequence(sequence.Id);
                                }
                                list.RemoveAt(i);
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void SyncSequenceChanges(DataMessage dataMessage, DataServiceTransaction dataServiceTransaction)
        {
            lock (_objLock)
            {
                ArrayList list = new ArrayList(this._sequenceIdToSequenceHash.Values.Count);
                list.AddRange(this._sequenceIdToSequenceHash.Values);
                foreach (Sequence sequence in list)
                {
                    switch (dataMessage.operation)
                    {
                        case 0:
                        case 11:
                            this.RefreshSequence(sequence, dataMessage, dataMessage.body, dataServiceTransaction);
                            break;

                        case 3:
                            this.RefreshSequence(sequence, dataMessage, (dataMessage.body as IList)[2], dataServiceTransaction);
                            break;

                        case 4:
                        {
                            Identity identity = Identity.GetIdentity(dataMessage.body, this._dataDestination);
                            if (sequence.IndexOf(identity) != -1)
                            {
                                this.RemoveIdentityFromSequence(sequence, identity, dataServiceTransaction);
                            }
                            break;
                        }
                    }
                }
            }
        }

        public object SyncRoot
        {
            get
            {
                return _objLock;
            }
        }
    }
}

