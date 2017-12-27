namespace FluorineFx.Messaging
{
    using FluorineFx;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Services;
    using FluorineFx.Messaging.Services.Messaging;
    using log4net;
    using System;

    internal class MessageDestination : Destination
    {
        private FluorineFx.Messaging.Services.Messaging.SubscriptionManager _subscriptionManager;
        private static readonly ILog log = LogManager.GetLogger(typeof(MessageDestination));

        public MessageDestination(IService service, DestinationSettings destinationSettings) : base(service, destinationSettings)
        {
            this._subscriptionManager = new FluorineFx.Messaging.Services.Messaging.SubscriptionManager(this);
        }

        public virtual MessageClient RemoveSubscriber(string clientId)
        {
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("MessageDestination_RemoveSubscriber", new object[] { clientId }));
            }
            return this._subscriptionManager.RemoveSubscriber(clientId);
        }

        public FluorineFx.Messaging.Services.Messaging.SubscriptionManager SubscriptionManager
        {
            get
            {
                return this._subscriptionManager;
            }
        }
    }
}

