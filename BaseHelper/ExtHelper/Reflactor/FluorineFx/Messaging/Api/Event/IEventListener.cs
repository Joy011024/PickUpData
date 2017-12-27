namespace FluorineFx.Messaging.Api.Event
{
    using System;

    public interface IEventListener
    {
        void NotifyEvent(IEvent evt);
    }
}

