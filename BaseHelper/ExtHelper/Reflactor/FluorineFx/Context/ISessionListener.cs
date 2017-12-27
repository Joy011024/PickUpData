namespace FluorineFx.Context
{
    using FluorineFx.Messaging.Api;
    using System;

    public interface ISessionListener
    {
        void SessionCreated(IClient client);
        void SessionDestroyed(IClient client);
    }
}

