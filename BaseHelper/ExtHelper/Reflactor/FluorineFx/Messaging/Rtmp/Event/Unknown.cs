namespace FluorineFx.Messaging.Rtmp.Event
{
    using FluorineFx.Util;
    using System;

    internal class Unknown : BaseEvent
    {
        protected ByteBuffer _data;

        public Unknown(ByteBuffer data) : base(EventType.SYSTEM)
        {
            base._dataType = 0;
            this._data = data;
        }

        public Unknown(byte dataType, ByteBuffer data) : base(EventType.SYSTEM)
        {
            base._dataType = dataType;
            this._data = data;
        }

        public Unknown(byte dataType, byte[] data) : this(dataType, ByteBuffer.Wrap(data))
        {
        }

        public ByteBuffer Data
        {
            get
            {
                return this._data;
            }
        }
    }
}

