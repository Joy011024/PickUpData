namespace FluorineFx.Messaging.Rtmp.Event
{
    using System;

    [CLSCompliant(false)]
    public class ChunkSize : BaseEvent
    {
        private int _size;

        internal ChunkSize(int size) : base(EventType.SYSTEM)
        {
            this._size = 0;
            base._dataType = 1;
            this._size = size;
        }

        public int Size
        {
            get
            {
                return this._size;
            }
            set
            {
                this._size = value;
            }
        }
    }
}

