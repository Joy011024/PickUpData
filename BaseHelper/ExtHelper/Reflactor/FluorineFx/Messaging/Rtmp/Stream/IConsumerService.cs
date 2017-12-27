namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Messaging;
    using FluorineFx.Messaging.Api.Stream;
    using System;

    [CLSCompliant(false)]
    public interface IConsumerService : IScopeService, IService
    {
        IMessageOutput GetConsumerOutput(IClientStream stream);
    }
}

