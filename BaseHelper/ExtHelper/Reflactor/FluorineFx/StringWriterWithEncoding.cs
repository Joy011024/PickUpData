namespace FluorineFx
{
    using System;
    using System.IO;
    using System.Text;

    internal sealed class StringWriterWithEncoding : StringWriter
    {
        private System.Text.Encoding _encoding;

        public StringWriterWithEncoding(StringBuilder sb, System.Text.Encoding encoding) : base(sb)
        {
            this._encoding = encoding;
        }

        public override System.Text.Encoding Encoding
        {
            get
            {
                return this._encoding;
            }
        }
    }
}

