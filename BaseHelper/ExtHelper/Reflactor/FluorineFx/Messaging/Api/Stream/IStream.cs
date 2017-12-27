namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api;
    using System;

    [CLSCompliant(false)]
    public interface IStream
    {
        void Close();
        void Start();
        void Stop();

        IStreamCodecInfo CodecInfo { get; }

        string Name { get; }

        IScope Scope { get; }

        object SyncRoot { get; }
    }
}

