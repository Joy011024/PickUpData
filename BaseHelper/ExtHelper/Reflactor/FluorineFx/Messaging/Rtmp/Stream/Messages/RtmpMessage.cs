namespace FluorineFx.Messaging.Rtmp.Stream.Messages
{
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmp.Event;
    using System;

    internal class RtmpMessage : AsyncMessage
    {
        public IRtmpEvent body
        {
            get
            {
                return (base._body as IRtmpEvent);
            }
            set
            {
                base._body = value;
            }
        }
    }
}

