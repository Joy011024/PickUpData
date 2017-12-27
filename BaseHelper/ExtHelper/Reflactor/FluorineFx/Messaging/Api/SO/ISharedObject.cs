namespace FluorineFx.Messaging.Api.SO
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Persistence;
    using System;
    using System.Collections;

    public interface ISharedObject : IBasicScope, ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener, IEventObservable, IPersistable, IEnumerable, ISharedObjectSecurityService, IService
    {
        void AddSharedObjectListener(ISharedObjectListener listener);
        void BeginUpdate();
        void BeginUpdate(IEventListener source);
        bool Clear();
        void Close();
        void EndUpdate();
        void RemoveSharedObjectListener(ISharedObjectListener listener);
        void SendMessage(string handler, IList arguments);

        bool IsLocked { get; }

        bool IsPersistentObject { get; }

        int Version { get; }
    }
}

