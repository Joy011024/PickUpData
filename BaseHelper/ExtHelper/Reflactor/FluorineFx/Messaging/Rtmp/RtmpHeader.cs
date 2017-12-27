namespace FluorineFx.Messaging.Rtmp
{
    using System;
    using System.Text;

    public sealed class RtmpHeader
    {
        private int _channelId = 0;
        private byte _headerDataType = 0;
        private int _size = 0;
        private int _streamId = 0;
        private int _timer = 0;
        private bool _timerRelative = true;

        internal RtmpHeader()
        {
        }

        public static int GetHeaderLength(HeaderType headerType)
        {
            switch (headerType)
            {
                case HeaderType.HeaderNew:
                    return 12;

                case HeaderType.HeaderSameSource:
                    return 8;

                case HeaderType.HeaderTimerChange:
                    return 4;

                case HeaderType.HeaderContinue:
                    return 1;
            }
            return -1;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ChannelId: ").Append(this._channelId).Append(", ");
            builder.Append("Timer: ").Append(this._timer).Append(" (" + (this._timerRelative ? "relative" : "absolute") + ')').Append(", ");
            builder.Append("Size: ").Append(this._size).Append(", ");
            builder.Append("DateType: ").Append(this._headerDataType).Append(", ");
            builder.Append("StreamId: ").Append(this._streamId);
            return builder.ToString();
        }

        public int ChannelId
        {
            get
            {
                return this._channelId;
            }
            set
            {
                this._channelId = value;
            }
        }

        public byte DataType
        {
            get
            {
                return this._headerDataType;
            }
            set
            {
                this._headerDataType = value;
            }
        }

        public bool IsTimerRelative
        {
            get
            {
                return this._timerRelative;
            }
            set
            {
                this._timerRelative = value;
            }
        }

        public int Size
        {
            get
            {
                return this._size;
            }
            set
            {
                this._size = value;
            }
        }

        public int StreamId
        {
            get
            {
                return this._streamId;
            }
            set
            {
                this._streamId = value;
            }
        }

        public int Timer
        {
            get
            {
                return this._timer;
            }
            set
            {
                this._timer = value;
            }
        }
    }
}

