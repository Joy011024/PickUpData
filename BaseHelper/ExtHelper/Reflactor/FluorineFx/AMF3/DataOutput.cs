namespace FluorineFx.AMF3
{
    using FluorineFx;
    using FluorineFx.IO;
    using System;

    internal class DataOutput : IDataOutput
    {
        private AMFWriter _amfWriter;
        private FluorineFx.ObjectEncoding _objectEncoding;

        public DataOutput(AMFWriter amfWriter)
        {
            this._amfWriter = amfWriter;
            this._objectEncoding = FluorineFx.ObjectEncoding.AMF3;
        }

        public void WriteBoolean(bool value)
        {
            this._amfWriter.WriteBoolean(value);
        }

        public void WriteByte(byte value)
        {
            this._amfWriter.WriteByte(value);
        }

        public void WriteBytes(byte[] bytes, int offset, int length)
        {
            for (int i = offset; i < (offset + length); i++)
            {
                this._amfWriter.WriteByte(bytes[i]);
            }
        }

        public void WriteDouble(double value)
        {
            this._amfWriter.WriteDouble(value);
        }

        public void WriteFloat(float value)
        {
            this._amfWriter.WriteFloat(value);
        }

        public void WriteInt(int value)
        {
            this._amfWriter.WriteInt32(value);
        }

        public void WriteObject(object value)
        {
            if (this._objectEncoding == FluorineFx.ObjectEncoding.AMF0)
            {
                this._amfWriter.WriteData(FluorineFx.ObjectEncoding.AMF0, value);
            }
            if (this._objectEncoding == FluorineFx.ObjectEncoding.AMF3)
            {
                this._amfWriter.WriteAMF3Data(value);
            }
        }

        public void WriteShort(short value)
        {
            this._amfWriter.WriteShort(value);
        }

        public void WriteUnsignedInt(uint value)
        {
            this._amfWriter.WriteInt32((int) value);
        }

        public void WriteUTF(string value)
        {
            this._amfWriter.WriteUTF(value);
        }

        public void WriteUTFBytes(string value)
        {
            this._amfWriter.WriteUTFBytes(value);
        }

        public FluorineFx.ObjectEncoding ObjectEncoding
        {
            get
            {
                return this._objectEncoding;
            }
            set
            {
                this._objectEncoding = value;
            }
        }
    }
}

