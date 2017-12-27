namespace FluorineFx.Messaging.Api.Stream.Support
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Stream;
    using System;

    public class DenyAllStreamAccess : IStreamPublishSecurity, IStreamPlaybackSecurity
    {
        public bool IsPlaybackAllowed(IScope scope, string name, long start, long length, bool flushPlaylist)
        {
            return false;
        }

        public bool IsPublishAllowed(IScope scope, string name, string mode)
        {
            return false;
        }
    }
}

