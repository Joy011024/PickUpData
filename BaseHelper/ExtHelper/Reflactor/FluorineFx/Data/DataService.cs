namespace FluorineFx.Data
{
    using FluorineFx;
    using FluorineFx.Data.Messages;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Services;
    using log4net;
    using System;
    using System.Collections;

    internal class DataService : MessageService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DataService));

        public DataService(MessageBroker messageBroker, ServiceSettings serviceSettings) : base(messageBroker, serviceSettings)
        {
        }

        private void Dump()
        {
        }

        private AcknowledgeMessage ExecuteFillOperation(IMessage message)
        {
            DataMessage message2 = message as DataMessage;
            DataDestination destination = base.GetDestination(message2) as DataDestination;
            IList items = destination.ServiceAdapter.Invoke(message) as IList;
            return destination.SequenceManager.ManageSequence(message2, items);
        }

        private AcknowledgeMessage ExecuteGetOperation(IMessage message)
        {
            DataMessage message2 = message as DataMessage;
            DataDestination destination = base.GetDestination(message2) as DataDestination;
            object obj2 = destination.ServiceAdapter.Invoke(message);
            if (obj2 == null)
            {
                return new AcknowledgeMessage();
            }
            ArrayList items = new ArrayList(1);
            items.Add(obj2);
            return destination.SequenceManager.ManageSequence(message2, items);
        }

        private AcknowledgeMessage ExecuteGetSequenceIdOperation(IMessage message)
        {
            DataMessage message2 = message as DataMessage;
            AcknowledgeMessage message3 = null;
            DataDestination destination = base.GetDestination(message2) as DataDestination;
            if ((message2.body != null) && (message2.body is IList))
            {
                message2.operation = 1;
                IList items = destination.ServiceAdapter.Invoke(message) as IList;
                message3 = destination.SequenceManager.ManageSequence(message2, items);
                message2.operation = 12;
                return message3;
            }
            message2.operation = 2;
            object obj2 = destination.ServiceAdapter.Invoke(message);
            if (obj2 != null)
            {
                ArrayList list2 = new ArrayList(1);
                list2.Add(obj2);
                message3 = destination.SequenceManager.ManageSequence(message2, list2);
            }
            else
            {
                SequencedMessage message4 = new SequencedMessage {
                    sequenceId = -1
                };
                message3 = message4;
            }
            message2.operation = 12;
            return message3;
        }

        private AcknowledgeMessage ExecuteMultiBatchOperation(IMessage message)
        {
            AcknowledgeMessage message2 = null;
            MessageBatch batch;
            DataMessage message3 = message as DataMessage;
            IList body = message3.body as IList;
            DataServiceTransaction transaction = DataServiceTransaction.Begin(this);
            transaction.ClientId = message.clientId as string;
            transaction.CorrelationId = message.messageId;
            string str = null;
            ArrayList messages = new ArrayList();
            for (int i = 0; i < body.Count; i++)
            {
                DataMessage message4 = body[i] as DataMessage;
                string str2 = message4.destination;
                DataDestination destination = base.GetDestination(message4) as DataDestination;
                if (((str != null) && (str2 != str)) && (messages.Count > 0))
                {
                    batch = this.ServiceBatch(message, messages);
                    transaction.AddProcessedMessageBatch(batch);
                    messages = new ArrayList();
                }
                messages.Add(message4);
                str = str2;
                if (message4 is UpdateCollectionMessage)
                {
                    transaction.AddClientUpdateCollection(message4 as UpdateCollectionMessage);
                }
            }
            if (messages.Count > 0)
            {
                batch = this.ServiceBatch(message, messages);
                transaction.AddProcessedMessageBatch(batch);
            }
            transaction.Commit();
            IList outgoingMessages = transaction.GetOutgoingMessages();
            message2 = new AcknowledgeMessage();
            object[] array = new object[outgoingMessages.Count];
            outgoingMessages.CopyTo(array, 0);
            message2.body = array;
            return message2;
        }

        private AcknowledgeMessage ExecutePageItemsOperation(IMessage message)
        {
            DataMessage message2 = message as DataMessage;
            DataDestination destination = base.GetDestination(message2) as DataDestination;
            return destination.SequenceManager.GetPageItems(message2);
        }

        private AcknowledgeMessage ExecutePageOperation(IMessage message)
        {
            DataMessage message2 = message as DataMessage;
            DataDestination destination = base.GetDestination(message2) as DataDestination;
            return destination.SequenceManager.GetPage(message2);
        }

        private AcknowledgeMessage ExecuteReleaseCollectionOperation(IMessage message)
        {
            AcknowledgeMessage message2 = new AcknowledgeMessage();
            DataMessage message3 = message as DataMessage;
            DataDestination destination = base.GetDestination(message3) as DataDestination;
            destination.SequenceManager.ReleaseCollectionOperation(message3);
            return message2;
        }

        private AcknowledgeMessage ExecuteReleaseItemOperation(IMessage message)
        {
            AcknowledgeMessage message2 = new AcknowledgeMessage();
            DataMessage message3 = message as DataMessage;
            DataDestination destination = base.GetDestination(message3) as DataDestination;
            destination.SequenceManager.ReleaseItemOperation(message3);
            return message2;
        }

        protected override Destination NewDestination(DestinationSettings destinationSettings)
        {
            return new DataDestination(this, destinationSettings);
        }

        private MessageBatch ServiceBatch(IMessage message, ArrayList messages)
        {
            DataMessage message2 = messages[0] as DataMessage;
            DataMessage incomingMessage = null;
            if ((messages.Count == 1) && (message2.operation == 5))
            {
                incomingMessage = message2;
            }
            else
            {
                incomingMessage = new DataMessage {
                    destination = message2.destination,
                    operation = 5,
                    body = messages,
                    headers = message.headers,
                    clientId = message.clientId
                };
            }
            DataDestination destination = base.GetDestination(message2) as DataDestination;
            return new MessageBatch(incomingMessage, destination.ServiceAdapter.Invoke(incomingMessage) as IList);
        }

        public override object ServiceMessage(IMessage message)
        {
            CommandMessage message2 = message as CommandMessage;
            if (message2 != null)
            {
                return base.ServiceMessage(message2);
            }
            AsyncMessage message3 = null;
            DataMessage message4 = message as DataMessage;
            DataDestination destination = base.GetDestination(message4) as DataDestination;
            if (destination.SubscriptionManager.GetSubscriber(message4.clientId as string) == null)
            {
                CommandMessage message5 = new CommandMessage {
                    destination = destination.Id,
                    operation = 0,
                    clientId = message4.clientId as string
                };
                string header = message4.GetHeader("DSEndpoint") as string;
                message5.headers["DSEndpoint"] = header;
                string str2 = message4.GetHeader("DSId") as string;
                if (str2 != null)
                {
                    message5.headers["DSId"] = str2;
                }
                base.GetMessageBroker().GetEndpoint(header).ServiceMessage(message5);
            }
            switch (message4.operation)
            {
                case 1:
                    message3 = this.ExecuteFillOperation(message);
                    break;

                case 2:
                    message3 = this.ExecuteGetOperation(message);
                    break;

                case 5:
                case 6:
                case 7:
                    message3 = this.ExecuteMultiBatchOperation(message);
                    break;

                case 8:
                    message3 = this.ExecutePageOperation(message);
                    break;

                case 12:
                    message3 = this.ExecuteGetSequenceIdOperation(message);
                    break;

                case 0x12:
                    message3 = this.ExecuteReleaseCollectionOperation(message);
                    break;

                case 0x13:
                    message3 = this.ExecuteReleaseItemOperation(message);
                    break;

                case 20:
                    message3 = this.ExecutePageItemsOperation(message);
                    break;

                default:
                    if (log.get_IsErrorEnabled())
                    {
                        log.Error(__Res.GetString("DataService_Unknown", new object[] { message4.operation }));
                    }
                    message3 = new AcknowledgeMessage();
                    break;
            }
            message3.clientId = message.clientId;
            message3.correlationId = message.messageId;
            return message3;
        }
    }
}

