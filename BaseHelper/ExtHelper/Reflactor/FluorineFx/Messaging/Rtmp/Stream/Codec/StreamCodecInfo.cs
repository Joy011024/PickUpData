namespace FluorineFx.Messaging.Rtmp.Stream.Codec
{
    using FluorineFx.Messaging.Api.Stream;
    using System;

    internal class StreamCodecInfo : IStreamCodecInfo
    {
        private bool _audio;
        private bool _video;
        private IVideoStreamCodec _videoCodec;

        public string AudioCodecName
        {
            get
            {
                return null;
            }
        }

        public bool HasAudio
        {
            get
            {
                return this._audio;
            }
            set
            {
                this._audio = value;
            }
        }

        public bool HasVideo
        {
            get
            {
                return this._video;
            }
            set
            {
                this._video = value;
            }
        }

        public IVideoStreamCodec VideoCodec
        {
            get
            {
                return this._videoCodec;
            }
            set
            {
                this._videoCodec = value;
            }
        }

        public string VideoCodecName
        {
            get
            {
                if (this._videoCodec == null)
                {
                    return null;
                }
                return this._videoCodec.Name;
            }
        }
    }
}

