namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx;
    using System;
    using System.Collections.Generic;

    [CLSCompliant(false)]
    public sealed class RtmpContext
    {
        private long _decoderBufferAmount = 0L;
        private DecoderState _decoderState = DecoderState.Ok;
        private int _lastReadChannel = 0;
        private int _lastWriteChannel = 0;
        private RtmpMode _mode;
        private FluorineFx.ObjectEncoding _objectEncoding;
        private int _readChunkSize = 0x80;
        private Dictionary<int, RtmpHeader> _readHeaders = new Dictionary<int, RtmpHeader>();
        private Dictionary<int, RtmpPacket> _readPackets = new Dictionary<int, RtmpPacket>();
        private RtmpState _state;
        private int _writeChunkSize = 0x80;
        private Dictionary<int, RtmpHeader> _writeHeaders = new Dictionary<int, RtmpHeader>();
        private Dictionary<int, RtmpPacket> _writePackets = new Dictionary<int, RtmpPacket>();
        private const int DefaultChunkSize = 0x80;

        internal RtmpContext(RtmpMode mode)
        {
            this._mode = mode;
            this._objectEncoding = FluorineFx.ObjectEncoding.AMF0;
        }

        public bool CanStartDecoding(long remaining)
        {
            return (remaining >= this._decoderBufferAmount);
        }

        public void ContinueDecoding()
        {
            this._decoderState = DecoderState.Continue;
        }

        private void FreePackets(Dictionary<int, RtmpPacket> packets)
        {
            foreach (RtmpPacket packet in packets.Values)
            {
                if ((packet != null) && (packet.Data != null))
                {
                    packet.Data = null;
                }
            }
            packets.Clear();
        }

        public long GetDecoderBufferAmount()
        {
            return this._decoderBufferAmount;
        }

        public int GetLastReadChannel()
        {
            return this._lastReadChannel;
        }

        public RtmpHeader GetLastReadHeader(int channelId)
        {
            if (this._readHeaders.ContainsKey(channelId))
            {
                return this._readHeaders[channelId];
            }
            return null;
        }

        public RtmpPacket GetLastReadPacket(int channelId)
        {
            if (this._readPackets.ContainsKey(channelId))
            {
                return this._readPackets[channelId];
            }
            return null;
        }

        public int GetLastWriteChannel()
        {
            return this._lastWriteChannel;
        }

        public RtmpHeader GetLastWriteHeader(int channelId)
        {
            if (this._writeHeaders.ContainsKey(channelId))
            {
                return this._writeHeaders[channelId];
            }
            return null;
        }

        public RtmpPacket GetLastWritePacket(int channelId)
        {
            if (this._writePackets.ContainsKey(channelId))
            {
                return this._writePackets[channelId];
            }
            return null;
        }

        public int GetReadChunkSize()
        {
            return this._readChunkSize;
        }

        public int GetWriteChunkSize()
        {
            return this._writeChunkSize;
        }

        public void SetBufferDecoding(long amount)
        {
            this._decoderState = DecoderState.Buffer;
            this._decoderBufferAmount = amount;
        }

        public void SetLastReadHeader(int channelId, RtmpHeader header)
        {
            this._lastReadChannel = channelId;
            this._readHeaders[channelId] = header;
        }

        public void SetLastReadPacket(int channelId, RtmpPacket packet)
        {
            RtmpPacket packet2 = null;
            if (this._readPackets.ContainsKey(channelId))
            {
                packet2 = this._readPackets[channelId];
            }
            if ((packet2 != null) && (packet2.Data != null))
            {
                packet2.Data = null;
            }
            this._readPackets[channelId] = packet;
        }

        public void SetLastWriteHeader(int channelId, RtmpHeader header)
        {
            this._lastWriteChannel = channelId;
            this._writeHeaders[channelId] = header;
        }

        public void SetLastWritePacket(int channelId, RtmpPacket packet)
        {
            RtmpPacket packet2 = null;
            if (this._writePackets.ContainsKey(channelId))
            {
                packet2 = this._writePackets[channelId];
            }
            if ((packet2 != null) && (packet2.Data != null))
            {
                packet2.Data = null;
            }
            this._writePackets[channelId] = packet;
        }

        internal void SetMode(RtmpMode value)
        {
            this._mode = value;
        }

        public void SetReadChunkSize(int readChunkSize)
        {
            this._readChunkSize = readChunkSize;
        }

        public void SetWriteChunkSize(int writeChunkSize)
        {
            this._writeChunkSize = writeChunkSize;
        }

        public void StartDecoding()
        {
            this._decoderState = DecoderState.Ok;
            this._decoderBufferAmount = 0L;
        }

        public bool CanContinueDecoding
        {
            get
            {
                return (this._decoderState != DecoderState.Buffer);
            }
        }

        public bool HasDecodedObject
        {
            get
            {
                return (this._decoderState == DecoderState.Ok);
            }
        }

        public RtmpMode Mode
        {
            get
            {
                return this._mode;
            }
        }

        public FluorineFx.ObjectEncoding ObjectEncoding
        {
            get
            {
                return this._objectEncoding;
            }
            set
            {
                this._objectEncoding = value;
            }
        }

        public RtmpState State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
                if (this._state == RtmpState.Disconnected)
                {
                    this.FreePackets(this._readPackets);
                    this.FreePackets(this._writePackets);
                }
            }
        }
    }
}

