namespace FluorineFx.Messaging.Api.Service
{
    using System;

    public interface IPendingServiceCall : IServiceCall
    {
        IPendingServiceCallback[] GetCallbacks();
        void RegisterCallback(IPendingServiceCallback callback);
        void UnregisterCallback(IPendingServiceCallback callback);

        object Result { get; set; }
    }
}

