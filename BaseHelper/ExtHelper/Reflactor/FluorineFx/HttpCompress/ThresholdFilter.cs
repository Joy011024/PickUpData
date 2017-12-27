namespace FluorineFx.HttpCompress
{
    using System;
    using System.IO;

    internal class ThresholdFilter : HttpOutputFilter
    {
        private Stream _baseStream;
        private MemoryStream _stream;
        private int _threshold;

        public ThresholdFilter(Stream compressStream, Stream baseStream, int threshold) : base(compressStream)
        {
            this._baseStream = baseStream;
            this._stream = new MemoryStream();
            this._threshold = threshold;
        }

        public override void Close()
        {
            byte[] buffer = this._stream.ToArray();
            if ((this._threshold <= 0) || (this._stream.Length > this._threshold))
            {
                base.BaseStream.Write(buffer, 0, buffer.Length);
                base.BaseStream.Flush();
                base.BaseStream.Close();
            }
            else
            {
                this._baseStream.Write(buffer, 0, buffer.Length);
                this._baseStream.Flush();
                this._baseStream.Close();
            }
            this._stream.Close();
        }

        public override void Flush()
        {
            this._stream.Flush();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this._stream.Write(buffer, offset, count);
        }

        public FluorineFx.HttpCompress.CompressingFilter CompressingFilter
        {
            get
            {
                return (base.BaseStream as FluorineFx.HttpCompress.CompressingFilter);
            }
        }
    }
}

