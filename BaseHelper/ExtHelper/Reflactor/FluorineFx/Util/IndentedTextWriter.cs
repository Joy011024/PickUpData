namespace FluorineFx.Util
{
    using System;
    using System.IO;
    using System.Text;

    public sealed class IndentedTextWriter : TextWriter
    {
        private int _level;
        private string _tab;
        private bool _tabsPending;
        private TextWriter _writer;
        public const string DefaultTabString = "    ";

        public IndentedTextWriter(TextWriter writer) : this(writer, "    ")
        {
        }

        public IndentedTextWriter(TextWriter writer, string tabString) : base(CultureInfo.InvariantCulture)
        {
            this._writer = writer;
            this._tab = tabString;
            this._level = 0;
            this._tabsPending = false;
        }

        public override void Close()
        {
            this._writer.Close();
        }

        public override void Flush()
        {
            this._writer.Flush();
        }

        public override void Write(bool value)
        {
            this.WritePendingTabs();
            this._writer.Write(value);
        }

        public override void Write(char value)
        {
            this.WritePendingTabs();
            this._writer.Write(value);
        }

        public override void Write(string s)
        {
            this.WritePendingTabs();
            this._writer.Write(s);
        }

        public override void Write(char[] buffer)
        {
            this.WritePendingTabs();
            this._writer.Write(buffer);
        }

        public override void Write(double value)
        {
            this.WritePendingTabs();
            this._writer.Write(value);
        }

        public override void Write(int value)
        {
            this.WritePendingTabs();
            this._writer.Write(value);
        }

        public override void Write(long value)
        {
            this.WritePendingTabs();
            this._writer.Write(value);
        }

        public override void Write(object value)
        {
            this.WritePendingTabs();
            this._writer.Write(value);
        }

        public override void Write(float value)
        {
            this.WritePendingTabs();
            this._writer.Write(value);
        }

        public override void Write(string format, object arg0)
        {
            this.WritePendingTabs();
            this._writer.Write(format, arg0);
        }

        public override void Write(string format, params object[] arg)
        {
            this.WritePendingTabs();
            this._writer.Write(format, arg);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            this.WritePendingTabs();
            this._writer.Write(buffer, index, count);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            this.WritePendingTabs();
            this._writer.Write(format, arg0, arg1);
        }

        public override void WriteLine()
        {
            this.WritePendingTabs();
            this._writer.WriteLine();
            this._tabsPending = true;
        }

        public override void WriteLine(bool value)
        {
            this.WritePendingTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(char value)
        {
            this.WritePendingTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(char[] buffer)
        {
            this.WritePendingTabs();
            this._writer.WriteLine(buffer);
            this._tabsPending = true;
        }

        public override void WriteLine(double value)
        {
            this.WritePendingTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(int value)
        {
            this.WritePendingTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(long value)
        {
            this.WritePendingTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(object value)
        {
            this.WritePendingTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(float value)
        {
            this.WritePendingTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(string s)
        {
            this.WritePendingTabs();
            this._writer.WriteLine(s);
            this._tabsPending = true;
        }

        public override void WriteLine(string format, object arg0)
        {
            this.WritePendingTabs();
            this._writer.WriteLine(format, arg0);
            this._tabsPending = true;
        }

        public override void WriteLine(string format, params object[] arg)
        {
            this.WritePendingTabs();
            this._writer.WriteLine(format, arg);
            this._tabsPending = true;
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            this.WritePendingTabs();
            this._writer.WriteLine(buffer, index, count);
            this._tabsPending = true;
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            this.WritePendingTabs();
            this._writer.WriteLine(format, arg0, arg1);
            this._tabsPending = true;
        }

        public void WriteLineNoTabs(string s)
        {
            this._writer.WriteLine(s);
        }

        private void WritePendingTabs()
        {
            if (this._tabsPending)
            {
                this._tabsPending = false;
                for (int i = 0; i < this._level; i++)
                {
                    this._writer.Write(this._tab);
                }
            }
        }

        public override System.Text.Encoding Encoding
        {
            get
            {
                return this._writer.Encoding;
            }
        }

        public int Indent
        {
            get
            {
                return this._level;
            }
            set
            {
                this._level = (value < 0) ? 0 : value;
            }
        }

        public TextWriter InnerWriter
        {
            get
            {
                return this._writer;
            }
        }

        public override string NewLine
        {
            get
            {
                return this._writer.NewLine;
            }
            set
            {
                this._writer.NewLine = value;
            }
        }

        internal string TabString
        {
            get
            {
                return this._tab;
            }
        }
    }
}

