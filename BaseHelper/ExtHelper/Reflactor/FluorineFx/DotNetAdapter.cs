namespace FluorineFx
{
    using FluorineFx.Data;
    using FluorineFx.Data.Assemblers;
    using FluorineFx.Data.Messages;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Services;
    using log4net;
    using System;
    using System.Collections;

    internal class DotNetAdapter : ServiceAdapter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DotNetAdapter));

        public bool AutoRefreshFill(IList parameters)
        {
            IAssembler assembler = this.GetAssembler();
            if (assembler != null)
            {
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    log.Debug(assembler.GetType().FullName + " AutoRefreshFill");
                }
                return assembler.AutoRefreshFill(parameters);
            }
            return false;
        }

        private IList Batch(DataMessage dataMessage)
        {
            ArrayList list = new ArrayList();
            IList body = dataMessage.body as IList;
            for (int i = 0; i < body.Count; i++)
            {
                IMessage message = body[i] as IMessage;
                try
                {
                    if (message is UpdateCollectionMessage)
                    {
                        list.Add(this.UpdateCollection(message as UpdateCollectionMessage, body));
                    }
                    else
                    {
                        object obj2 = this.Invoke(message);
                        list.Add(obj2);
                    }
                }
                catch (DataSyncException exception)
                {
                    DataErrorMessage errorMessage = exception.GetErrorMessage() as DataErrorMessage;
                    errorMessage.cause = message as DataMessage;
                    errorMessage.correlationId = message.messageId;
                    errorMessage.destination = message.destination;
                    list.Add(errorMessage);
                }
                catch (Exception exception2)
                {
                    ErrorMessage message3 = new MessageException(exception2).GetErrorMessage();
                    message3.correlationId = message.messageId;
                    message3.destination = message.destination;
                    list.Add(message3);
                }
            }
            return list;
        }

        private IMessage Create(DataMessage dataMessage)
        {
            IAssembler assembler = this.GetAssembler();
            if (assembler != null)
            {
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    log.Debug(assembler.GetType().FullName + " CreateItem");
                }
                assembler.CreateItem(dataMessage.body);
                Identity identity = Identity.GetIdentity(dataMessage.body, base.Destination as DataDestination);
                dataMessage.identity = identity;
            }
            return dataMessage;
        }

        private IMessage Delete(DataMessage dataMessage)
        {
            IAssembler assembler = this.GetAssembler();
            if (assembler != null)
            {
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    log.Debug(assembler.GetType().FullName + " Delete");
                }
                assembler.DeleteItem(dataMessage.body);
            }
            return dataMessage;
        }

        private IList Fill(DataMessage dataMessage)
        {
            IAssembler assembler = this.GetAssembler();
            if (assembler != null)
            {
                IList body = dataMessage.body as IList;
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    log.Debug(assembler.GetType().FullName + " Fill");
                }
                return assembler.Fill(body);
            }
            return null;
        }

        private IAssembler GetAssembler()
        {
            return (base.Destination.GetFactoryInstance().Lookup() as IAssembler);
        }

        private object GetItem(DataMessage dataMessage)
        {
            IAssembler assembler = this.GetAssembler();
            if (assembler != null)
            {
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    log.Debug(assembler.GetType().FullName + " GetItem");
                }
                return assembler.GetItem(dataMessage.identity);
            }
            return null;
        }

        private IMessage GetMessage(IList messages, string messageId)
        {
            foreach (IMessage message in messages)
            {
                if (message.messageId == messageId)
                {
                    return message;
                }
            }
            return null;
        }

        public override object Invoke(IMessage message)
        {
            DataMessage dataMessage = message as DataMessage;
            switch (dataMessage.operation)
            {
                case 0:
                case 11:
                    return this.Create(dataMessage);

                case 1:
                    return this.Fill(dataMessage);

                case 2:
                    return this.GetItem(dataMessage);

                case 3:
                    return this.Update(dataMessage);

                case 4:
                    return this.Delete(dataMessage);

                case 5:
                    return this.Batch(dataMessage);
            }
            if ((log != null) && log.get_IsErrorEnabled())
            {
                log.Error(__Res.GetString("DataService_Unknown", new object[] { dataMessage.operation }));
            }
            return null;
        }

        public int RefreshFill(IList fillParameters, object item, bool isCreate)
        {
            IAssembler assembler = this.GetAssembler();
            if (assembler != null)
            {
                if ((log != null) && log.get_IsDebugEnabled())
                {
                    log.Debug(assembler.GetType().FullName + " RefreshFill");
                }
                return assembler.RefreshFill(fillParameters, item, isCreate);
            }
            return 0;
        }

        private IMessage Update(DataMessage dataMessage)
        {
            IList body = dataMessage.body as IList;
            IList list2 = body[0] as IList;
            if ((list2 != null) && (list2.Count != 0))
            {
                IAssembler assembler = this.GetAssembler();
                if (assembler != null)
                {
                    if ((log != null) && log.get_IsDebugEnabled())
                    {
                        log.Debug(assembler.GetType().FullName + " Update");
                    }
                    assembler.UpdateItem(body[2], body[1], body[0] as IList);
                }
            }
            return dataMessage;
        }

        private IMessage UpdateCollection(UpdateCollectionMessage updateCollectionMessage, IList messages)
        {
            IList body = updateCollectionMessage.body as IList;
            for (int i = 0; i < body.Count; i++)
            {
                UpdateCollectionRange range = body[i] as UpdateCollectionRange;
                for (int j = 0; j < range.identities.Length; j++)
                {
                    string messageId = range.identities[j] as string;
                    Identity identity = null;
                    if (messageId != null)
                    {
                        DataMessage message = this.GetMessage(messages, messageId) as DataMessage;
                        if (message != null)
                        {
                            DataDestination destination = base.Destination as DataDestination;
                            identity = Identity.GetIdentity(message.body, destination);
                        }
                        range.identities[j] = identity;
                    }
                    else
                    {
                        IDictionary map = range.identities[j] as IDictionary;
                        if (map != null)
                        {
                            identity = new Identity(map);
                        }
                    }
                    IList collectionId = updateCollectionMessage.collectionId;
                    IAssembler assembler = this.GetAssembler();
                    if (assembler != null)
                    {
                        if (range.updateType == 0)
                        {
                            assembler.AddItemToFill(collectionId, range.position + j, identity);
                        }
                        if (range.updateType == 1)
                        {
                            assembler.RemoveItemFromFill(collectionId, range.position, identity);
                        }
                    }
                }
            }
            return updateCollectionMessage;
        }
    }
}

