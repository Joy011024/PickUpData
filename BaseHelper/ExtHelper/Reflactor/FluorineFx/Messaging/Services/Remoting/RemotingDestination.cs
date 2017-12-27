namespace FluorineFx.Messaging.Services.Remoting
{
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Services;
    using System;

    internal class RemotingDestination : Destination
    {
        public RemotingDestination(IService service, DestinationSettings destinationSettings) : base(service, destinationSettings)
        {
        }
    }
}

