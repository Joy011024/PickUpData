namespace FluorineFx.HttpCompress
{
    using System;
    using System.IO;
    using System.Web;

    internal abstract class CompressingFilter : HttpOutputFilter
    {
        private CompressionLevels _compressionLevel;
        private bool hasWrittenHeaders;

        protected CompressingFilter(Stream baseStream, CompressionLevels compressionLevel) : base(baseStream)
        {
            this.hasWrittenHeaders = false;
            this._compressionLevel = compressionLevel;
        }

        protected void WriteHeaders()
        {
            HttpContext.Current.Response.AppendHeader("Content-Encoding", this.ContentEncoding);
            HttpContext.Current.Response.AppendHeader("X-Compressed-By", "FluorineHttpCompress");
            this.hasWrittenHeaders = true;
        }

        protected CompressionLevels CompressionLevel
        {
            get
            {
                return this._compressionLevel;
            }
        }

        public abstract string ContentEncoding { get; }

        protected bool HasWrittenHeaders
        {
            get
            {
                return this.hasWrittenHeaders;
            }
        }

        public virtual long TotalIn
        {
            get
            {
                return 0L;
            }
        }

        public virtual long TotalOut
        {
            get
            {
                return 0L;
            }
        }
    }
}

