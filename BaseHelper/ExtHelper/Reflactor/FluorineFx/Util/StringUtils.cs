namespace FluorineFx.Util
{
    using System;
    using System.Data.SqlTypes;
    using System.Globalization;
    using System.IO;
    using System.Text;

    internal abstract class StringUtils
    {
        public const char CarriageReturn = '\r';
        public const string CarriageReturnLineFeed = "\r\n";
        public const string Empty = "";
        public const char LineFeed = '\n';
        public const char Tab = '\t';

        protected StringUtils()
        {
        }

        public static bool CaselessEquals(string a, string b)
        {
            return (string.Compare(a, b, true, CultureInfo.InvariantCulture) == 0);
        }

        public static bool ContainsWhiteSpace(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsWhiteSpace(s[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static StringWriter CreateStringWriter(int capacity)
        {
            return new StringWriter(new StringBuilder(capacity));
        }

        public static string EnsureEndsWith(string target, string value)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (target.Length >= value.Length)
            {
                if (string.Compare(target, target.Length - value.Length, value, 0, value.Length, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return target;
                }
                string strA = target.TrimEnd(null);
                if (string.Compare(strA, strA.Length - value.Length, value, 0, value.Length, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return target;
                }
            }
            return (target + value);
        }

        public static int GetLength(string value)
        {
            if (value == null)
            {
                return 0;
            }
            return value.Length;
        }

        public static bool HasLength(string target)
        {
            return ((target != null) && (target.Length > 0));
        }

        public static bool HasText(string target)
        {
            if (target == null)
            {
                return false;
            }
            return HasLength(target.Trim());
        }

        public static bool IsNullOrEmpty(SqlString s)
        {
            return (s.IsNull || IsNullOrEmpty(s.Value));
        }

        public static bool IsNullOrEmpty(string s)
        {
            return ((s == null) || (s == string.Empty));
        }

        public static bool IsNullOrEmptyOrWhiteSpace(string s)
        {
            return (IsNullOrEmpty(s) || IsWhiteSpace(s));
        }

        public static bool IsWhiteSpace(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (s.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < s.Length; i++)
            {
                if (!char.IsWhiteSpace(s[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static string MaskEmptyString(string actual, string emptyValue)
        {
            return ((MaskNullString(actual).Length == 0) ? emptyValue : actual);
        }

        public static string MaskNullString(string actual)
        {
            return ((actual == null) ? string.Empty : actual);
        }

        public static string MaskNullString(string actual, string mask)
        {
            return ((actual == null) ? mask : actual);
        }

        public static string NullEmptyString(string s)
        {
            return (IsNullOrEmpty(s) ? null : s);
        }

        public static string ReplaceNewLines(string s, string replacement)
        {
            string str;
            StringReader reader = new StringReader(s);
            StringBuilder builder = new StringBuilder();
            bool flag = true;
            while ((str = reader.ReadLine()) != null)
            {
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    builder.Append(replacement);
                }
                builder.Append(str);
            }
            return builder.ToString();
        }

        public static string ToCharAsUnicode(char c)
        {
            using (StringWriter writer = new StringWriter())
            {
                WriteCharAsUnicode(writer, c);
                return writer.ToString();
            }
        }

        public static string Truncate(string s, int maximumLength)
        {
            return Truncate(s, maximumLength, "...");
        }

        public static string Truncate(string s, int maximumLength, string suffix)
        {
            if (suffix == null)
            {
                throw new ArgumentNullException("suffix");
            }
            if (maximumLength <= 0)
            {
                throw new ArgumentException("Maximum length must be greater than zero.", "maximumLength");
            }
            int length = maximumLength - suffix.Length;
            if (length <= 0)
            {
                throw new ArgumentException("Length of suffix string is greater or equal to maximumLength");
            }
            if ((s != null) && (s.Length > maximumLength))
            {
                return (s.Substring(0, length).Trim() + suffix);
            }
            return s;
        }

        public static void WriteCharAsUnicode(TextWriter writer, char c)
        {
            ValidationUtils.ArgumentNotNull(writer, "writer");
            char ch = NumberUtils.IntToHex((c >> 12) & '\x000f');
            char ch2 = NumberUtils.IntToHex((c >> 8) & '\x000f');
            char ch3 = NumberUtils.IntToHex((c >> 4) & '\x000f');
            char ch4 = NumberUtils.IntToHex(c & '\x000f');
            writer.Write('\\');
            writer.Write('u');
            writer.Write(ch);
            writer.Write(ch2);
            writer.Write(ch3);
            writer.Write(ch4);
        }
    }
}

