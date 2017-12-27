namespace FluorineFx.Util
{
    using System;
    using System.Text;

    internal class DumpContext
    {
        private string _indent = string.Empty;
        private StringBuilder _sb = new StringBuilder();

        public void Append(string text)
        {
            this._sb.Append(text);
        }

        public void AppendLine(string text)
        {
            this._sb.Append(this._indent);
            this._sb.Append(text);
            this._sb.Append(Environment.NewLine);
        }

        public void Indent()
        {
            this._indent = this._indent + "\t";
        }

        public override string ToString()
        {
            return this._sb.ToString();
        }

        public void Unindent()
        {
            if (this._indent != string.Empty)
            {
                this._indent = this._indent.Remove(this._indent.Length - 1, 1);
            }
        }
    }
}

