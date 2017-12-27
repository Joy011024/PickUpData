namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Api.Stream;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;

    internal class VideoCodecFactory
    {
        private IList _codecs = new ArrayList();
        private static ILog log = LogManager.GetLogger(typeof(VideoCodecFactory));

        public IVideoStreamCodec GetVideoCodec(ByteBuffer data)
        {
            foreach (IVideoStreamCodec codec3 in this._codecs)
            {
                IVideoStreamCodec codec2;
                try
                {
                    codec2 = Activator.CreateInstance(codec3.GetType()) as IVideoStreamCodec;
                }
                catch (Exception exception)
                {
                    log.Error("Could not create video codec instance.", exception);
                    continue;
                }
                log.Info("Trying codec " + codec2);
                if (codec2.CanHandleData(data))
                {
                    return codec2;
                }
            }
            return null;
        }

        public IList Codecs
        {
            set
            {
                this._codecs = value;
            }
        }
    }
}

