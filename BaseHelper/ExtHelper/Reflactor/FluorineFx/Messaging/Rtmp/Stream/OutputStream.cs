namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Rtmp;
    using System;

    [CLSCompliant(false)]
    public class OutputStream
    {
        private RtmpChannel _audio;
        private RtmpChannel _data;
        private RtmpChannel _video;

        internal OutputStream(RtmpChannel video, RtmpChannel audio, RtmpChannel data)
        {
            this._video = video;
            this._audio = audio;
            this._data = data;
        }

        public void Close()
        {
            this._video.Close();
            this._audio.Close();
            this._data.Close();
        }

        public RtmpChannel Audio
        {
            get
            {
                return this._audio;
            }
        }

        public RtmpChannel Data
        {
            get
            {
                return this._data;
            }
        }

        public RtmpChannel Video
        {
            get
            {
                return this._video;
            }
        }
    }
}

