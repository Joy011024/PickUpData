namespace FluorineFx.Messaging.Rtmp.SO
{
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Rtmp.Event;
    using System;
    using System.Collections.Generic;

    internal interface ISharedObjectMessage : IRtmpEvent, IEvent
    {
        void AddEvent(ISharedObjectEvent sharedObjectEvent);
        void AddEvent(SharedObjectEventType type, string key, object value);
        void Clear();

        IList<ISharedObjectEvent> Events { get; }

        bool IsEmpty { get; }

        bool IsPersistent { get; }

        string Name { get; }

        int Version { get; }
    }
}

