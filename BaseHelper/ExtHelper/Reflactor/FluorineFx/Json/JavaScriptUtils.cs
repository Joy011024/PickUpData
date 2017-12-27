namespace FluorineFx.Json
{
    using FluorineFx.Util;
    using System;
    using System.IO;

    internal class JavaScriptUtils
    {
        public static string ToEscapedJavaScriptString(string value)
        {
            return ToEscapedJavaScriptString(value, '"', true);
        }

        public static string ToEscapedJavaScriptString(string value, char delimiter, bool appendDelimiters)
        {
            using (StringWriter writer = StringUtils.CreateStringWriter((StringUtils.GetLength(value) != 0) ? StringUtils.GetLength(value) : 0x10))
            {
                WriteEscapedJavaScriptString(writer, value, delimiter, appendDelimiters);
                return writer.ToString();
            }
        }

        public static void WriteEscapedJavaScriptChar(TextWriter writer, char c, char delimiter)
        {
            char ch = c;
            if (ch <= '"')
            {
                switch (ch)
                {
                    case '\b':
                        writer.Write(@"\b");
                        return;

                    case '\t':
                        writer.Write(@"\t");
                        return;

                    case '\n':
                        writer.Write(@"\n");
                        return;

                    case '\f':
                        writer.Write(@"\f");
                        return;

                    case '\r':
                        writer.Write(@"\r");
                        return;

                    case '"':
                        writer.Write((delimiter == '"') ? "\\\"" : "\"");
                        return;
                }
            }
            else
            {
                if (ch != '\'')
                {
                    if (ch != '\\')
                    {
                        goto Label_00D2;
                    }
                    writer.Write(@"\\");
                }
                else
                {
                    writer.Write((delimiter == '\'') ? @"\'" : "'");
                }
                return;
            }
        Label_00D2:
            if (c > '\x001f')
            {
                writer.Write(c);
            }
            else
            {
                StringUtils.WriteCharAsUnicode(writer, c);
            }
        }

        public static void WriteEscapedJavaScriptString(TextWriter writer, string value, char delimiter, bool appendDelimiters)
        {
            if (appendDelimiters)
            {
                writer.Write(delimiter);
            }
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    WriteEscapedJavaScriptChar(writer, value[i], delimiter);
                }
            }
            if (appendDelimiters)
            {
                writer.Write(delimiter);
            }
        }
    }
}

