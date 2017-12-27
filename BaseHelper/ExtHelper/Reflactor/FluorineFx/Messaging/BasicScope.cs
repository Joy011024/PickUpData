namespace FluorineFx.Messaging
{
    using FluorineFx.Collections.Generic;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Persistence;
    using System;
    using System.Collections;

    internal class BasicScope : PersistableAttributeStore, IBasicScope, ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener, IEventObservable, IPersistable, IEnumerable
    {
        protected bool _keepOnDisconnect;
        protected CopyOnWriteArray<IEventListener> _listeners;
        protected IScope _parent;
        private object _syncLock;

        public BasicScope(IScope parent, string type, string name, bool persistent) : base(type, name, null, persistent)
        {
            this._syncLock = new object();
            this._listeners = new CopyOnWriteArray<IEventListener>();
            this._keepOnDisconnect = false;
            this._parent = parent;
        }

        public virtual void AddEventListener(IEventListener listener)
        {
            this._listeners.Add(listener);
        }

        public virtual void DispatchEvent(IEvent evt)
        {
            foreach (IEventListener listener in this._listeners)
            {
                if ((evt.Source == null) || (evt.Source != listener))
                {
                    listener.NotifyEvent(evt);
                }
            }
        }

        public virtual IEnumerator GetEnumerator()
        {
            return null;
        }

        public ICollection GetEventListeners()
        {
            return this._listeners;
        }

        public bool HandleEvent(IEvent evt)
        {
            return false;
        }

        public void NotifyEvent(IEvent evt)
        {
        }

        public virtual void RemoveEventListener(IEventListener listener)
        {
            this._listeners.Remove(listener);
            if ((!this._keepOnDisconnect && ScopeUtils.IsRoom(this)) && (this._listeners.Count == 0))
            {
                this._parent.RemoveChildScope(this);
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public int Depth
        {
            get
            {
                if (this.HasParent)
                {
                    return (this._parent.Depth + 1);
                }
                return 0;
            }
        }

        public bool HasParent
        {
            get
            {
                return (this._parent != null);
            }
        }

        public virtual IScope Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                this._parent = value;
            }
        }

        public override string Path
        {
            get
            {
                if (this.HasParent)
                {
                    return (this._parent.Path + "/" + this._parent.Name);
                }
                return string.Empty;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this._syncLock;
            }
        }
    }
}

