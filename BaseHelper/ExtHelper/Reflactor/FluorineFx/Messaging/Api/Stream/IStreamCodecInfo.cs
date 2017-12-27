namespace FluorineFx.Messaging.Api.Stream
{
    using System;

    [CLSCompliant(false)]
    public interface IStreamCodecInfo
    {
        string AudioCodecName { get; }

        bool HasAudio { get; }

        bool HasVideo { get; }

        IVideoStreamCodec VideoCodec { get; }

        string VideoCodecName { get; }
    }
}

