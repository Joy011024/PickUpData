namespace FluorineFx.Messaging.Rtmp
{
    using System;

    internal interface IRtmpHandler
    {
        void ConnectionClosed(RtmpConnection connection);
        void ConnectionOpened(RtmpConnection connection);
        void MessageReceived(RtmpConnection connection, object message);
        void MessageSent(RtmpConnection connection, object message);
    }
}

