namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api;
    using System;

    [CLSCompliant(false)]
    public interface IClientStream : IStream, IBWControllable
    {
        void SetClientBufferDuration(int bufferTime);

        IStreamCapableConnection Connection { get; }

        int StreamId { get; }
    }
}

