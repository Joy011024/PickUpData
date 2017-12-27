namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Util;
    using System;

    [CLSCompliant(false)]
    public interface IVideoStreamCodec
    {
        bool AddData(ByteBuffer data);
        bool CanHandleData(ByteBuffer data);
        ByteBuffer GetKeyframe();
        void Reset();

        bool CanDropFrames { get; }

        string Name { get; }
    }
}

