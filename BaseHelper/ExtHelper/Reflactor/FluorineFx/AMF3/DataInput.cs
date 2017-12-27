namespace FluorineFx.AMF3
{
    using FluorineFx;
    using FluorineFx.IO;
    using System;

    internal class DataInput : IDataInput
    {
        private AMFReader _amfReader;
        private FluorineFx.ObjectEncoding _objectEncoding;

        public DataInput(AMFReader amfReader)
        {
            this._amfReader = amfReader;
            this._objectEncoding = FluorineFx.ObjectEncoding.AMF3;
        }

        public bool ReadBoolean()
        {
            return this._amfReader.ReadBoolean();
        }

        public byte ReadByte()
        {
            return this._amfReader.ReadByte();
        }

        public void ReadBytes(byte[] bytes, uint offset, uint length)
        {
            byte[] buffer = this._amfReader.ReadBytes((int) length);
            for (int i = 0; i < buffer.Length; i++)
            {
                bytes[(int) ((IntPtr) (i + offset))] = buffer[i];
            }
        }

        public double ReadDouble()
        {
            return this._amfReader.ReadDouble();
        }

        public float ReadFloat()
        {
            return this._amfReader.ReadFloat();
        }

        public int ReadInt()
        {
            return this._amfReader.ReadInt32();
        }

        public object ReadObject()
        {
            object obj2 = null;
            if (this._objectEncoding == FluorineFx.ObjectEncoding.AMF0)
            {
                obj2 = this._amfReader.ReadData();
            }
            if (this._objectEncoding == FluorineFx.ObjectEncoding.AMF3)
            {
                obj2 = this._amfReader.ReadAMF3Data();
            }
            return obj2;
        }

        public short ReadShort()
        {
            return this._amfReader.ReadInt16();
        }

        public byte ReadUnsignedByte()
        {
            return this._amfReader.ReadByte();
        }

        public uint ReadUnsignedInt()
        {
            return (uint) this._amfReader.ReadInt32();
        }

        public ushort ReadUnsignedShort()
        {
            return this._amfReader.ReadUInt16();
        }

        public string ReadUTF()
        {
            return this._amfReader.ReadString();
        }

        public string ReadUTFBytes(uint length)
        {
            return this._amfReader.ReadUTF((int) length);
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

