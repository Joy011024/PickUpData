namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api;
    using System;

    [CLSCompliant(false)]
    public interface ISubscriberStream : IClientStream, IStream, IBWControllable
    {
        void Pause(int position);
        void Play();
        void ReceiveAudio(bool receive);
        void ReceiveVideo(bool receive);
        void Resume(int position);
        void Seek(int position);

        bool IsPaused { get; }
    }
}

