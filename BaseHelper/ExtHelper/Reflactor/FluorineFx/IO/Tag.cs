namespace FluorineFx.IO
{
    using FluorineFx.Util;
    using System;

    internal class Tag : ITag
    {
        private byte[] _body;
        private int _bodySize;
        private byte _dataType;
        private byte _flags;
        private int _previuosTagSize;
        private int _timestamp;

        public Tag()
        {
        }

        public Tag(byte dataType, int timestamp, int bodySize, byte[] body, int previousTagSize)
        {
            this._dataType = dataType;
            this._timestamp = timestamp;
            this._bodySize = bodySize;
            this._previuosTagSize = previousTagSize;
            if (body != null)
            {
                this._body = new byte[bodySize];
                Buffer.BlockCopy(body, 0, this._body, 0, bodySize);
            }
        }

        public byte[] Body
        {
            get
            {
                return this._body;
            }
            set
            {
                this._body = value;
                this._bodySize = this._body.Length;
            }
        }

        public int BodySize
        {
            get
            {
                return this._bodySize;
            }
            set
            {
                this._bodySize = value;
            }
        }

        public ByteBuffer Data
        {
            get
            {
                return null;
            }
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

        public byte Flags
        {
            get
            {
                return this._flags;
            }
            set
            {
                this._flags = value;
            }
        }

        public int PreviousTagSize
        {
            get
            {
                return this._previuosTagSize;
            }
            set
            {
                this._previuosTagSize = value;
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

