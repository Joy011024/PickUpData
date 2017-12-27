namespace FluorineFx.Messaging.Api.Event
{
    using System;

    public interface IEventHandler
    {
        bool HandleEvent(IEvent evt);
    }
}

