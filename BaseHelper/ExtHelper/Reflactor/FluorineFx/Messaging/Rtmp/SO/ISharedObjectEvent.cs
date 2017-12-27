namespace FluorineFx.Messaging.Rtmp.SO
{
    using System;

    public interface ISharedObjectEvent
    {
        string Key { get; }

        SharedObjectEventType Type { get; }

        object Value { get; }
    }
}

