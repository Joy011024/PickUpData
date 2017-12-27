namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx.Messaging.Rtmp.Event;
    using FluorineFx.Util;
    using System;

    [CLSCompliant(false)]
    public sealed class RtmpPacket
    {
        private ByteBuffer _data;
        private RtmpHeader _header;
        private IRtmpEvent _message;

        internal RtmpPacket(RtmpHeader header)
        {
            this._header = header;
            this._data = ByteBuffer.Allocate(header.Size + ((header.Timer == 0xffffff) ? 4 : 0));
        }

        internal RtmpPacket(RtmpHeader header, IRtmpEvent message)
        {
            this._header = header;
            this._message = message;
        }

        public ByteBuffer Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }

        public RtmpHeader Header
        {
            get
            {
                return this._header;
            }
            set
            {
                this._header = value;
            }
        }

        public IRtmpEvent Message
        {
            get
            {
                return this._message;
            }
            set
            {
                this._message = value;
            }
        }
    }
}

