namespace FluorineFx.Messaging.Rtmp.Event
{
    using FluorineFx.Messaging.Rtmp.Stream;
    using FluorineFx.Util;
    using System;

    internal class AudioData : BaseEvent, IStreamData
    {
        protected ByteBuffer _data;

        public AudioData() : this(ByteBuffer.Allocate(0))
        {
        }

        public AudioData(ByteBuffer data) : base(EventType.STREAM_DATA)
        {
            base._dataType = 8;
            this._data = data;
        }

        public AudioData(byte[] data) : this(ByteBuffer.Wrap(data))
        {
        }

        public override string ToString()
        {
            return ("Audio  ts: " + base.Header.Timer);
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

