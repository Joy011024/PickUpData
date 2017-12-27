namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api;
    using System;

    public interface IStreamPlaybackSecurity
    {
        bool IsPlaybackAllowed(IScope scope, string name, long start, long length, bool flushPlaylist);
    }
}

