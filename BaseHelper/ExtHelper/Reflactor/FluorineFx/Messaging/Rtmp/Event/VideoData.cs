namespace FluorineFx.Messaging.Rtmp.Event
{
    using FluorineFx.IO;
    using FluorineFx.Messaging.Rtmp.Stream;
    using FluorineFx.Util;
    using System;

    internal class VideoData : BaseEvent, IStreamData
    {
        protected ByteBuffer _data;
        private FluorineFx.Messaging.Rtmp.Event.FrameType _frameType;

        public VideoData() : this(ByteBuffer.Allocate(0))
        {
        }

        public VideoData(ByteBuffer data) : base(EventType.STREAM_DATA)
        {
            this._frameType = FluorineFx.Messaging.Rtmp.Event.FrameType.UNKNOWN;
            base._dataType = 9;
            this._data = data;
            if ((this._data != null) && (this._data.Limit > 0))
            {
                long position = this._data.Position;
                int num2 = data.Get() & 0xff;
                data.Position = position;
                int num3 = (num2 & IOConstants.MASK_VIDEO_FRAMETYPE) >> 4;
                if (num3 == IOConstants.FLAG_FRAMETYPE_KEYFRAME)
                {
                    this._frameType = FluorineFx.Messaging.Rtmp.Event.FrameType.KEYFRAME;
                }
                else if (num3 == IOConstants.FLAG_FRAMETYPE_INTERFRAME)
                {
                    this._frameType = FluorineFx.Messaging.Rtmp.Event.FrameType.INTERFRAME;
                }
                else if (num3 == IOConstants.FLAG_FRAMETYPE_DISPOSABLE)
                {
                    this._frameType = FluorineFx.Messaging.Rtmp.Event.FrameType.DISPOSABLE_INTERFRAME;
                }
                else
                {
                    this._frameType = FluorineFx.Messaging.Rtmp.Event.FrameType.UNKNOWN;
                }
            }
        }

        public VideoData(byte[] data) : this(ByteBuffer.Wrap(data))
        {
        }

        public override string ToString()
        {
            return ("Video ts: " + base.Header.Timer);
        }

        public ByteBuffer Data
        {
            get
            {
                return this._data;
            }
        }

        public FluorineFx.Messaging.Rtmp.Event.FrameType FrameType
        {
            get
            {
                return this._frameType;
            }
        }
    }
}

