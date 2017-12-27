namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api;
    using System;

    [CLSCompliant(false)]
    public interface IStreamService : IScopeService, IService
    {
        void closeStream();
        int createStream();
        void deleteStream(int streamId);
        void deleteStream(IStreamCapableConnection connection, int streamId);
        void pause(bool pausePlayback, double position);
        void play(bool dontStop);
        void play(string name);
        void play(string name, double start);
        void play(string name, double start, double length);
        void play(string name, double start, double length, bool flushPlaylist);
        void publish(bool dontStop);
        void publish(string name);
        void publish(string name, string mode);
        void receiveAudio(bool receive);
        void receiveVideo(bool receive);
        void releaseStream(string streamName);
        void seek(double position);
    }
}

