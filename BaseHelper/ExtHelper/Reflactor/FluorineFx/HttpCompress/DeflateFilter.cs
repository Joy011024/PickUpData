namespace FluorineFx.HttpCompress
{
    using System;
    using System.IO;
    using System.IO.Compression;

    internal class DeflateFilter : CompressingFilter
    {
        private DeflateStream m_stream;

        public DeflateFilter(Stream baseStream) : this(baseStream, CompressionLevels.Normal)
        {
        }

        public DeflateFilter(Stream baseStream, CompressionLevels compressionLevel) : base(baseStream, compressionLevel)
        {
            this.m_stream = null;
            this.m_stream = new DeflateStream(baseStream, CompressionMode.Compress);
        }

        public override void Close()
        {
            this.m_stream.Close();
        }

        public override void Flush()
        {
            this.m_stream.Flush();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!base.HasWrittenHeaders)
            {
                base.WriteHeaders();
            }
            this.m_stream.Write(buffer, offset, count);
        }

        public override string ContentEncoding
        {
            get
            {
                return "deflate";
            }
        }
    }
}

