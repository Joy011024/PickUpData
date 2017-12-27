namespace FluorineFx.Json
{
    using FluorineFx.Util;
    using System;
    using System.Globalization;
    using System.IO;

    public class JavaScriptConvert
    {
        public static readonly string False = "false";
        internal static long InitialJavaScriptDateTicks;
        internal static DateTime MinimumJavaScriptDate;
        public static readonly string Null = "null";
        public static readonly string True = "true";
        public static readonly string Undefined = "undefined";

        static JavaScriptConvert()
        {
            DateTime time = new DateTime(0x7b2, 1, 1);
            InitialJavaScriptDateTicks = time.Ticks;
            MinimumJavaScriptDate = new DateTime(100, 1, 1);
        }

        internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime)
        {
            if (dateTime < MinimumJavaScriptDate)
            {
                dateTime = MinimumJavaScriptDate;
            }
            return ((dateTime.Ticks - InitialJavaScriptDateTicks) / 0x2710L);
        }

        internal static DateTime ConvertJavaScriptTicksToDateTime(long javaScriptTicks)
        {
            return new DateTime((javaScriptTicks * 0x2710L) + InitialJavaScriptDateTicks);
        }

        public static object DeserializeObject(TextReader reader)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (JsonReader reader2 = new JsonReader(reader))
            {
                return serializer.Deserialize(reader2);
            }
        }

        public static object DeserializeObject(string value)
        {
            return DeserializeObject(value, null, null);
        }

        public static T DeserializeObject<T>(string value)
        {
            return DeserializeObject<T>(value, null);
        }

        public static object DeserializeObject(string value, Type type)
        {
            return DeserializeObject(value, type, null);
        }

        public static T DeserializeObject<T>(string value, params JsonConverter[] converters)
        {
            return (T) DeserializeObject(value, typeof(T), converters);
        }

        public static object DeserializeObject(string value, Type type, params JsonConverter[] converters)
        {
            StringReader reader = new StringReader(value);
            JsonSerializer serializer = new JsonSerializer();
            if (!CollectionUtils.IsNullOrEmpty<JsonConverter>(converters))
            {
                for (int i = 0; i < converters.Length; i++)
                {
                    serializer.Converters.Add(converters[i]);
                }
            }
            using (JsonReader reader2 = new JsonReader(reader))
            {
                return serializer.Deserialize(reader2, type);
            }
        }

        public static bool IsNull(object o)
        {
            return ((o == null) || (o.Equals(Null) || System.Convert.IsDBNull(o)));
        }

        public static string SerializeObject(object value)
        {
            return SerializeObject(value, null);
        }

        public static string SerializeObject(object value, params JsonConverter[] converters)
        {
            StringWriter textWriter = new StringWriter(CultureInfo.InvariantCulture);
            JsonSerializer serializer = new JsonSerializer();
            if (!CollectionUtils.IsNullOrEmpty<JsonConverter>(converters))
            {
                for (int i = 0; i < converters.Length; i++)
                {
                    serializer.Converters.Add(converters[i]);
                }
            }
            using (JsonWriter writer2 = new JsonWriter(textWriter))
            {
                serializer.Serialize(writer2, value);
            }
            return textWriter.ToString();
        }

        public static string ToString(bool value)
        {
            return (value ? True : False);
        }

        public static string ToString(byte value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(char value)
        {
            return ToString(char.ToString(value));
        }

        public static string ToString(DateTime value)
        {
            long num = ConvertDateTimeToJavaScriptTicks(value);
            return ("new Date(" + num + ")");
        }

        public static string ToString(decimal value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(double value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(Enum value)
        {
            return value.ToString();
        }

        public static string ToString(Guid value)
        {
            return ('"' + value.ToString("D", CultureInfo.InvariantCulture) + '"');
        }

        public static string ToString(short value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(int value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(long value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(object value)
        {
            if (value == null)
            {
                return Null;
            }
            if (value is IConvertible)
            {
                IConvertible convertible = value as IConvertible;
                switch (convertible.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        return ToString((bool) convertible);

                    case TypeCode.Char:
                        return ToString((char) convertible);

                    case TypeCode.SByte:
                        return ToString((sbyte) convertible);

                    case TypeCode.Byte:
                        return ToString((byte) convertible);

                    case TypeCode.Int16:
                        return ToString((short) convertible);

                    case TypeCode.UInt16:
                        return ToString((ushort) convertible);

                    case TypeCode.Int32:
                        return ToString((int) convertible);

                    case TypeCode.UInt32:
                        return ToString((uint) convertible);

                    case TypeCode.Int64:
                        return ToString((long) convertible);

                    case TypeCode.UInt64:
                        return ToString((ulong) convertible);

                    case TypeCode.Single:
                        return ToString((float) convertible);

                    case TypeCode.Double:
                        return ToString((double) convertible);

                    case TypeCode.Decimal:
                        return ToString((decimal) convertible);

                    case TypeCode.DateTime:
                        return ToString((DateTime) convertible);

                    case TypeCode.String:
                        return ToString((string) convertible);
                }
            }
            throw new ArgumentException(string.Format("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.", value.GetType()));
        }

        [CLSCompliant(false)]
        public static string ToString(sbyte value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(float value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(string value)
        {
            return ToString(value, '"');
        }

        [CLSCompliant(false)]
        public static string ToString(ushort value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        [CLSCompliant(false)]
        public static string ToString(uint value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        [CLSCompliant(false)]
        public static string ToString(ulong value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        public static string ToString(string value, char delimter)
        {
            return JavaScriptUtils.ToEscapedJavaScriptString(value, delimter, true);
        }
    }
}

