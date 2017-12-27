namespace FluorineFx.Messaging.Api
{
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Persistence;
    using System;
    using System.Collections;

    public interface IBasicScope : ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener, IEventObservable, IPersistable, IEnumerable
    {
        int Depth { get; }

        bool HasParent { get; }

        string Name { get; }

        IScope Parent { get; }

        string Path { get; }

        object SyncRoot { get; }

        string Type { get; }
    }
}

