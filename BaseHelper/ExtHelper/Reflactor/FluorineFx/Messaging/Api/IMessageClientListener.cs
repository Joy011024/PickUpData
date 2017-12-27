namespace FluorineFx.Messaging.Api
{
    using System;

    public interface IMessageClientListener
    {
        void MessageClientCreated(IMessageClient messageClient);
        void MessageClientDestroyed(IMessageClient messageClient);
    }
}

