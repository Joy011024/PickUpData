namespace FluorineFx.Messaging.Api
{
    using System;

    public interface IServiceContainer : IServiceProvider
    {
        void AddService(Type serviceType, object service);
        void AddService(Type serviceType, object service, bool promote);
        void RemoveService(Type serviceType);
        void RemoveService(Type serviceType, bool promote);
    }
}

