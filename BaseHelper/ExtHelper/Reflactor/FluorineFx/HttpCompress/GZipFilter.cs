namespace FluorineFx.HttpCompress
{
    using System;
    using System.IO;
    using System.IO.Compression;

    internal class GZipFilter : CompressingFilter
    {
        private GZipStream m_stream;

        public GZipFilter(Stream baseStream) : base(baseStream, CompressionLevels.Normal)
        {
            this.m_stream = null;
            this.m_stream = new GZipStream(baseStream, CompressionMode.Compress);
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
                return "gzip";
            }
        }
    }
}

