namespace FluorineFx
{
    using FluorineFx.Context;
    using System;

    public sealed class DateWrapper
    {
        public const string FluorineContextKey = "__@fluorinetimezone";

        internal DateWrapper()
        {
        }

        public static DateTime GetClientDate(DateTime date)
        {
            return date.Add(ClientTimeZone);
        }

        public static DateTime GetServerDate(DateTime date)
        {
            return date.Add(ServerTimeZone);
        }

        internal static int GetTimeZone()
        {
            object data = WebSafeCallContext.GetData("__@fluorinetimezone");
            if (data != null)
            {
                Convert.ToInt32(data);
            }
            return 0;
        }

        internal static void SetTimeZone(int timezone)
        {
            WebSafeCallContext.SetData("__@fluorinetimezone", timezone);
        }

        public static TimeSpan ClientTimeZone
        {
            get
            {
                return new TimeSpan(GetTimeZone(), 0, 0);
            }
        }

        public static TimeSpan ServerTimeZone
        {
            get
            {
                return TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Today);
            }
        }
    }
}

