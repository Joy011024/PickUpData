namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Rtmp.Stream.Messages;
    using System;

    internal interface IFrameDropper
    {
        bool CanSendPacket(RtmpMessage message, long pending);
        void DropPacket(RtmpMessage message);
        void Reset();
        void Reset(FrameDropperState state);
        void SendPacket(RtmpMessage message);
    }
}

