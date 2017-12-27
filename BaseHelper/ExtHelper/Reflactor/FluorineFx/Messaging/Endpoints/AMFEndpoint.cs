namespace FluorineFx.Messaging.Endpoints
{
    using FluorineFx.Configuration;
    using FluorineFx.Context;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Endpoints.Filter;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Util;
    using System;
    using System.Web;

    internal class AMFEndpoint : EndpointBase
    {
        private FilterChain _filterChain;
        private AtomicInteger _waitingPollRequests;
        private const string LegacyCollectionKey = "legacy-collection";

        public AMFEndpoint(MessageBroker messageBroker, ChannelSettings channelSettings) : base(messageBroker, channelSettings)
        {
            this._waitingPollRequests = new AtomicInteger();
        }

        public bool GetIsLegacyCollection()
        {
            if (!base._channelSettings.Contains("legacy-collection"))
            {
                return false;
            }
            string str = base._channelSettings["legacy-collection"] as string;
            return Convert.ToBoolean(str);
        }

        public override void Push(IMessage message, MessageClient messageClient)
        {
            if ((base._channelSettings != null) && base._channelSettings.IsPollingEnabled)
            {
                IMessage message2 = message.Clone() as IMessage;
                message2.SetHeader("DSDstClientId", messageClient.ClientId);
                message2.clientId = messageClient.ClientId;
                messageClient.AddMessage(message2);
            }
        }

        public override void Service()
        {
            AMFContext context = new AMFContext(HttpContext.Current.Request.InputStream, HttpContext.Current.Response.OutputStream);
            AMFContext.Current = context;
            this._filterChain.InvokeFilters(context);
        }

        public override IMessage ServiceMessage(IMessage message)
        {
            if (message is CommandMessage)
            {
                CommandMessage message2 = message as CommandMessage;
                switch (message2.operation)
                {
                    case 0:
                    {
                        if (FluorineContext.Current.Client == null)
                        {
                            FluorineContext.Current.SetCurrentClient(base.GetMessageBroker().ClientRegistry.GetClient(message));
                        }
                        RemotingConnection connection = null;
                        foreach (IConnection connection2 in FluorineContext.Current.Client.Connections)
                        {
                            if (connection2 is RemotingConnection)
                            {
                                connection = connection2 as RemotingConnection;
                                break;
                            }
                        }
                        if (connection == null)
                        {
                            connection = new RemotingConnection(this, null, FluorineContext.Current.Client.Id, null);
                            FluorineContext.Current.Client.Renew(connection.ClientLeaseTime);
                            connection.Initialize(FluorineContext.Current.Client);
                        }
                        break;
                    }
                    case 2:
                    {
                        if (FluorineContext.Current.Client != null)
                        {
                            FluorineContext.Current.Client.Renew();
                        }
                        IMessage[] pendingMessages = null;
                        this._waitingPollRequests.Increment();
                        int waitIntervalMillis = (base._channelSettings.WaitIntervalMillis != -1) ? base._channelSettings.WaitIntervalMillis : 0xea60;
                        if (FluorineContext.Current.Client != null)
                        {
                            FluorineContext.Current.Client.Renew();
                        }
                        if (message2.HeaderExists(CommandMessage.FluorineSuppressPollWaitHeader))
                        {
                            waitIntervalMillis = 0;
                        }
                        if (!FluorineConfiguration.Instance.FluorineSettings.Runtime.AsyncHandler)
                        {
                            waitIntervalMillis = 0;
                        }
                        if ((base._channelSettings.MaxWaitingPollRequests <= 0) || (this._waitingPollRequests.Value >= base._channelSettings.MaxWaitingPollRequests))
                        {
                            waitIntervalMillis = 0;
                        }
                        if ((message.destination != null) && (message.destination != string.Empty))
                        {
                            string clientId = message2.clientId as string;
                            MessageDestination destination = base.GetMessageBroker().GetDestination(message.destination) as MessageDestination;
                            MessageClient subscriber = destination.SubscriptionManager.GetSubscriber(clientId);
                            subscriber.Renew();
                            pendingMessages = subscriber.GetPendingMessages();
                        }
                        else if (FluorineContext.Current.Client != null)
                        {
                            pendingMessages = FluorineContext.Current.Client.GetPendingMessages(waitIntervalMillis);
                        }
                        this._waitingPollRequests.Decrement();
                        if ((pendingMessages == null) || (pendingMessages.Length == 0))
                        {
                            return new AcknowledgeMessage();
                        }
                        return new CommandMessage { operation = 4, body = pendingMessages };
                    }
                }
            }
            return base.ServiceMessage(message);
        }

        public override void Start()
        {
            DeserializationFilter filter = new DeserializationFilter {
                UseLegacyCollection = this.GetIsLegacyCollection()
            };
            ServiceMapFilter filter2 = new ServiceMapFilter(this);
            WsdlFilter filter3 = new WsdlFilter();
            AuthenticationFilter filter4 = new AuthenticationFilter(this);
            DescribeServiceFilter filter5 = new DescribeServiceFilter();
            ProcessFilter filter6 = new ProcessFilter(this);
            MessageFilter filter7 = new MessageFilter(this);
            DebugFilter filter8 = new DebugFilter();
            SerializationFilter filter9 = new SerializationFilter {
                UseLegacyCollection = this.GetIsLegacyCollection()
            };
            filter.Next = filter2;
            filter2.Next = filter3;
            filter3.Next = filter4;
            filter4.Next = filter5;
            filter5.Next = filter6;
            filter6.Next = filter8;
            filter8.Next = filter7;
            filter7.Next = filter9;
            this._filterChain = new FilterChain(filter);
        }

        public override void Stop()
        {
            this._filterChain = null;
            base.Stop();
        }
    }
}

