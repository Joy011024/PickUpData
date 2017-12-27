namespace FluorineFx.Messaging.Rtmp.Stream
{
    using System;

    internal class DummyTokenBukcet : ITokenBucket
    {
        public bool AcquireToken(long tokenCount, long wait)
        {
            return true;
        }

        public long AcquireTokenBestEffort(long upperLimitCount)
        {
            return upperLimitCount;
        }

        public bool AcquireTokenNonblocking(long tokenCount, ITokenBucketCallback callback)
        {
            return true;
        }

        public void Reset()
        {
        }

        public long Capacity
        {
            get
            {
                return 0L;
            }
        }

        public double Speed
        {
            get
            {
                return 0.0;
            }
        }
    }
}

