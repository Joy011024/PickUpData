namespace FluorineFx.HttpCompress
{
    using System;
    using System.IO;

    internal abstract class HttpOutputFilter : Stream
    {
        private Stream _sink;

        protected HttpOutputFilter(Stream baseStream)
        {
            this._sink = baseStream;
        }

        public override void Close()
        {
            this._sink.Close();
        }

        public override void Flush()
        {
            this._sink.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin direction)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long length)
        {
            throw new NotSupportedException();
        }

        protected Stream BaseStream
        {
            get
            {
                return this._sink;
            }
        }

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this._sink.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}

