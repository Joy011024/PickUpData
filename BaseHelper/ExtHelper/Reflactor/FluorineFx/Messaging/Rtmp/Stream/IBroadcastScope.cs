namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Messaging;
    using FluorineFx.Messaging.Api.Persistence;
    using System.Collections;

    internal interface IBroadcastScope : IBasicScope, ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener, IEventObservable, IPersistable, IEnumerable, IPipe, IMessageInput, IMessageOutput
    {
    }
}

