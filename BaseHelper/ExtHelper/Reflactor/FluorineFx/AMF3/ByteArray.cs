namespace FluorineFx.AMF3
{
    using FluorineFx;
    using FluorineFx.IO;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.IO.Compression;

    [TypeConverter(typeof(ByteArrayConverter)), CLSCompliant(false)]
    public class ByteArray : IDataInput, IDataOutput
    {
        private DataInput _dataInput;
        private DataOutput _dataOutput;
        private System.IO.MemoryStream _memoryStream;
        private FluorineFx.ObjectEncoding _objectEncoding;

        public ByteArray()
        {
            this._memoryStream = new System.IO.MemoryStream();
            AMFReader amfReader = new AMFReader(this._memoryStream);
            AMFWriter amfWriter = new AMFWriter(this._memoryStream);
            this._dataOutput = new DataOutput(amfWriter);
            this._dataInput = new DataInput(amfReader);
            this._objectEncoding = FluorineFx.ObjectEncoding.AMF3;
        }

        public ByteArray(System.IO.MemoryStream ms)
        {
            this._memoryStream = ms;
            AMFReader amfReader = new AMFReader(this._memoryStream);
            AMFWriter amfWriter = new AMFWriter(this._memoryStream);
            this._dataOutput = new DataOutput(amfWriter);
            this._dataInput = new DataInput(amfReader);
            this._objectEncoding = FluorineFx.ObjectEncoding.AMF3;
        }

        internal ByteArray(byte[] buffer)
        {
            this._memoryStream = new System.IO.MemoryStream();
            this._memoryStream.Write(buffer, 0, buffer.Length);
            this._memoryStream.Position = 0L;
            AMFReader amfReader = new AMFReader(this._memoryStream);
            AMFWriter amfWriter = new AMFWriter(this._memoryStream);
            this._dataOutput = new DataOutput(amfWriter);
            this._dataInput = new DataInput(amfReader);
            this._objectEncoding = FluorineFx.ObjectEncoding.AMF3;
        }

        public void Compress()
        {
            byte[] buffer = this._memoryStream.GetBuffer();
            DeflateStream stream = new DeflateStream(this._memoryStream, CompressionMode.Compress, true);
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();
            AMFReader amfReader = new AMFReader(this._memoryStream);
            AMFWriter amfWriter = new AMFWriter(this._memoryStream);
            this._dataOutput = new DataOutput(amfWriter);
            this._dataInput = new DataInput(amfReader);
        }

        public byte[] GetBuffer()
        {
            return this._memoryStream.GetBuffer();
        }

        public bool ReadBoolean()
        {
            return this._dataInput.ReadBoolean();
        }

        public byte ReadByte()
        {
            return this._dataInput.ReadByte();
        }

        public void ReadBytes(byte[] bytes, uint offset, uint length)
        {
            this._dataInput.ReadBytes(bytes, offset, length);
        }

        public double ReadDouble()
        {
            return this._dataInput.ReadDouble();
        }

        public float ReadFloat()
        {
            return this._dataInput.ReadFloat();
        }

        public int ReadInt()
        {
            return this._dataInput.ReadInt();
        }

        public object ReadObject()
        {
            return this._dataInput.ReadObject();
        }

        public short ReadShort()
        {
            return this._dataInput.ReadShort();
        }

        public byte ReadUnsignedByte()
        {
            return this._dataInput.ReadUnsignedByte();
        }

        public uint ReadUnsignedInt()
        {
            return this._dataInput.ReadUnsignedInt();
        }

        public ushort ReadUnsignedShort()
        {
            return this._dataInput.ReadUnsignedShort();
        }

        public string ReadUTF()
        {
            return this._dataInput.ReadUTF();
        }

        public string ReadUTFBytes(uint length)
        {
            return this._dataInput.ReadUTFBytes(length);
        }

        public void Uncompress()
        {
            DeflateStream stream = new DeflateStream(this._memoryStream, CompressionMode.Decompress, false);
            System.IO.MemoryStream stream2 = new System.IO.MemoryStream();
            byte[] buffer = new byte[0x400];
            this._memoryStream.ReadByte();
            this._memoryStream.ReadByte();
            while (true)
            {
                int count = stream.Read(buffer, 0, buffer.Length);
                if (count <= 0)
                {
                    stream.Close();
                    this._memoryStream.Close();
                    this._memoryStream.Dispose();
                    this._memoryStream = stream2;
                    this._memoryStream.Position = 0L;
                    AMFReader amfReader = new AMFReader(this._memoryStream);
                    AMFWriter amfWriter = new AMFWriter(this._memoryStream);
                    this._dataOutput = new DataOutput(amfWriter);
                    this._dataInput = new DataInput(amfReader);
                    return;
                }
                stream2.Write(buffer, 0, count);
            }
        }

        public void WriteBoolean(bool value)
        {
            this._dataOutput.WriteBoolean(value);
        }

        public void WriteByte(byte value)
        {
            this._dataOutput.WriteByte(value);
        }

        public void WriteBytes(byte[] bytes, int offset, int length)
        {
            this._dataOutput.WriteBytes(bytes, offset, length);
        }

        public void WriteDouble(double value)
        {
            this._dataOutput.WriteDouble(value);
        }

        public void WriteFloat(float value)
        {
            this._dataOutput.WriteFloat(value);
        }

        public void WriteInt(int value)
        {
            this._dataOutput.WriteInt(value);
        }

        public void WriteObject(object value)
        {
            this._dataOutput.WriteObject(value);
        }

        public void WriteShort(short value)
        {
            this._dataOutput.WriteShort(value);
        }

        public void WriteUnsignedInt(uint value)
        {
            this._dataOutput.WriteUnsignedInt(value);
        }

        public void WriteUTF(string value)
        {
            this._dataOutput.WriteUTF(value);
        }

        public void WriteUTFBytes(string value)
        {
            this._dataOutput.WriteUTFBytes(value);
        }

        public uint Length
        {
            get
            {
                return (uint) this._memoryStream.Length;
            }
        }

        internal System.IO.MemoryStream MemoryStream
        {
            get
            {
                return this._memoryStream;
            }
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
                this._dataInput.ObjectEncoding = value;
                this._dataOutput.ObjectEncoding = value;
            }
        }

        public int Position
        {
            get
            {
                return (int) this._memoryStream.Position;
            }
            set
            {
                this._memoryStream.Position = value;
            }
        }
    }
}

