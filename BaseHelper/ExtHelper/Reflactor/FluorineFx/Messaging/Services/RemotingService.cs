namespace FluorineFx.Messaging.Services
{
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Services.Remoting;
    using System;

    internal class RemotingService : ServiceBase
    {
        public const string RemotingServiceId = "remoting-service";

        public RemotingService(MessageBroker broker, ServiceSettings settings) : base(broker, settings)
        {
        }

        protected override Destination NewDestination(DestinationSettings destinationSettings)
        {
            return new RemotingDestination(this, destinationSettings);
        }

        public override object ServiceMessage(IMessage message)
        {
            RemotingMessage message2 = message as RemotingMessage;
            RemotingDestination destination = base.GetDestination(message) as RemotingDestination;
            return destination.ServiceAdapter.Invoke(message);
        }
    }
}

