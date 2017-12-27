namespace FluorineFx.Messaging.Api
{
    using FluorineFx.Messaging.Api.Event;

    public interface ICoreObject : IAttributeStore, IEventDispatcher, IEventHandler, IEventListener
    {
    }
}

