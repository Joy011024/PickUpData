namespace FluorineFx.Util
{
    using System;
    using System.Collections;
    using System.Text;

    internal sealed class Layouter
    {
        private Stack _blockStack = new Stack();
        private int _indent = 0;
        private StringBuilder _sb = new StringBuilder();

        public StringBuilder Append(string value)
        {
            this._sb.Append(new string(' ', this._indent));
            this._sb.Append(value);
            return this._sb.Append("\n");
        }

        public StringBuilder AppendFormat(string format, params object[] args)
        {
            this._sb.Append(new string(' ', this._indent));
            this._sb.AppendFormat(format, args);
            return this._sb.Append("\n");
        }

        public void Begin()
        {
            this._blockStack.Push(this._indent);
            this._indent += 4;
        }

        public void End()
        {
            this._indent = (int) this._blockStack.Pop();
        }

        public override string ToString()
        {
            return this._sb.ToString();
        }

        public StringBuilder Builder
        {
            get
            {
                return this._sb;
            }
        }
    }
}

