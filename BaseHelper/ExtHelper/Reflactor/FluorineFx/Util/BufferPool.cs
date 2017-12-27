namespace FluorineFx.Util
{
    using System;

    internal class BufferPool : ObjectPool
    {
        private int _bufferSize;

        public BufferPool() : this(10, 10, 0x1000)
        {
        }

        public BufferPool(int bufferSize) : this(10, 10, bufferSize)
        {
        }

        public BufferPool(int capacity, int growth, int bufferSize)
        {
            this._bufferSize = bufferSize;
            base.Initialize(capacity, growth, true);
        }

        public void CheckIn(byte[] buffer)
        {
            base.CheckIn(buffer);
        }

        public byte[] CheckOut()
        {
            return (base.CheckOut() as byte[]);
        }

        protected override object GetObject()
        {
            return new byte[this._bufferSize];
        }
    }
}

