namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api;
    using System;

    public interface IStreamPublishSecurity
    {
        bool IsPublishAllowed(IScope scope, string name, string mode);
    }
}

