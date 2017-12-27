namespace FluorineFx.Messaging.Rtmp.Event
{
    using System;

    [CLSCompliant(false)]
    public sealed class ServerBW : BaseEvent
    {
        private int _bandwidth;

        internal ServerBW(int bandwidth) : base(EventType.STREAM_CONTROL)
        {
            base._dataType = 5;
            this._bandwidth = bandwidth;
        }

        public override string ToString()
        {
            return ("ServerBW: " + this._bandwidth);
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
    }
}

