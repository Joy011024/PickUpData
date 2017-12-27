namespace FluorineFx.Messaging.Api
{
    using System;

    public interface IServiceProvider
    {
        object GetService(Type serviceType);
    }
}

