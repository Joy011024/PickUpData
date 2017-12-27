namespace FluorineFx.Net
{
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
                            bufferPool = new BufferPool(0x1000);
                        }
                    }
                }
                return bufferPool;
            }
        }
    }
}

