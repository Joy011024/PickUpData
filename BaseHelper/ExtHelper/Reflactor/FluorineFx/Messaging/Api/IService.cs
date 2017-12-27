namespace FluorineFx.Messaging.Api
{
    using FluorineFx.Configuration;
    using System;

    public interface IService
    {
        void Shutdown();
        void Start(ConfigurationSection configuration);
    }
}

