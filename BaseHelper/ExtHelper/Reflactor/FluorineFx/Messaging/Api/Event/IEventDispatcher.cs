namespace FluorineFx.Messaging.Api.Event
{
    using System;

    public interface IEventDispatcher
    {
        void DispatchEvent(IEvent evt);
    }
}

