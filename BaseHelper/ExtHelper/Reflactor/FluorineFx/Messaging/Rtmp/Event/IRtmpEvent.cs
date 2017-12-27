namespace FluorineFx.Messaging.Rtmp.Event
{
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Rtmp;
    using System;

    public interface IRtmpEvent : IEvent
    {
        byte DataType { get; }

        RtmpHeader Header { get; set; }

        int Timestamp { get; set; }
    }
}

