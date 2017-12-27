namespace FluorineFx.Messaging.Api
{
    using FluorineFx.Messaging.Messages;
    using System;

    public interface IClientRegistry
    {
        IClient GetClient(IMessage message);
        IClient GetClient(string id);
        bool HasClient(string id);
        IClient LookupClient(string id);
    }
}

