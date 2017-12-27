namespace FluorineFx.Messaging.Rtmp.Stream
{
    using System;

    internal enum FrameDropperState
    {
        SEND_ALL,
        SEND_INTERFRAMES,
        SEND_KEYFRAMES,
        SEND_KEYFRAMES_CHECK
    }
}

