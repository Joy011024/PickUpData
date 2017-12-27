namespace FluorineFx.Messaging.Rtmp.Event
{
    using System;

    [CLSCompliant(false)]
    public class Ping : BaseEvent
    {
        private short _value1;
        private int _value2;
        private int _value3;
        private int _value4;
        public const short ClientBuffer = 3;
        public const short PingClient = 6;
        public const short PongServer = 7;
        public const short StreamClear = 0;
        public const short StreamPlayBufferClear = 1;
        public const short StreamReset = 4;
        public const int Undefined = -1;
        public const short Unknown2 = 2;
        public const short Unknown5 = 5;
        public const short Unknown8 = 8;

        internal Ping() : base(EventType.SYSTEM)
        {
            this._value3 = -1;
            this._value4 = -1;
            base._dataType = 4;
        }

        internal Ping(short value1, int value2) : this()
        {
            this._value1 = value1;
            this._value2 = value2;
        }

        internal Ping(short value1, int value2, int value3) : this()
        {
            this._value1 = value1;
            this._value2 = value2;
            this._value3 = value3;
        }

        internal Ping(short value1, int value2, int value3, int value4) : this()
        {
            this._value1 = value1;
            this._value2 = value2;
            this._value3 = value3;
            this._value4 = value4;
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "Ping: ", this._value1, ", ", this._value2, ", ", this._value3, ", ", this._value4 });
        }

        public short Value1
        {
            get
            {
                return this._value1;
            }
            set
            {
                this._value1 = value;
            }
        }

        public int Value2
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

        public int Value3
        {
            get
            {
                return this._value3;
            }
            set
            {
                this._value3 = value;
            }
        }

        public int Value4
        {
            get
            {
                return this._value4;
            }
            set
            {
                this._value4 = value;
            }
        }
    }
}

