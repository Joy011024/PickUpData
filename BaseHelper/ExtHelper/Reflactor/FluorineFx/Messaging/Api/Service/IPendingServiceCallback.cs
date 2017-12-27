namespace FluorineFx.Messaging.Api.Service
{
    using System;

    public interface IPendingServiceCallback
    {
        void ResultReceived(IPendingServiceCall call);
    }
}

