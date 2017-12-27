namespace FluorineFx.IO
{
    using System;

    internal class RawBinary
    {
        private byte[] _buffer;

        public RawBinary(byte[] buffer)
        {
            this._buffer = buffer;
        }

        public byte[] Buffer
        {
            get
            {
                return this._buffer;
            }
        }
    }
}

