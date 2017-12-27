namespace FluorineFx.Messaging.Api.Event
{
    using System;
    using System.Collections;

    public interface IEventObservable
    {
        void AddEventListener(IEventListener listener);
        ICollection GetEventListeners();
        void RemoveEventListener(IEventListener listener);
    }
}

