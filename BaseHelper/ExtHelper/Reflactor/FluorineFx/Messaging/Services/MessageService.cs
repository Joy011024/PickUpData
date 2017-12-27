namespace FluorineFx.Messaging.Services
{
    using FluorineFx;
    using FluorineFx.Context;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Services.Messaging;
    using log4net;
    using System;
    using System.Collections;

    [CLSCompliant(false)]
    public class MessageService : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MessageService));

        private MessageService()
        {
        }

        public MessageService(MessageBroker messageBroker, ServiceSettings serviceSettings) : base(messageBroker, serviceSettings)
        {
        }

        public ICollection GetSubscriber(IMessage message)
        {
            return this.GetSubscriber(message, true);
        }

        public ICollection GetSubscriber(IMessage message, bool evalSelector)
        {
            MessageDestination destination = base.GetDestination(message) as MessageDestination;
            return destination.SubscriptionManager.GetSubscribers(message, evalSelector);
        }

        [CLSCompliant(false)]
        protected override Destination NewDestination(DestinationSettings destinationSettings)
        {
            return new MessageDestination(this, destinationSettings);
        }

        public void PushMessageToClients(IMessage message)
        {
            MessageDestination destination = base.GetDestination(message) as MessageDestination;
            ICollection subscribers = destination.SubscriptionManager.GetSubscribers(message);
            if ((subscribers != null) && (subscribers.Count > 0))
            {
                this.PushMessageToClients(subscribers, message);
            }
        }

        public void PushMessageToClients(ICollection subscribers, IMessage message)
        {
            MessageDestination destination = base.GetDestination(message) as MessageDestination;
            SubscriptionManager subscriptionManager = destination.SubscriptionManager;
            if ((subscribers != null) && (subscribers.Count > 0))
            {
                IMessage message2 = message.Clone() as IMessage;
                foreach (string str in subscribers)
                {
                    MessageClient subscriber = subscriptionManager.GetSubscriber(str);
                    if (subscriber != null)
                    {
                        if (log.get_IsDebugEnabled())
                        {
                            if (message2 is BinaryMessage)
                            {
                                log.Debug(__Res.GetString("MessageServicePushBinary", new object[] { message.GetType().Name, str }));
                            }
                            else
                            {
                                log.Debug(__Res.GetString("MessageServicePush", new object[] { message.GetType().Name, str }));
                            }
                        }
                        base._messageBroker.GetEndpoint(subscriber.Endpoint).Push(message2, subscriber);
                    }
                }
            }
        }

        public override object ServiceMessage(IMessage message)
        {
            CommandMessage commandMessage = message as CommandMessage;
            MessageDestination messageDestination = base.GetDestination(message) as MessageDestination;
            if (commandMessage == null)
            {
                if (log.get_IsDebugEnabled())
                {
                    log.Debug(__Res.GetString("MessageServiceRoute", new object[] { messageDestination.Id, message.clientId }));
                }
                if ((FluorineContext.Current != null) && (FluorineContext.Current.Client != null))
                {
                    FluorineContext.Current.Client.Renew();
                }
                return messageDestination.ServiceAdapter.Invoke(message);
            }
            string clientId = commandMessage.clientId as string;
            MessageClient subscriber = messageDestination.SubscriptionManager.GetSubscriber(clientId);
            switch (commandMessage.operation)
            {
                case 0:
                    if (subscriber == null)
                    {
                        if (clientId == null)
                        {
                            clientId = Guid.NewGuid().ToString("D");
                        }
                        if (log.get_IsDebugEnabled())
                        {
                            log.Debug(__Res.GetString("MessageServiceSubscribe", new object[] { messageDestination.Id, clientId }));
                        }
                        string header = commandMessage.GetHeader("DSEndpoint") as string;
                        commandMessage.clientId = clientId;
                        if ((messageDestination.ServiceAdapter != null) && messageDestination.ServiceAdapter.HandlesSubscriptions)
                        {
                            messageDestination.ServiceAdapter.Manage(commandMessage);
                        }
                        Subtopic subtopic = null;
                        Selector selector = null;
                        if (commandMessage.headers != null)
                        {
                            if (commandMessage.headers.ContainsKey(CommandMessage.SelectorHeader))
                            {
                                selector = Selector.CreateSelector(commandMessage.headers[CommandMessage.SelectorHeader] as string);
                            }
                            if (commandMessage.headers.ContainsKey("DSSubtopic"))
                            {
                                subtopic = new Subtopic(commandMessage.headers["DSSubtopic"] as string);
                            }
                        }
                        IClient client = FluorineContext.Current.Client;
                        client.Renew();
                        subscriber = messageDestination.SubscriptionManager.AddSubscriber(client, clientId, header, messageDestination, subtopic, selector);
                    }
                    return new AcknowledgeMessage { clientId = clientId };

                case 1:
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("MessageServiceUnsubscribe", new object[] { messageDestination.Id, clientId }));
                    }
                    if ((messageDestination.ServiceAdapter != null) && messageDestination.ServiceAdapter.HandlesSubscriptions)
                    {
                        messageDestination.ServiceAdapter.Manage(commandMessage);
                    }
                    if (subscriber != null)
                    {
                        subscriber.Unsubscribe();
                    }
                    return new AcknowledgeMessage();

                case 2:
                    if (subscriber == null)
                    {
                        ServiceException exception = new ServiceException(string.Format("MessageClient is not subscribed to {0}", commandMessage.destination)) {
                            FaultCode = "Server.Processing.NotSubscribed"
                        };
                        throw exception;
                    }
                    FluorineContext.Current.Client.Renew();
                    messageDestination.ServiceAdapter.Manage(commandMessage);
                    return new AcknowledgeMessage();

                case 5:
                    if ((messageDestination.ServiceAdapter != null) && messageDestination.ServiceAdapter.HandlesSubscriptions)
                    {
                        messageDestination.ServiceAdapter.Manage(commandMessage);
                    }
                    return true;
            }
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("MessageServiceUnknown", new object[] { commandMessage.operation, messageDestination.Id }));
            }
            messageDestination.ServiceAdapter.Manage(commandMessage);
            return new AcknowledgeMessage();
        }
    }
}

