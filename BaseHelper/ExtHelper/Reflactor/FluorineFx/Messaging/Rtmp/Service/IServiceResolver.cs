namespace FluorineFx.Messaging.Rtmp.Service
{
    using FluorineFx.Messaging.Api;
    using System;

    internal interface IServiceResolver
    {
        object ResolveService(IScope scope, string serviceName);
    }
}

