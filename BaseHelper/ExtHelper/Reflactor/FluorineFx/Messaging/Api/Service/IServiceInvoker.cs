namespace FluorineFx.Messaging.Api.Service
{
    using FluorineFx.Messaging.Api;
    using System;

    public interface IServiceInvoker
    {
        bool Invoke(IServiceCall call, IScope scope);
        bool Invoke(IServiceCall call, object service);
    }
}

