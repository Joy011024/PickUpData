namespace FluorineFx.Messaging.Rtmp.Event
{
    using FluorineFx.Messaging.Api.Service;
    using FluorineFx.Util;
    using System;
    using System.Collections;

    [CLSCompliant(false)]
    public class Notify : BaseEvent
    {
        private IDictionary _connectionParameters;
        protected ByteBuffer _data;
        private int _invokeId;
        protected IServiceCall _serviceCall;

        internal Notify() : base(EventType.SERVICE_CALL)
        {
            this._serviceCall = null;
            this._data = null;
            this._invokeId = 0;
            base._dataType = 0x12;
        }

        internal Notify(byte[] data) : this()
        {
            this._data = ByteBuffer.Wrap(data);
        }

        internal Notify(IServiceCall serviceCall) : this()
        {
            this._serviceCall = serviceCall;
        }

        internal Notify(ByteBuffer data) : this()
        {
            this._data = data;
        }

        public IDictionary ConnectionParameters
        {
            get
            {
                return this._connectionParameters;
            }
            set
            {
                this._connectionParameters = value;
            }
        }

        public ByteBuffer Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }

        public int InvokeId
        {
            get
            {
                return this._invokeId;
            }
            set
            {
                this._invokeId = value;
            }
        }

        public IServiceCall ServiceCall
        {
            get
            {
                return this._serviceCall;
            }
            set
            {
                this._serviceCall = value;
            }
        }
    }
}

