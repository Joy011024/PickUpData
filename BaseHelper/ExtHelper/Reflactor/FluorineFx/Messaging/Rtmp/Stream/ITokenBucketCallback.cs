namespace FluorineFx.Messaging.Rtmp.Stream
{
    using System;

    public interface ITokenBucketCallback
    {
        void Available(ITokenBucket bucket, long tokenCount);
        void Reset(ITokenBucket bucket, long tokenCount);
    }
}

