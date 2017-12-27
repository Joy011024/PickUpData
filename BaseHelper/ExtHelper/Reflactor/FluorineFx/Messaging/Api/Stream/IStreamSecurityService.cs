namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api;
    using System;
    using System.Collections;

    public interface IStreamSecurityService : IScopeService, IService
    {
        IEnumerator GetStreamPlaybackSecurity();
        IEnumerator GetStreamPublishSecurity();
        void RegisterStreamPlaybackSecurity(IStreamPlaybackSecurity handler);
        void RegisterStreamPublishSecurity(IStreamPublishSecurity handler);
        void UnregisterStreamPlaybackSecurity(IStreamPlaybackSecurity handler);
        void UnregisterStreamPublishSecurity(IStreamPublishSecurity handler);
    }
}

