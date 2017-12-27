namespace FluorineFx.Messaging.Rtmp.Event
{
    using System;

    [CLSCompliant(false)]
    public sealed class BytesRead : BaseEvent
    {
        private int _bytesRead;

        internal BytesRead(int bytesRead) : base(EventType.STREAM_CONTROL)
        {
            this._bytesRead = 0;
            base._dataType = 3;
            this._bytesRead = bytesRead;
        }

        public int Bytes
        {
            get
            {
                return this._bytesRead;
            }
        }
    }
}

