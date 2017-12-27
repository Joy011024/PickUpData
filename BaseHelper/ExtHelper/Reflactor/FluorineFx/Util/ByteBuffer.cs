namespace FluorineFx.Util
{
    using System;
    using System.IO;

    [CLSCompliant(false)]
    public class ByteBuffer : Stream
    {
        private bool _autoExpand;
        private long _bookmark;
        private MemoryStream _stream;

        public ByteBuffer(MemoryStream stream)
        {
            this._stream = stream;
            this.ClearBookmark();
        }

        public static ByteBuffer Allocate(int capacity)
        {
            return new ByteBuffer(new MemoryStream(capacity)) { Limit = capacity };
        }

        public void Append(byte[] src)
        {
            this.Append(src, 0, src.Length);
        }

        public void Append(byte[] src, int offset, int length)
        {
            long position = this.Position;
            this.Position = this.Limit;
            this.Put(src, offset, length);
            this.Position = position;
        }

        public void ClearBookmark()
        {
            this._bookmark = -1L;
        }

        public override void Close()
        {
            this._stream.Close();
        }

        public void Compact()
        {
            if (this.Position != 0L)
            {
                for (int i = (int) this.Position; i < this.Limit; i++)
                {
                    byte num2 = this.Get(i);
                    this.Put(i - ((int) this.Position), num2);
                }
                this.Limit -= (int) this.Position;
                this.Position = 0L;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._stream != null)
                {
                    this._stream.Dispose();
                }
                this._stream = null;
            }
            base.Dispose(disposing);
        }

        public void Dump(string file)
        {
            FileStream stream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            byte[] buffer = this.ToArray();
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();
        }

        public void Fill(byte value, int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.Put(value);
            }
        }

        public void Flip()
        {
            this.Limit = (int) this.Position;
            this.Position = 0L;
        }

        public override void Flush()
        {
            this._stream.Flush();
        }

        public byte Get()
        {
            return (byte) this.ReadByte();
        }

        public byte Get(int index)
        {
            return this._stream.GetBuffer()[index];
        }

        public byte[] GetBuffer()
        {
            return this._stream.GetBuffer();
        }

        public int GetInt()
        {
            byte[] buffer = this.ReadBytes(4);
            return ((((buffer[0] << 0x18) | (buffer[1] << 0x10)) | (buffer[2] << 8)) | buffer[3]);
        }

        public short GetShort()
        {
            byte[] buffer = this.ReadBytes(2);
            return (short) ((buffer[0] << 8) | buffer[1]);
        }

        public void Put(ByteBuffer src)
        {
            while (src.HasRemaining)
            {
                this.Put(src.Get());
            }
        }

        public void Put(byte value)
        {
            this.WriteByte(value);
        }

        public void Put(byte[] src)
        {
            this.Put(src, 0, src.Length);
        }

        public void Put(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.Put(bytes);
        }

        public void Put(ByteBuffer src, int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.Put(src.Get());
            }
        }

        public void Put(int index, byte value)
        {
            this._stream.GetBuffer()[index] = value;
        }

        public void Put(int index, ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.WriteBytes(index, bytes);
        }

        public void Put(int index, uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.WriteBytes(index, bytes);
        }

        public static int Put(ByteBuffer output, ByteBuffer input, int numBytesMax)
        {
            int limit = input.Limit;
            int count = (numBytesMax > input.Remaining) ? input.Remaining : numBytesMax;
            output.Put(input, count);
            return count;
        }

        public void Put(byte[] src, int offset, int length)
        {
            this._stream.Write(src, offset, length);
        }

        public void PutInt(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.WriteBigEndian(bytes);
        }

        public void PutInt(int index, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            int num = bytes.Length - 1;
            for (int i = 0; num >= 0; i++)
            {
                this.Put(index + i, bytes[num]);
                num--;
            }
        }

        public void PutShort(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            this.WriteBigEndian(bytes);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this._stream.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            return this._stream.ReadByte();
        }

        public byte[] ReadBytes(int count)
        {
            byte[] buffer = new byte[count];
            for (int i = 0; i < count; i++)
            {
                buffer[i] = (byte) this.ReadByte();
            }
            return buffer;
        }

        public int ReadReverseInt()
        {
            byte[] buffer = this.ReadBytes(4);
            int num = 0;
            num += buffer[3] << 0x18;
            num += buffer[2] << 0x10;
            num += buffer[1] << 8;
            return (num + buffer[0]);
        }

        public int ReadUInt24()
        {
            byte[] buffer = this.ReadBytes(3);
            return (((buffer[0] << 0x10) | (buffer[1] << 8)) | buffer[2]);
        }

        public void Rewind()
        {
            this.Position = 0L;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this._stream.Seek(offset, origin);
        }

        public long SetBookmark()
        {
            this._bookmark = this.Position;
            return this._bookmark;
        }

        public override void SetLength(long value)
        {
            this._stream.SetLength(value);
        }

        public void Skip(int size)
        {
            this.Position += size;
        }

        public byte[] ToArray()
        {
            return this._stream.ToArray();
        }

        public static ByteBuffer Wrap(byte[] array)
        {
            return Wrap(array, 0, array.Length);
        }

        public static ByteBuffer Wrap(byte[] array, int offset, int length)
        {
            MemoryStream stream = new MemoryStream(array, offset, length, true, true) {
                Capacity = array.Length
            };
            stream.SetLength((long) (offset + length));
            stream.Position = offset;
            return new ByteBuffer(stream);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this._stream.Write(buffer, offset, count);
        }

        private void WriteBigEndian(byte[] bytes)
        {
            this.WriteBigEndian((int) this.Position, bytes);
        }

        private void WriteBigEndian(int index, byte[] bytes)
        {
            int num = bytes.Length - 1;
            for (int i = 0; num >= 0; i++)
            {
                this.Put(index + i, bytes[num]);
                num--;
            }
            this.Position += bytes.Length;
        }

        public override void WriteByte(byte value)
        {
            this._stream.WriteByte(value);
        }

        private void WriteBytes(int index, byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                this.Put(index + i, bytes[i]);
            }
        }

        public void WriteMediumInt(int value)
        {
            byte[] buffer = new byte[] { (byte) (0xff & (value >> 0x10)), (byte) (0xff & (value >> 8)), (byte) (0xff & value) };
            this.Write(buffer, 0, buffer.Length);
        }

        public void WriteReverseInt(int value)
        {
            byte[] buffer = new byte[4];
            buffer[3] = (byte) (0xff & (value >> 0x18));
            buffer[2] = (byte) (0xff & (value >> 0x10));
            buffer[1] = (byte) (0xff & (value >> 8));
            buffer[0] = (byte) (0xff & value);
            this.Write(buffer, 0, buffer.Length);
        }

        public bool AutoExpand
        {
            get
            {
                return this._autoExpand;
            }
            set
            {
                this._autoExpand = value;
            }
        }

        public long Bookmark
        {
            get
            {
                return this._bookmark;
            }
        }

        public override bool CanRead
        {
            get
            {
                return this._stream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this._stream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this._stream.CanWrite;
            }
        }

        public int Capacity
        {
            get
            {
                return this._stream.Capacity;
            }
        }

        public bool HasRemaining
        {
            get
            {
                return (this.Remaining > 0);
            }
        }

        public override long Length
        {
            get
            {
                return this._stream.Length;
            }
        }

        public int Limit
        {
            get
            {
                return (int) this._stream.Length;
            }
            set
            {
                this._stream.SetLength((long) value);
            }
        }

        public override long Position
        {
            get
            {
                return this._stream.Position;
            }
            set
            {
                this._stream.Position = value;
                if (this._bookmark > value)
                {
                    this._bookmark = 0L;
                }
            }
        }

        public int Remaining
        {
            get
            {
                return (this.Limit - ((int) this.Position));
            }
        }
    }
}

