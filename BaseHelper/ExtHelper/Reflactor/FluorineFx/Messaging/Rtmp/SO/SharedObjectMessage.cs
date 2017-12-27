namespace FluorineFx.Messaging.Rtmp.SO
{
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Rtmp.Event;
    using System;
    using System.Collections.Generic;

    [CLSCompliant(false)]
    public class SharedObjectMessage : BaseEvent, ISharedObjectMessage, IRtmpEvent, IEvent
    {
        private List<ISharedObjectEvent> _events;
        private string _name;
        private bool _persistent;
        private int _version;

        internal SharedObjectMessage(string name, int version, bool persistent) : this(null, name, version, persistent)
        {
        }

        internal SharedObjectMessage(IEventListener source, string name, int version, bool persistent) : base(EventType.SHARED_OBJECT, 0x13, source)
        {
            this._events = new List<ISharedObjectEvent>();
            this._version = 0;
            this._persistent = false;
            this._name = name;
            this._version = version;
            this._persistent = persistent;
        }

        public void AddEvent(ISharedObjectEvent sharedObjectEvent)
        {
            this._events.Add(sharedObjectEvent);
        }

        public void AddEvent(SharedObjectEventType type, string key, object value)
        {
            this._events.Add(new SharedObjectEvent(type, key, value));
        }

        public void AddEvents(IList<ISharedObjectEvent> events)
        {
            this._events.AddRange(events);
        }

        public void Clear()
        {
            this._events.Clear();
        }

        internal void SetIsPersistent(bool persistent)
        {
            this._persistent = persistent;
        }

        internal void SetName(string name)
        {
            this._name = name;
        }

        public IList<ISharedObjectEvent> Events
        {
            get
            {
                return this._events;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (this._events.Count == 0);
            }
        }

        public bool IsPersistent
        {
            get
            {
                return this._persistent;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public override object Object
        {
            get
            {
                return this.Events;
            }
        }

        public int Version
        {
            get
            {
                return this._version;
            }
        }
    }
}

