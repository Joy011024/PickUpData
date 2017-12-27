namespace FluorineFx.Util
{
    using System;

    internal sealed class NumberUtils
    {
        public static object Add(object m, object n)
        {
            CoerceTypes(ref m, ref n);
            if (n is int)
            {
                return (((int) m) + ((int) n));
            }
            if (n is short)
            {
                return (((short) m) + ((short) n));
            }
            if (n is long)
            {
                return (((long) m) + ((long) n));
            }
            if (n is ushort)
            {
                return (((ushort) m) + ((ushort) n));
            }
            if (n is uint)
            {
                return (((uint) m) + ((uint) n));
            }
            if (n is ulong)
            {
                return (((ulong) m) + ((ulong) n));
            }
            if (n is byte)
            {
                return (((byte) m) + ((byte) n));
            }
            if (n is sbyte)
            {
                return (((sbyte) m) + ((sbyte) n));
            }
            if (n is float)
            {
                return (((float) m) + ((float) n));
            }
            if (n is double)
            {
                return (((double) m) + ((double) n));
            }
            if (n is decimal)
            {
                return (((decimal) m) + ((decimal) n));
            }
            return null;
        }

        public static void CoerceTypes(ref object m, ref object n)
        {
            TypeCode typeCode = Convert.GetTypeCode(m);
            TypeCode code2 = Convert.GetTypeCode(n);
            if (typeCode > code2)
            {
                n = Convert.ChangeType(n, typeCode, null);
            }
            else
            {
                m = Convert.ChangeType(m, code2);
            }
        }

        public static object Divide(object m, object n)
        {
            CoerceTypes(ref m, ref n);
            if (n is int)
            {
                return (((int) m) / ((int) n));
            }
            if (n is short)
            {
                return (((short) m) / ((short) n));
            }
            if (n is long)
            {
                return (((long) m) / ((long) n));
            }
            if (n is ushort)
            {
                return (((ushort) m) / ((ushort) n));
            }
            if (n is uint)
            {
                return (((uint) m) / ((uint) n));
            }
            if (n is ulong)
            {
                return (((ulong) m) / ((ulong) n));
            }
            if (n is byte)
            {
                return (((byte) m) / ((byte) n));
            }
            if (n is sbyte)
            {
                return (((sbyte) m) / ((sbyte) n));
            }
            if (n is float)
            {
                return (((float) m) / ((float) n));
            }
            if (n is double)
            {
                return (((double) m) / ((double) n));
            }
            if (n is decimal)
            {
                return (((decimal) m) / ((decimal) n));
            }
            return null;
        }

        public static int HexToInt(char h)
        {
            if ((h >= '0') && (h <= '9'))
            {
                return (h - '0');
            }
            if ((h >= 'a') && (h <= 'f'))
            {
                return ((h - 'a') + 10);
            }
            if ((h >= 'A') && (h <= 'F'))
            {
                return ((h - 'A') + 10);
            }
            return -1;
        }

        public static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char) (n + 0x30);
            }
            return (char) ((n - 10) + 0x61);
        }

        public static bool IsDecimal(object number)
        {
            return (((number is float) || (number is double)) || (number is decimal));
        }

        public static bool IsInteger(object number)
        {
            return (((((number is int) || (number is short)) || ((number is long) || (number is uint))) || (((number is ushort) || (number is ulong)) || (number is byte))) || (number is sbyte));
        }

        public static bool IsNumber(object number)
        {
            return (IsInteger(number) || IsDecimal(number));
        }

        public static bool IsZero(object number)
        {
            if (number is int)
            {
                return (((int) number) == 0);
            }
            if (number is short)
            {
                return (((short) number) == 0);
            }
            if (number is long)
            {
                return (((long) number) == 0L);
            }
            if (number is ushort)
            {
                return (((int) number) == 0);
            }
            if (number is uint)
            {
                return (((long) number) == 0L);
            }
            if (number is ulong)
            {
                return (Convert.ToDecimal(number) == 0M);
            }
            if (number is byte)
            {
                return (((short) number) == 0);
            }
            if (number is sbyte)
            {
                return (((short) number) == 0);
            }
            if (number is float)
            {
                return (((float) number) == 0f);
            }
            if (number is double)
            {
                return (((double) number) == 0.0);
            }
            return ((number is decimal) && (((decimal) number) == 0M));
        }

        public static object Modulus(object m, object n)
        {
            CoerceTypes(ref m, ref n);
            if (n is int)
            {
                return (((int) m) % ((int) n));
            }
            if (n is short)
            {
                return (((short) m) % ((short) n));
            }
            if (n is long)
            {
                return (((long) m) % ((long) n));
            }
            if (n is ushort)
            {
                return (((ushort) m) % ((ushort) n));
            }
            if (n is uint)
            {
                return (((uint) m) % ((uint) n));
            }
            if (n is ulong)
            {
                return (((ulong) m) % ((ulong) n));
            }
            if (n is byte)
            {
                return (((byte) m) % ((byte) n));
            }
            if (n is sbyte)
            {
                return (((sbyte) m) % ((sbyte) n));
            }
            if (n is float)
            {
                return (((float) m) % ((float) n));
            }
            if (n is double)
            {
                return (((double) m) % ((double) n));
            }
            if (n is decimal)
            {
                return (((decimal) m) % ((decimal) n));
            }
            return null;
        }

        public static object Multiply(object m, object n)
        {
            CoerceTypes(ref m, ref n);
            if (n is int)
            {
                return (((int) m) * ((int) n));
            }
            if (n is short)
            {
                return (((short) m) * ((short) n));
            }
            if (n is long)
            {
                return (((long) m) * ((long) n));
            }
            if (n is ushort)
            {
                return (((ushort) m) * ((ushort) n));
            }
            if (n is uint)
            {
                return (((uint) m) * ((uint) n));
            }
            if (n is ulong)
            {
                return (((ulong) m) * ((ulong) n));
            }
            if (n is byte)
            {
                return (((byte) m) * ((byte) n));
            }
            if (n is sbyte)
            {
                return (((sbyte) m) * ((sbyte) n));
            }
            if (n is float)
            {
                return (((float) m) * ((float) n));
            }
            if (n is double)
            {
                return (((double) m) * ((double) n));
            }
            if (n is decimal)
            {
                return (((decimal) m) * ((decimal) n));
            }
            return null;
        }

        public static object Negate(object number)
        {
            if (number is int)
            {
                return -((int) number);
            }
            if (number is short)
            {
                return (int) -((short) number);
            }
            if (number is long)
            {
                return -((long) number);
            }
            if (number is ushort)
            {
                return -((int) number);
            }
            if (number is uint)
            {
                return -((long) number);
            }
            if (number is ulong)
            {
                return -Convert.ToDecimal(number);
            }
            if (number is byte)
            {
                return (int) -((short) number);
            }
            if (number is sbyte)
            {
                return (int) -((short) number);
            }
            if (number is float)
            {
                return -((float) number);
            }
            if (number is double)
            {
                return -((double) number);
            }
            if (!(number is decimal))
            {
                throw new ArgumentException(string.Format("'{0}' is not one of the supported numeric types.", number));
            }
            return -((decimal) number);
        }

        public static object Power(object m, object n)
        {
            return Math.Pow(Convert.ToDouble(m), Convert.ToDouble(n));
        }

        public static object Subtract(object m, object n)
        {
            CoerceTypes(ref m, ref n);
            if (n is int)
            {
                return (((int) m) - ((int) n));
            }
            if (n is short)
            {
                return (((short) m) - ((short) n));
            }
            if (n is long)
            {
                return (((long) m) - ((long) n));
            }
            if (n is ushort)
            {
                return (((ushort) m) - ((ushort) n));
            }
            if (n is uint)
            {
                return (((uint) m) - ((uint) n));
            }
            if (n is ulong)
            {
                return (((ulong) m) - ((ulong) n));
            }
            if (n is byte)
            {
                return (((byte) m) - ((byte) n));
            }
            if (n is sbyte)
            {
                return (((sbyte) m) - ((sbyte) n));
            }
            if (n is float)
            {
                return (((float) m) - ((float) n));
            }
            if (n is double)
            {
                return (((double) m) - ((double) n));
            }
            if (n is decimal)
            {
                return (((decimal) m) - ((decimal) n));
            }
            return null;
        }
    }
}

