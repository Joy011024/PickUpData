namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Rtmp.Event;
    using System;

    internal class StreamTracker
    {
        private bool _firstAudio;
        private bool _firstNotify;
        private bool _firstVideo;
        private int _lastAudio;
        private int _lastNotify;
        private int _lastVideo;
        private bool _relative;

        public StreamTracker()
        {
            this.Reset();
        }

        public int Add(IRtmpEvent evt)
        {
            this._relative = true;
            int timestamp = evt.Timestamp;
            int num2 = 0;
            switch (evt.DataType)
            {
                case 8:
                    if (!this._firstAudio)
                    {
                        num2 = timestamp - this._lastAudio;
                        this._lastAudio = timestamp;
                        return num2;
                    }
                    num2 = evt.Timestamp;
                    this._relative = false;
                    this._firstAudio = false;
                    return num2;

                case 9:
                    if (!this._firstVideo)
                    {
                        num2 = timestamp - this._lastVideo;
                        this._lastVideo = timestamp;
                        return num2;
                    }
                    num2 = evt.Timestamp;
                    this._relative = false;
                    this._firstVideo = false;
                    return num2;

                case 0x12:
                case 20:
                    if (!this._firstNotify)
                    {
                        num2 = timestamp - this._lastNotify;
                        this._lastNotify = timestamp;
                        return num2;
                    }
                    num2 = evt.Timestamp;
                    this._relative = false;
                    this._firstNotify = false;
                    return num2;

                case 0x13:
                    return num2;
            }
            return num2;
        }

        public void Reset()
        {
            this._lastAudio = 0;
            this._lastVideo = 0;
            this._lastNotify = 0;
            this._firstVideo = true;
            this._firstAudio = true;
            this._firstNotify = true;
        }

        public bool IsRelative
        {
            get
            {
                return this._relative;
            }
        }
    }
}

