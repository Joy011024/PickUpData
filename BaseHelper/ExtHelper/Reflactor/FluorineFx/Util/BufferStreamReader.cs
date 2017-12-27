namespace FluorineFx.Util
{
    using System;
    using System.IO;

    internal class BufferStreamReader : TextReader
    {
        private ByteBuffer _buffer;

        public BufferStreamReader(ByteBuffer buffer)
        {
            this._buffer = buffer;
        }

        public override int Peek()
        {
            if (this._buffer.Position != this._buffer.Length)
            {
                return this._buffer.Get((int) this._buffer.Position);
            }
            return -1;
        }

        public override int Read()
        {
            if (this._buffer.Position == this._buffer.Length)
            {
                return -1;
            }
            return this._buffer.ReadByte();
        }
    }
}

