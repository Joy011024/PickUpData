namespace FluorineFx.Messaging.Rtmp.Stream
{
    using System;

    public interface ITokenBucket
    {
        bool AcquireToken(long tokenCount, long wait);
        long AcquireTokenBestEffort(long upperLimitCount);
        bool AcquireTokenNonblocking(long tokenCount, ITokenBucketCallback callback);
        void Reset();

        long Capacity { get; }

        double Speed { get; }
    }
}

