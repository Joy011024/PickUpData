namespace FluorineFx.Json
{
    using System;

    internal class StringBuffer
    {
        private char[] _buffer;
        private static char[] _emptyBuffer = new char[0];
        private int _position;

        public StringBuffer()
        {
            this._buffer = _emptyBuffer;
        }

        public StringBuffer(int initalSize)
        {
            this._buffer = new char[initalSize];
        }

        public void Append(char value)
        {
            if ((this._position + 1) > this._buffer.Length)
            {
                this.EnsureSize(1);
            }
            this._buffer[this._position++] = value;
        }

        public void Clear()
        {
            this._buffer = _emptyBuffer;
            this._position = 0;
        }

        private void EnsureSize(int appendLength)
        {
            char[] destinationArray = new char[this._position + (appendLength * 2)];
            Array.Copy(this._buffer, destinationArray, this._position);
            this._buffer = destinationArray;
        }

        public override string ToString()
        {
            return new string(this._buffer, 0, this._position);
        }

        public int Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }
    }
}

