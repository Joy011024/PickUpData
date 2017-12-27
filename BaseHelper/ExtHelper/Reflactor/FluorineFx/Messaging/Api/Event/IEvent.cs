namespace FluorineFx.Messaging.Api.Event
{
    using System;

    public interface IEvent
    {
        FluorineFx.Messaging.Api.Event.EventType EventType { get; }

        bool HasSource { get; }

        object Object { get; }

        IEventListener Source { get; set; }
    }
}

