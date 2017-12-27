namespace FluorineFx.Messaging.Rtmp.Event
{
    using FluorineFx.Util;
    using System;

    internal class FlexStreamSend : Notify
    {
        public FlexStreamSend(ByteBuffer data) : base(data)
        {
            base._dataType = 15;
        }
    }
}

