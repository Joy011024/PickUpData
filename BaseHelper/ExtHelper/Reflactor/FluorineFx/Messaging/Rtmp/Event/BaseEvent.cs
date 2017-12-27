namespace FluorineFx.Messaging.Rtmp.Event
{
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Rtmp;
    using System;

    [CLSCompliant(false)]
    public class BaseEvent : IRtmpEvent, IEvent
    {
        protected byte _dataType;
        protected FluorineFx.Messaging.Api.Event.EventType _eventType;
        protected RtmpHeader _header;
        protected object _object;
        protected IEventListener _source;
        protected int _timestamp;

        internal BaseEvent(FluorineFx.Messaging.Api.Event.EventType eventType)
        {
            this._eventType = eventType;
            this._object = null;
        }

        internal BaseEvent(FluorineFx.Messaging.Api.Event.EventType eventType, byte dataType, IEventListener source)
        {
            this._dataType = dataType;
            this._eventType = eventType;
            this._source = source;
        }

        public byte DataType
        {
            get
            {
                return this._dataType;
            }
            set
            {
                this._dataType = value;
            }
        }

        public FluorineFx.Messaging.Api.Event.EventType EventType
        {
            get
            {
                return this._eventType;
            }
            set
            {
                this._eventType = value;
            }
        }

        public bool HasSource
        {
            get
            {
                return (this._source != null);
            }
        }

        public RtmpHeader Header
        {
            get
            {
                return this._header;
            }
            set
            {
                this._header = value;
            }
        }

        public virtual object Object
        {
            get
            {
                return this._object;
            }
        }

        public IEventListener Source
        {
            get
            {
                return this._source;
            }
            set
            {
                this._source = value;
            }
        }

        public int Timestamp
        {
            get
            {
                return this._timestamp;
            }
            set
            {
                this._timestamp = value;
            }
        }
    }
}

