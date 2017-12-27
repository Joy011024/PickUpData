namespace FluorineFx.Util
{
    using System;
    using System.Globalization;

    public abstract class DateTimeUtils
    {
        private static readonly string[] _internetDateFormats = new string[] { "dd MMM yyyy HH':'mm", "dd MMM yyyy HH':'mm':'ss", "ddd, dd MMM yyyy HH':'mm", "ddd, dd MMM yyyy HH':'mm':'ss" };

        protected DateTimeUtils()
        {
        }

        public static DateTime AssumeUniversalTime(DateTime dt)
        {
            return new DateTime(dt.Ticks, DateTimeKind.Utc);
        }

        public static DateTime? AssumeUniversalTime(DateTime? dt)
        {
            if (dt.HasValue)
            {
                return new DateTime?(AssumeUniversalTime(dt.Value));
            }
            return null;
        }

        public static DateTime ParseInternetDate(string input)
        {
            int num;
            ValidationUtils.ArgumentNotNull(input, "input");
            if (input.Length < _internetDateFormats[0].Length)
            {
                throw new ArgumentException("input");
            }
            int length = input.LastIndexOf(' ');
            if (length <= 0)
            {
                throw new FormatException();
            }
            string s = input.Substring(length + 1);
            if (s.Length == 0)
            {
                throw new FormatException("Missing time zone.");
            }
            switch (s)
            {
                case "UT":
                case "GMT":
                    num = 0;
                    break;

                case "EDT":
                    num = -400;
                    break;

                case "EST":
                case "CDT":
                    num = -500;
                    break;

                case "CST":
                case "MDT":
                    num = -600;
                    break;

                case "MST":
                case "PDT":
                    num = -700;
                    break;

                case "PST":
                    num = -800;
                    break;

                default:
                    if (s.Length < 4)
                    {
                        throw new FormatException("Length of local differential component must be at least 4 characters (HHMM).");
                    }
                    try
                    {
                        num = int.Parse(s, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
                    }
                    catch (FormatException exception)
                    {
                        throw new FormatException("Invalid local differential.", exception);
                    }
                    break;
            }
            input = input.Substring(0, length).TrimEnd(new char[0]);
            DateTime time = DateTime.ParseExact(input, _internetDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.AllowInnerWhite);
            TimeSpan span = new TimeSpan(num / 100, num % 100, 0);
            return time.Subtract(span).ToLocalTime();
        }
    }
}

