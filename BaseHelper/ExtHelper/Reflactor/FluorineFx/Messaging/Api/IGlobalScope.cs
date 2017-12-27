namespace FluorineFx.Messaging.Api
{
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Persistence;
    using System;
    using System.Collections;

    public interface IGlobalScope : IScope, IBasicScope, ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener, IEventObservable, IPersistable, IEnumerable, IServiceContainer, IServiceProvider
    {
        void Register();

        IServiceProvider ServiceProvider { get; }
    }
}

