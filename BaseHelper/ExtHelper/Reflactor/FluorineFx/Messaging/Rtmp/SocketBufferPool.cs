namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx.Configuration;
    using FluorineFx.Util;
    using System;

    internal class SocketBufferPool
    {
        private static BufferPool bufferPool;

        public static BufferPool Pool
        {
            get
            {
                if (bufferPool == null)
                {
                    lock (typeof(SocketBufferPool))
                    {
                        if (bufferPool == null)
                        {
                            bufferPool = new BufferPool(FluorineConfiguration.Instance.FluorineSettings.RtmpServer.RtmpTransportSettings.ReceiveBufferSize);
                        }
                    }
                }
                return bufferPool;
            }
        }
    }
}

