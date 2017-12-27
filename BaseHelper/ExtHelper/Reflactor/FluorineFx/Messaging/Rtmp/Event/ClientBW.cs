namespace FluorineFx.Messaging.Rtmp.Event
{
    using System;

    [CLSCompliant(false)]
    public sealed class ClientBW : BaseEvent
    {
        private int _bandwidth;
        private byte _value2;

        public ClientBW(int bandwidth, byte value2) : base(EventType.STREAM_CONTROL)
        {
            base._dataType = 6;
            this._bandwidth = bandwidth;
            this._value2 = value2;
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "ClientBW: ", this._bandwidth, " value2: ", this._value2 });
        }

        public int Bandwidth
        {
            get
            {
                return this._bandwidth;
            }
            set
            {
                this._bandwidth = value;
            }
        }

        public byte Value2
        {
            get
            {
                return this._value2;
            }
            set
            {
                this._value2 = value;
            }
        }
    }
}

