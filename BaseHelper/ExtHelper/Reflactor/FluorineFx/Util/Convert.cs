namespace FluorineFx.Util
{
    using FluorineFx.AMF3;
    using System;
    using System.Data.SqlTypes;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    public class Convert
    {
        public static bool CanConvertToChar(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return true;
            }
            if (value is char)
            {
                return true;
            }
            if (value is string)
            {
                return ((value == null) || ((value as string).Length == 1));
            }
            if (value is bool)
            {
                return true;
            }
            if (value is IConvertible)
            {
                try
                {
                    ((IConvertible) value).ToChar(null);
                    return true;
                }
                catch (InvalidCastException)
                {
                    return false;
                }
            }
            return false;
        }

        public static bool CanConvertToNullableChar(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return true;
            }
            if (value is char)
            {
                return true;
            }
            if (value is string)
            {
                return ((value == null) || ((value as string).Length == 1));
            }
            if (value is bool)
            {
                return true;
            }
            if (value is IConvertible)
            {
                try
                {
                    ((IConvertible) value).ToChar(null);
                    return true;
                }
                catch (InvalidCastException)
                {
                    return false;
                }
            }
            return false;
        }

        private static InvalidCastException CreateInvalidCastException(Type originalType, Type conversionType)
        {
            return new InvalidCastException(string.Format("Invalid cast from {0} to {1}", originalType.FullName, conversionType.FullName));
        }

        public static bool ToBoolean(byte value)
        {
            return (value != 0);
        }

        public static bool ToBoolean(char value)
        {
            switch (value)
            {
                case 'F':
                    return false;

                case 'N':
                    return false;

                case 'T':
                    return true;

                case '\0':
                    return false;

                case '\x0001':
                    return true;

                case '0':
                    return false;

                case '1':
                    return true;

                case 'Y':
                    return true;

                case 'f':
                    return false;

                case 'n':
                    return false;

                case 't':
                    return true;

                case 'y':
                    return true;
            }
            throw new InvalidCastException(string.Format("Invalid cast from {0} to {1}", typeof(char).FullName, typeof(bool).FullName));
        }

        public static bool ToBoolean(SqlBoolean value)
        {
            return (!value.IsNull && value.Value);
        }

        public static bool ToBoolean(SqlByte value)
        {
            return (!value.IsNull && ToBoolean(value.Value));
        }

        public static bool ToBoolean(SqlDecimal value)
        {
            return (!value.IsNull && ToBoolean(value.Value));
        }

        public static bool ToBoolean(SqlDouble value)
        {
            return (!value.IsNull && ToBoolean(value.Value));
        }

        public static bool ToBoolean(SqlInt16 value)
        {
            return (!value.IsNull && ToBoolean(value.Value));
        }

        public static bool ToBoolean(SqlInt32 value)
        {
            return (!value.IsNull && ToBoolean(value.Value));
        }

        public static bool ToBoolean(SqlInt64 value)
        {
            return (!value.IsNull && ToBoolean(value.Value));
        }

        public static bool ToBoolean(SqlMoney value)
        {
            return (!value.IsNull && ToBoolean(value.Value));
        }

        public static bool ToBoolean(SqlSingle value)
        {
            return (!value.IsNull && ToBoolean(value.Value));
        }

        public static bool ToBoolean(SqlString value)
        {
            return (!value.IsNull && ToBoolean(value.Value));
        }

        public static bool ToBoolean(decimal value)
        {
            return (value != 0M);
        }

        public static bool ToBoolean(double value)
        {
            return (value != 0.0);
        }

        public static bool ToBoolean(short value)
        {
            return (value != 0);
        }

        public static bool ToBoolean(int value)
        {
            return (value != 0);
        }

        public static bool ToBoolean(long value)
        {
            return (value != 0L);
        }

        public static bool ToBoolean(bool? value)
        {
            return (value.HasValue ? value.Value : false);
        }

        public static bool ToBoolean(byte? value)
        {
            return (value.HasValue ? (value.Value != 0) : false);
        }

        public static bool ToBoolean(char? value)
        {
            return (value.HasValue ? ToBoolean(value.Value) : false);
        }

        public static bool ToBoolean(decimal? value)
        {
            return (value.HasValue ? (value.Value != 0M) : false);
        }

        public static bool ToBoolean(double? value)
        {
            return (value.HasValue ? (value.Value != 0.0) : false);
        }

        public static bool ToBoolean(short? value)
        {
            return (value.HasValue ? (value.Value != 0) : false);
        }

        public static bool ToBoolean(int? value)
        {
            return (value.HasValue ? (value.Value != 0) : false);
        }

        public static bool ToBoolean(long? value)
        {
            return (value.HasValue ? (value.Value != 0L) : false);
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(sbyte? value)
        {
            return (value.HasValue ? (value.Value != 0) : false);
        }

        public static bool ToBoolean(float? value)
        {
            return (value.HasValue ? (value.Value != 0f) : false);
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(ushort? value)
        {
            return (value.HasValue ? (value.Value != 0) : false);
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(uint? value)
        {
            return (value.HasValue ? (value.Value != 0) : false);
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(ulong? value)
        {
            return (value.HasValue ? (value.Value != 0L) : false);
        }

        public static bool ToBoolean(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return false;
            }
            if (value is bool)
            {
                return (bool) value;
            }
            if (value is string)
            {
                return ToBoolean((string) value);
            }
            if (value is char)
            {
                return ToBoolean((char) value);
            }
            if (value is SqlBoolean)
            {
                return ToBoolean((SqlBoolean) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(bool));
            }
            return ((IConvertible) value).ToBoolean(null);
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(sbyte value)
        {
            return (value != 0);
        }

        public static bool ToBoolean(float value)
        {
            return (value != 0f);
        }

        public static bool ToBoolean(string value)
        {
            return ((value != null) && bool.Parse(value));
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(ushort value)
        {
            return (value != 0);
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(uint value)
        {
            return (value != 0);
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(ulong value)
        {
            return (value != 0L);
        }

        public static byte ToByte(bool value)
        {
            return (value ? ((byte) 1) : ((byte) 0));
        }

        public static byte ToByte(char value)
        {
            return (byte) value;
        }

        public static byte ToByte(SqlBoolean value)
        {
            return (value.IsNull ? ((byte) 0) : ToByte(value.Value));
        }

        public static byte ToByte(SqlByte value)
        {
            return (value.IsNull ? ((byte) 0) : value.Value);
        }

        public static byte ToByte(SqlDecimal value)
        {
            return (value.IsNull ? ((byte) 0) : ToByte(value.Value));
        }

        public static byte ToByte(SqlDouble value)
        {
            return (value.IsNull ? ((byte) 0) : ToByte(value.Value));
        }

        public static byte ToByte(SqlInt16 value)
        {
            return (value.IsNull ? ((byte) 0) : ToByte(value.Value));
        }

        public static byte ToByte(SqlInt32 value)
        {
            return (value.IsNull ? ((byte) 0) : ToByte(value.Value));
        }

        public static byte ToByte(SqlInt64 value)
        {
            return (value.IsNull ? ((byte) 0) : ToByte(value.Value));
        }

        public static byte ToByte(SqlMoney value)
        {
            return (value.IsNull ? ((byte) 0) : ToByte(value.Value));
        }

        public static byte ToByte(SqlSingle value)
        {
            return (value.IsNull ? ((byte) 0) : ToByte(value.Value));
        }

        public static byte ToByte(SqlString value)
        {
            return (value.IsNull ? ((byte) 0) : ToByte(value.Value));
        }

        public static byte ToByte(decimal value)
        {
            return (byte) value;
        }

        public static byte ToByte(double value)
        {
            return (byte) value;
        }

        public static byte ToByte(short value)
        {
            return (byte) value;
        }

        public static byte ToByte(int value)
        {
            return (byte) value;
        }

        public static byte ToByte(long value)
        {
            return (byte) value;
        }

        public static byte ToByte(bool? value)
        {
            return ((value.HasValue && value.Value) ? ((byte) 1) : ((byte) 0));
        }

        public static byte ToByte(byte? value)
        {
            return (value.HasValue ? value.Value : ((byte) 0));
        }

        public static byte ToByte(char? value)
        {
            return (value.HasValue ? ((byte) value.Value) : ((byte) 0));
        }

        public static byte ToByte(decimal? value)
        {
            return (value.HasValue ? ((byte) value.Value) : ((byte) 0));
        }

        public static byte ToByte(double? value)
        {
            return (value.HasValue ? ((byte) value.Value) : ((byte) 0));
        }

        public static byte ToByte(short? value)
        {
            return (value.HasValue ? ((byte) value.Value) : ((byte) 0));
        }

        public static byte ToByte(int? value)
        {
            return (value.HasValue ? ((byte) value.Value) : ((byte) 0));
        }

        public static byte ToByte(long? value)
        {
            return (value.HasValue ? ((byte) value.Value) : ((byte) 0));
        }

        [CLSCompliant(false)]
        public static byte ToByte(sbyte? value)
        {
            return (value.HasValue ? ((byte) value.Value) : ((byte) 0));
        }

        public static byte ToByte(float? value)
        {
            return (value.HasValue ? ((byte) value.Value) : ((byte) 0));
        }

        [CLSCompliant(false)]
        public static byte ToByte(ushort? value)
        {
            return (value.HasValue ? ((byte) value.Value) : ((byte) 0));
        }

        [CLSCompliant(false)]
        public static byte ToByte(uint? value)
        {
            return (value.HasValue ? ((byte) value.Value) : ((byte) 0));
        }

        [CLSCompliant(false)]
        public static byte ToByte(ulong? value)
        {
            return (value.HasValue ? ((byte) value.Value) : ((byte) 0));
        }

        public static byte ToByte(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return 0;
            }
            if (value is byte)
            {
                return (byte) value;
            }
            if (value is string)
            {
                return ToByte((string) value);
            }
            if (value is bool)
            {
                return ToByte((bool) value);
            }
            if (value is char)
            {
                return ToByte((char) value);
            }
            if (value is SqlByte)
            {
                return ToByte((SqlByte) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(byte));
            }
            return ((IConvertible) value).ToByte(null);
        }

        [CLSCompliant(false)]
        public static byte ToByte(sbyte value)
        {
            return (byte) value;
        }

        public static byte ToByte(float value)
        {
            return (byte) value;
        }

        public static byte ToByte(string value)
        {
            return ((value == null) ? ((byte) 0) : byte.Parse(value));
        }

        [CLSCompliant(false)]
        public static byte ToByte(ushort value)
        {
            return (byte) value;
        }

        [CLSCompliant(false)]
        public static byte ToByte(uint value)
        {
            return (byte) value;
        }

        [CLSCompliant(false)]
        public static byte ToByte(ulong value)
        {
            return (byte) value;
        }

        [CLSCompliant(false)]
        public static byte[] ToByteArray(ByteArray p)
        {
            if (p == null)
            {
                return null;
            }
            return p.GetBuffer();
        }

        public static byte[] ToByteArray(bool p)
        {
            return BitConverter.GetBytes(p);
        }

        public static byte[] ToByteArray(byte p)
        {
            return new byte[] { p };
        }

        public static byte[] ToByteArray(char p)
        {
            return BitConverter.GetBytes(p);
        }

        public static byte[] ToByteArray(SqlBinary p)
        {
            return (p.IsNull ? null : p.Value);
        }

        public static byte[] ToByteArray(SqlBoolean p)
        {
            return (p.IsNull ? null : ToByteArray(p.Value));
        }

        public static byte[] ToByteArray(SqlByte p)
        {
            return (p.IsNull ? null : ToByteArray(p.Value));
        }

        public static byte[] ToByteArray(SqlBytes p)
        {
            return (p.IsNull ? null : p.Value);
        }

        public static byte[] ToByteArray(SqlDateTime p)
        {
            return (p.IsNull ? null : ToByteArray(p.Value));
        }

        public static byte[] ToByteArray(SqlDecimal p)
        {
            return (p.IsNull ? null : ToByteArray(p.Value));
        }

        public static byte[] ToByteArray(SqlDouble p)
        {
            return (p.IsNull ? null : ToByteArray(p.Value));
        }

        public static byte[] ToByteArray(SqlGuid p)
        {
            return (p.IsNull ? null : p.ToByteArray());
        }

        public static byte[] ToByteArray(SqlInt16 p)
        {
            return (p.IsNull ? null : ToByteArray(p.Value));
        }

        public static byte[] ToByteArray(SqlInt32 p)
        {
            return (p.IsNull ? null : ToByteArray(p.Value));
        }

        public static byte[] ToByteArray(SqlInt64 p)
        {
            return (p.IsNull ? null : ToByteArray(p.Value));
        }

        public static byte[] ToByteArray(SqlMoney p)
        {
            return (p.IsNull ? null : ToByteArray(p.Value));
        }

        public static byte[] ToByteArray(SqlSingle p)
        {
            return (p.IsNull ? null : ToByteArray(p.Value));
        }

        public static byte[] ToByteArray(SqlString p)
        {
            return (p.IsNull ? null : ToByteArray(p.Value));
        }

        public static byte[] ToByteArray(DateTime p)
        {
            return ToByteArray(p.ToBinary());
        }

        public static byte[] ToByteArray(decimal p)
        {
            int[] bits = decimal.GetBits(p);
            byte[] dst = new byte[bits.Length << 2];
            for (int i = 0; i < bits.Length; i++)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(bits[i]), 0, dst, i * 4, 4);
            }
            return dst;
        }

        public static byte[] ToByteArray(double p)
        {
            return BitConverter.GetBytes(p);
        }

        public static byte[] ToByteArray(Guid p)
        {
            return ((p == Guid.Empty) ? null : p.ToByteArray());
        }

        public static byte[] ToByteArray(short p)
        {
            return BitConverter.GetBytes(p);
        }

        public static byte[] ToByteArray(int p)
        {
            return BitConverter.GetBytes(p);
        }

        public static byte[] ToByteArray(long p)
        {
            return BitConverter.GetBytes(p);
        }

        public static byte[] ToByteArray(Stream p)
        {
            if (p == null)
            {
                return null;
            }
            if (p is MemoryStream)
            {
                return ((MemoryStream) p).ToArray();
            }
            long num = p.Seek(0L, SeekOrigin.Begin);
            byte[] buffer = new byte[p.Length];
            p.Read(buffer, 0, buffer.Length);
            p.Position = num;
            return buffer;
        }

        public static byte[] ToByteArray(bool? p)
        {
            return (p.HasValue ? BitConverter.GetBytes(p.Value) : null);
        }

        public static byte[] ToByteArray(byte? p)
        {
            return (p.HasValue ? new byte[] { p.Value } : null);
        }

        public static byte[] ToByteArray(char? p)
        {
            return (p.HasValue ? BitConverter.GetBytes(p.Value) : null);
        }

        public static byte[] ToByteArray(DateTime? p)
        {
            return (p.HasValue ? ToByteArray(p.Value.ToBinary()) : null);
        }

        public static byte[] ToByteArray(decimal? p)
        {
            return (p.HasValue ? ToByteArray(p.Value) : null);
        }

        public static byte[] ToByteArray(double? p)
        {
            return (p.HasValue ? BitConverter.GetBytes(p.Value) : null);
        }

        public static byte[] ToByteArray(Guid? p)
        {
            return (p.HasValue ? p.Value.ToByteArray() : null);
        }

        public static byte[] ToByteArray(short? p)
        {
            return (p.HasValue ? BitConverter.GetBytes(p.Value) : null);
        }

        public static byte[] ToByteArray(int? p)
        {
            return (p.HasValue ? BitConverter.GetBytes(p.Value) : null);
        }

        public static byte[] ToByteArray(long? p)
        {
            return (p.HasValue ? BitConverter.GetBytes(p.Value) : null);
        }

        [CLSCompliant(false)]
        public static byte[] ToByteArray(sbyte? p)
        {
            return (p.HasValue ? new byte[] { ((byte) p.Value) } : null);
        }

        public static byte[] ToByteArray(float? p)
        {
            return (p.HasValue ? BitConverter.GetBytes(p.Value) : null);
        }

        public static byte[] ToByteArray(TimeSpan? p)
        {
            return (p.HasValue ? ToByteArray(p.Value.Ticks) : null);
        }

        [CLSCompliant(false)]
        public static byte[] ToByteArray(ushort? p)
        {
            return (p.HasValue ? BitConverter.GetBytes(p.Value) : null);
        }

        [CLSCompliant(false)]
        public static byte[] ToByteArray(uint? p)
        {
            return (p.HasValue ? BitConverter.GetBytes(p.Value) : null);
        }

        [CLSCompliant(false)]
        public static byte[] ToByteArray(ulong? p)
        {
            return (p.HasValue ? BitConverter.GetBytes(p.Value) : null);
        }

        public static byte[] ToByteArray(object p)
        {
            if ((p == null) || (p is DBNull))
            {
                return null;
            }
            if (p is byte[])
            {
                return (byte[]) p;
            }
            if (p is string)
            {
                return ToByteArray((string) p);
            }
            if (p is sbyte)
            {
                return ToByteArray((sbyte) p);
            }
            if (p is short)
            {
                return ToByteArray((short) p);
            }
            if (p is int)
            {
                return ToByteArray((int) p);
            }
            if (p is long)
            {
                return ToByteArray((long) p);
            }
            if (p is byte)
            {
                return ToByteArray((byte) p);
            }
            if (p is ushort)
            {
                return ToByteArray((ushort) p);
            }
            if (p is uint)
            {
                return ToByteArray((uint) p);
            }
            if (p is ulong)
            {
                return ToByteArray((ulong) p);
            }
            if (p is char)
            {
                return ToByteArray((char) p);
            }
            if (p is float)
            {
                return ToByteArray((float) p);
            }
            if (p is double)
            {
                return ToByteArray((double) p);
            }
            if (p is bool)
            {
                return ToByteArray((bool) p);
            }
            if (p is decimal)
            {
                return ToByteArray((decimal) p);
            }
            if (p is DateTime)
            {
                return ToByteArray((DateTime) p);
            }
            if (p is TimeSpan)
            {
                return ToByteArray((TimeSpan) p);
            }
            if (p is Stream)
            {
                return ToByteArray((Stream) p);
            }
            if (p is Guid)
            {
                return ToByteArray((Guid) p);
            }
            if (p is SqlString)
            {
                return ToByteArray((SqlString) p);
            }
            if (p is SqlByte)
            {
                return ToByteArray((SqlByte) p);
            }
            if (p is SqlInt16)
            {
                return ToByteArray((SqlInt16) p);
            }
            if (p is SqlInt32)
            {
                return ToByteArray((SqlInt32) p);
            }
            if (p is SqlInt64)
            {
                return ToByteArray((SqlInt64) p);
            }
            if (p is SqlSingle)
            {
                return ToByteArray((SqlSingle) p);
            }
            if (p is SqlDouble)
            {
                return ToByteArray((SqlDouble) p);
            }
            if (p is SqlDecimal)
            {
                return ToByteArray((SqlDecimal) p);
            }
            if (p is SqlMoney)
            {
                return ToByteArray((SqlMoney) p);
            }
            if (p is SqlBoolean)
            {
                return ToByteArray((SqlBoolean) p);
            }
            if (p is SqlDateTime)
            {
                return ToByteArray((SqlDateTime) p);
            }
            if (p is SqlBinary)
            {
                return ToByteArray((SqlBinary) p);
            }
            if (p is SqlBytes)
            {
                return ToByteArray((SqlBytes) p);
            }
            if (p is SqlGuid)
            {
                return ToByteArray((SqlGuid) p);
            }
            if (!(p is ByteArray))
            {
                throw CreateInvalidCastException(p.GetType(), typeof(byte[]));
            }
            return ToByteArray((ByteArray) p);
        }

        [CLSCompliant(false)]
        public static byte[] ToByteArray(sbyte p)
        {
            return new byte[] { ((byte) p) };
        }

        public static byte[] ToByteArray(float p)
        {
            return BitConverter.GetBytes(p);
        }

        public static byte[] ToByteArray(string p)
        {
            return ((p == null) ? null : Encoding.UTF8.GetBytes(p));
        }

        public static byte[] ToByteArray(TimeSpan p)
        {
            return ToByteArray(p.Ticks);
        }

        [CLSCompliant(false)]
        public static byte[] ToByteArray(ushort p)
        {
            return BitConverter.GetBytes(p);
        }

        [CLSCompliant(false)]
        public static byte[] ToByteArray(uint p)
        {
            return BitConverter.GetBytes(p);
        }

        [CLSCompliant(false)]
        public static byte[] ToByteArray(ulong p)
        {
            return BitConverter.GetBytes(p);
        }

        public static char ToChar(bool value)
        {
            return (value ? '\x0001' : '\0');
        }

        public static char ToChar(byte value)
        {
            return (char) value;
        }

        public static char ToChar(SqlBoolean value)
        {
            return (value.IsNull ? '\0' : ToChar(value.Value));
        }

        public static char ToChar(SqlByte value)
        {
            return (value.IsNull ? '\0' : ToChar(value.Value));
        }

        public static char ToChar(SqlDecimal value)
        {
            return (value.IsNull ? '\0' : ToChar(value.Value));
        }

        public static char ToChar(SqlDouble value)
        {
            return (value.IsNull ? '\0' : ToChar(value.Value));
        }

        public static char ToChar(SqlInt16 value)
        {
            return (value.IsNull ? '\0' : ToChar(value.Value));
        }

        public static char ToChar(SqlInt32 value)
        {
            return (value.IsNull ? '\0' : ToChar(value.Value));
        }

        public static char ToChar(SqlInt64 value)
        {
            return (value.IsNull ? '\0' : ToChar(value.Value));
        }

        public static char ToChar(SqlMoney value)
        {
            return (value.IsNull ? '\0' : ToChar(value.Value));
        }

        public static char ToChar(SqlSingle value)
        {
            return (value.IsNull ? '\0' : ToChar(value.Value));
        }

        public static char ToChar(SqlString value)
        {
            return (value.IsNull ? '\0' : ToChar(value.Value));
        }

        public static char ToChar(decimal value)
        {
            return (char) value;
        }

        public static char ToChar(double value)
        {
            return (char) ((ushort) value);
        }

        public static char ToChar(short value)
        {
            return (char) ((ushort) value);
        }

        public static char ToChar(int value)
        {
            return (char) value;
        }

        public static char ToChar(long value)
        {
            return (char) ((ushort) value);
        }

        public static char ToChar(bool? value)
        {
            return ((value.HasValue && value.Value) ? '\x0001' : '\0');
        }

        public static char ToChar(byte? value)
        {
            return (value.HasValue ? ((char) value.Value) : '\0');
        }

        public static char ToChar(char? value)
        {
            return (value.HasValue ? value.Value : '\0');
        }

        public static char ToChar(decimal? value)
        {
            return (value.HasValue ? ((char) value.Value) : '\0');
        }

        public static char ToChar(double? value)
        {
            return (value.HasValue ? ((char) ((ushort) value.Value)) : '\0');
        }

        public static char ToChar(short? value)
        {
            return (value.HasValue ? ((char) ((ushort) value.Value)) : '\0');
        }

        public static char ToChar(int? value)
        {
            return (value.HasValue ? ((char) value.Value) : '\0');
        }

        public static char ToChar(long? value)
        {
            return (value.HasValue ? ((char) ((ushort) value.Value)) : '\0');
        }

        [CLSCompliant(false)]
        public static char ToChar(sbyte? value)
        {
            return (value.HasValue ? ((char) ((ushort) value.Value)) : '\0');
        }

        public static char ToChar(float? value)
        {
            return (value.HasValue ? ((char) ((ushort) value.Value)) : '\0');
        }

        [CLSCompliant(false)]
        public static char ToChar(ushort? value)
        {
            return (value.HasValue ? ((char) value.Value) : '\0');
        }

        [CLSCompliant(false)]
        public static char ToChar(uint? value)
        {
            return (value.HasValue ? ((char) value.Value) : '\0');
        }

        [CLSCompliant(false)]
        public static char ToChar(ulong? value)
        {
            return (value.HasValue ? ((char) value.Value) : '\0');
        }

        public static char ToChar(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return '\0';
            }
            if (value is char)
            {
                return (char) value;
            }
            if (value is string)
            {
                return ToChar((string) value);
            }
            if (value is bool)
            {
                return ToChar((bool) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(char));
            }
            return ((IConvertible) value).ToChar(null);
        }

        [CLSCompliant(false)]
        public static char ToChar(sbyte value)
        {
            return (char) ((ushort) value);
        }

        public static char ToChar(float value)
        {
            return (char) ((ushort) value);
        }

        public static char ToChar(string value)
        {
            char ch;
            if (char.TryParse(value, out ch))
            {
                return ch;
            }
            return '\0';
        }

        [CLSCompliant(false)]
        public static char ToChar(ushort value)
        {
            return (char) value;
        }

        [CLSCompliant(false)]
        public static char ToChar(uint value)
        {
            return (char) value;
        }

        [CLSCompliant(false)]
        public static char ToChar(ulong value)
        {
            return (char) value;
        }

        public static char[] ToCharArray(SqlChars p)
        {
            return (p.IsNull ? null : p.Value);
        }

        public static char[] ToCharArray(SqlString p)
        {
            return (p.IsNull ? null : p.Value.ToCharArray());
        }

        public static char[] ToCharArray(object p)
        {
            if ((p == null) || (p is DBNull))
            {
                return null;
            }
            if (p is char[])
            {
                return (char[]) p;
            }
            if (p is string)
            {
                return ToCharArray((string) p);
            }
            if (p is SqlString)
            {
                return ToCharArray((SqlString) p);
            }
            if (p is SqlChars)
            {
                return ToCharArray((SqlChars) p);
            }
            return ToString(p).ToCharArray();
        }

        public static char[] ToCharArray(string p)
        {
            return ((p == null) ? null : p.ToCharArray());
        }

        public static DateTime ToDateTime(SqlDateTime value)
        {
            return (value.IsNull ? DateTime.MinValue : value.Value);
        }

        public static DateTime ToDateTime(SqlDouble value)
        {
            return (value.IsNull ? DateTime.MinValue : (DateTime.MinValue + TimeSpan.FromDays(value.Value)));
        }

        public static DateTime ToDateTime(SqlInt64 value)
        {
            return (value.IsNull ? DateTime.MinValue : (DateTime.MinValue + TimeSpan.FromTicks(value.Value)));
        }

        public static DateTime ToDateTime(SqlString value)
        {
            return (value.IsNull ? DateTime.MinValue : ToDateTime(value.Value));
        }

        public static DateTime ToDateTime(double value)
        {
            return (DateTime.MinValue + TimeSpan.FromDays(value));
        }

        public static DateTime ToDateTime(long value)
        {
            return (DateTime.MinValue + TimeSpan.FromTicks(value));
        }

        public static DateTime ToDateTime(DateTime? value)
        {
            return (value.HasValue ? value.Value : DateTime.MinValue);
        }

        public static DateTime ToDateTime(double? value)
        {
            return (value.HasValue ? (DateTime.MinValue + TimeSpan.FromDays(value.Value)) : DateTime.MinValue);
        }

        public static DateTime ToDateTime(long? value)
        {
            return (value.HasValue ? (DateTime.MinValue + TimeSpan.FromTicks(value.Value)) : DateTime.MinValue);
        }

        public static DateTime ToDateTime(TimeSpan? value)
        {
            return (value.HasValue ? (DateTime.MinValue + value.Value) : DateTime.MinValue);
        }

        public static DateTime ToDateTime(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return DateTime.MinValue;
            }
            if (value is DateTime)
            {
                return (DateTime) value;
            }
            if (value is string)
            {
                return ToDateTime((string) value);
            }
            if (value is TimeSpan)
            {
                return ToDateTime((TimeSpan) value);
            }
            if (value is long)
            {
                return ToDateTime((long) value);
            }
            if (value is double)
            {
                return ToDateTime((double) value);
            }
            if (value is SqlDateTime)
            {
                return ToDateTime((SqlDateTime) value);
            }
            if (value is SqlString)
            {
                return ToDateTime((SqlString) value);
            }
            if (value is SqlInt64)
            {
                return ToDateTime((SqlInt64) value);
            }
            if (value is SqlDouble)
            {
                return ToDateTime((SqlDouble) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(DateTime));
            }
            return ((IConvertible) value).ToDateTime(null);
        }

        public static DateTime ToDateTime(string value)
        {
            return ((value == null) ? DateTime.MinValue : DateTime.Parse(value));
        }

        public static DateTime ToDateTime(TimeSpan value)
        {
            return (DateTime.MinValue + value);
        }

        public static decimal ToDecimal(bool value)
        {
            return (value ? 1.0M : 0.0M);
        }

        public static decimal ToDecimal(byte value)
        {
            return value;
        }

        public static decimal ToDecimal(char value)
        {
            return value;
        }

        public static decimal ToDecimal(SqlBoolean value)
        {
            return (value.IsNull ? 0.0M : ToDecimal(value.Value));
        }

        public static decimal ToDecimal(SqlByte value)
        {
            return (value.IsNull ? 0.0M : ToDecimal(value.Value));
        }

        public static decimal ToDecimal(SqlDecimal value)
        {
            return (value.IsNull ? 0.0M : value.Value);
        }

        public static decimal ToDecimal(SqlDouble value)
        {
            return (value.IsNull ? 0.0M : ToDecimal(value.Value));
        }

        public static decimal ToDecimal(SqlInt16 value)
        {
            return (value.IsNull ? 0.0M : ToDecimal(value.Value));
        }

        public static decimal ToDecimal(SqlInt32 value)
        {
            return (value.IsNull ? 0.0M : ToDecimal(value.Value));
        }

        public static decimal ToDecimal(SqlInt64 value)
        {
            return (value.IsNull ? 0.0M : ToDecimal(value.Value));
        }

        public static decimal ToDecimal(SqlMoney value)
        {
            return (value.IsNull ? 0.0M : value.Value);
        }

        public static decimal ToDecimal(SqlSingle value)
        {
            return (value.IsNull ? 0.0M : ToDecimal(value.Value));
        }

        public static decimal ToDecimal(SqlString value)
        {
            return (value.IsNull ? 0.0M : ToDecimal(value.Value));
        }

        public static decimal ToDecimal(double value)
        {
            return (decimal) value;
        }

        public static decimal ToDecimal(short value)
        {
            return value;
        }

        public static decimal ToDecimal(int value)
        {
            return value;
        }

        public static decimal ToDecimal(long value)
        {
            return value;
        }

        public static decimal ToDecimal(bool? value)
        {
            return ((value.HasValue && value.Value) ? 1.0M : 0.0M);
        }

        public static decimal ToDecimal(byte? value)
        {
            return (value.HasValue ? ((decimal) value.Value) : 0.0M);
        }

        public static decimal ToDecimal(char? value)
        {
            return (value.HasValue ? ((decimal) value.Value) : 0.0M);
        }

        public static decimal ToDecimal(decimal? value)
        {
            return (value.HasValue ? value.Value : 0.0M);
        }

        public static decimal ToDecimal(double? value)
        {
            return (value.HasValue ? ((decimal) value.Value) : 0.0M);
        }

        public static decimal ToDecimal(short? value)
        {
            return (value.HasValue ? ((decimal) value.Value) : 0.0M);
        }

        public static decimal ToDecimal(int? value)
        {
            return (value.HasValue ? ((decimal) value.Value) : 0.0M);
        }

        public static decimal ToDecimal(long? value)
        {
            return (value.HasValue ? ((decimal) value.Value) : 0.0M);
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(sbyte? value)
        {
            return (value.HasValue ? ((decimal) value.Value) : 0.0M);
        }

        public static decimal ToDecimal(float? value)
        {
            return (value.HasValue ? ((decimal) value.Value) : 0.0M);
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(ushort? value)
        {
            return (value.HasValue ? ((decimal) value.Value) : 0.0M);
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(uint? value)
        {
            return (value.HasValue ? ((decimal) value.Value) : 0.0M);
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(ulong? value)
        {
            return (value.HasValue ? ((decimal) value.Value) : 0.0M);
        }

        public static decimal ToDecimal(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return 0.0M;
            }
            if (value is decimal)
            {
                return (decimal) value;
            }
            if (value is string)
            {
                return ToDecimal((string) value);
            }
            if (value is bool)
            {
                return ToDecimal((bool) value);
            }
            if (value is char)
            {
                return ToDecimal((char) value);
            }
            if (value is SqlDecimal)
            {
                return ToDecimal((SqlDecimal) value);
            }
            if (value is SqlMoney)
            {
                return ToDecimal((SqlMoney) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(decimal));
            }
            return ((IConvertible) value).ToDecimal(null);
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(sbyte value)
        {
            return value;
        }

        public static decimal ToDecimal(float value)
        {
            return (decimal) value;
        }

        public static decimal ToDecimal(string value)
        {
            return ((value == null) ? 0.0M : decimal.Parse(value));
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(ushort value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(uint value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(ulong value)
        {
            return value;
        }

        public static double ToDouble(bool value)
        {
            return (value ? 1.0 : 0.0);
        }

        public static double ToDouble(byte value)
        {
            return (double) value;
        }

        public static double ToDouble(char value)
        {
            return (double) value;
        }

        public static double ToDouble(SqlBoolean value)
        {
            return (value.IsNull ? 0.0 : ToDouble(value.Value));
        }

        public static double ToDouble(SqlByte value)
        {
            return (value.IsNull ? 0.0 : ToDouble(value.Value));
        }

        public static double ToDouble(SqlDateTime value)
        {
            return (value.IsNull ? 0.0 : ToDouble(value.Value));
        }

        public static double ToDouble(SqlDecimal value)
        {
            return (value.IsNull ? 0.0 : ToDouble(value.Value));
        }

        public static double ToDouble(SqlDouble value)
        {
            return (value.IsNull ? 0.0 : value.Value);
        }

        public static double ToDouble(SqlInt16 value)
        {
            return (value.IsNull ? 0.0 : ToDouble(value.Value));
        }

        public static double ToDouble(SqlInt32 value)
        {
            return (value.IsNull ? 0.0 : ToDouble(value.Value));
        }

        public static double ToDouble(SqlInt64 value)
        {
            return (value.IsNull ? 0.0 : ToDouble(value.Value));
        }

        public static double ToDouble(SqlMoney value)
        {
            return (value.IsNull ? 0.0 : ToDouble(value.Value));
        }

        public static double ToDouble(SqlSingle value)
        {
            return (value.IsNull ? 0.0 : ToDouble(value.Value));
        }

        public static double ToDouble(SqlString value)
        {
            return (value.IsNull ? 0.0 : ToDouble(value.Value));
        }

        public static double ToDouble(DateTime value)
        {
            TimeSpan span = (TimeSpan) (value - DateTime.MinValue);
            return span.TotalDays;
        }

        public static double ToDouble(decimal value)
        {
            return (double) value;
        }

        public static double ToDouble(short value)
        {
            return (double) value;
        }

        public static double ToDouble(int value)
        {
            return (double) value;
        }

        public static double ToDouble(long value)
        {
            return (double) value;
        }

        public static double ToDouble(bool? value)
        {
            return ((value.HasValue && value.Value) ? 1.0 : 0.0);
        }

        public static double ToDouble(byte? value)
        {
            return (value.HasValue ? ((double) value.Value) : 0.0);
        }

        public static double ToDouble(char? value)
        {
            return (value.HasValue ? ((double) value.Value) : 0.0);
        }

        public static double ToDouble(DateTime? value)
        {
            return (value.HasValue ? (value.Value - DateTime.MinValue).TotalDays : 0.0);
        }

        public static double ToDouble(decimal? value)
        {
            return (value.HasValue ? ((double) value.Value) : 0.0);
        }

        public static double ToDouble(double? value)
        {
            return (value.HasValue ? value.Value : 0.0);
        }

        public static double ToDouble(short? value)
        {
            return (value.HasValue ? ((double) value.Value) : 0.0);
        }

        public static double ToDouble(int? value)
        {
            return (value.HasValue ? ((double) value.Value) : 0.0);
        }

        public static double ToDouble(long? value)
        {
            return (value.HasValue ? ((double) value.Value) : 0.0);
        }

        [CLSCompliant(false)]
        public static double ToDouble(sbyte? value)
        {
            return (value.HasValue ? ((double) value.Value) : 0.0);
        }

        public static double ToDouble(float? value)
        {
            return (value.HasValue ? ((double) value.Value) : 0.0);
        }

        public static double ToDouble(TimeSpan? value)
        {
            return (value.HasValue ? value.Value.TotalDays : 0.0);
        }

        [CLSCompliant(false)]
        public static double ToDouble(ushort? value)
        {
            return (value.HasValue ? ((double) value.Value) : 0.0);
        }

        [CLSCompliant(false)]
        public static double ToDouble(uint? value)
        {
            return (value.HasValue ? ((double) value.Value) : 0.0);
        }

        [CLSCompliant(false)]
        public static double ToDouble(ulong? value)
        {
            return (value.HasValue ? ((double) value.Value) : 0.0);
        }

        public static double ToDouble(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return 0.0;
            }
            if (value is double)
            {
                return (double) value;
            }
            if (value is string)
            {
                return ToDouble((string) value);
            }
            if (value is bool)
            {
                return ToDouble((bool) value);
            }
            if (value is char)
            {
                return ToDouble((char) value);
            }
            if (value is DateTime)
            {
                return ToDouble((DateTime) value);
            }
            if (value is TimeSpan)
            {
                return ToDouble((TimeSpan) value);
            }
            if (value is SqlDouble)
            {
                return ToDouble((SqlDouble) value);
            }
            if (value is SqlDateTime)
            {
                return ToDouble((SqlDateTime) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(double));
            }
            return ((IConvertible) value).ToDouble(null);
        }

        [CLSCompliant(false)]
        public static double ToDouble(sbyte value)
        {
            return (double) value;
        }

        public static double ToDouble(float value)
        {
            return (double) value;
        }

        public static double ToDouble(string value)
        {
            return ((value == null) ? 0.0 : double.Parse(value));
        }

        public static double ToDouble(TimeSpan value)
        {
            return value.TotalDays;
        }

        [CLSCompliant(false)]
        public static double ToDouble(ushort value)
        {
            return (double) value;
        }

        [CLSCompliant(false)]
        public static double ToDouble(uint value)
        {
            return (double) value;
        }

        [CLSCompliant(false)]
        public static double ToDouble(ulong value)
        {
            return (double) value;
        }

        public static Guid ToGuid(SqlBinary value)
        {
            return (value.IsNull ? Guid.Empty : value.ToSqlGuid().Value);
        }

        public static Guid ToGuid(SqlGuid value)
        {
            return (value.IsNull ? Guid.Empty : value.Value);
        }

        public static Guid ToGuid(SqlString value)
        {
            return (value.IsNull ? Guid.Empty : new Guid(value.Value));
        }

        public static Guid ToGuid(Guid? value)
        {
            return (value.HasValue ? value.Value : Guid.Empty);
        }

        public static Guid ToGuid(byte[] value)
        {
            return ((value == null) ? Guid.Empty : new Guid(value));
        }

        public static Guid ToGuid(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return Guid.Empty;
            }
            if (value is Guid)
            {
                return (Guid) value;
            }
            if (value is string)
            {
                return ToGuid((string) value);
            }
            if (value is SqlGuid)
            {
                return ToGuid((SqlGuid) value);
            }
            if (value is SqlString)
            {
                return ToGuid((SqlString) value);
            }
            if (value is SqlBinary)
            {
                return ToGuid((SqlBinary) value);
            }
            if (value is byte[])
            {
                return ToGuid((byte[]) value);
            }
            if (!(value is Type))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(Guid));
            }
            return ToGuid((Type) value);
        }

        public static Guid ToGuid(string value)
        {
            return ((value == null) ? Guid.Empty : new Guid(value));
        }

        public static Guid ToGuid(Type value)
        {
            return ((value == null) ? Guid.Empty : value.GUID);
        }

        public static short ToInt16(bool value)
        {
            return (value ? ((short) 1) : ((short) 0));
        }

        public static short ToInt16(byte value)
        {
            return value;
        }

        public static short ToInt16(char value)
        {
            return (short) value;
        }

        public static short ToInt16(SqlBoolean value)
        {
            return (value.IsNull ? ((short) 0) : ToInt16(value.Value));
        }

        public static short ToInt16(SqlByte value)
        {
            return (value.IsNull ? ((short) 0) : ToInt16(value.Value));
        }

        public static short ToInt16(SqlDecimal value)
        {
            return (value.IsNull ? ((short) 0) : ToInt16(value.Value));
        }

        public static short ToInt16(SqlDouble value)
        {
            return (value.IsNull ? ((short) 0) : ToInt16(value.Value));
        }

        public static short ToInt16(SqlInt16 value)
        {
            return (value.IsNull ? ((short) 0) : value.Value);
        }

        public static short ToInt16(SqlInt32 value)
        {
            return (value.IsNull ? ((short) 0) : ToInt16(value.Value));
        }

        public static short ToInt16(SqlInt64 value)
        {
            return (value.IsNull ? ((short) 0) : ToInt16(value.Value));
        }

        public static short ToInt16(SqlMoney value)
        {
            return (value.IsNull ? ((short) 0) : ToInt16(value.Value));
        }

        public static short ToInt16(SqlSingle value)
        {
            return (value.IsNull ? ((short) 0) : ToInt16(value.Value));
        }

        public static short ToInt16(SqlString value)
        {
            return (value.IsNull ? ((short) 0) : ToInt16(value.Value));
        }

        public static short ToInt16(decimal value)
        {
            return (short) value;
        }

        public static short ToInt16(double value)
        {
            return (short) value;
        }

        public static short ToInt16(int value)
        {
            return (short) value;
        }

        public static short ToInt16(long value)
        {
            return (short) value;
        }

        public static short ToInt16(bool? value)
        {
            return ((value.HasValue && value.Value) ? ((short) 1) : ((short) 0));
        }

        public static short ToInt16(byte? value)
        {
            return (value.HasValue ? ((short) value.Value) : ((short) 0));
        }

        public static short ToInt16(char? value)
        {
            return (value.HasValue ? ((short) value.Value) : ((short) 0));
        }

        public static short ToInt16(decimal? value)
        {
            return (value.HasValue ? ((short) value.Value) : ((short) 0));
        }

        public static short ToInt16(double? value)
        {
            return (value.HasValue ? ((short) value.Value) : ((short) 0));
        }

        public static short ToInt16(short? value)
        {
            return (value.HasValue ? value.Value : ((short) 0));
        }

        public static short ToInt16(int? value)
        {
            return (value.HasValue ? ((short) value.Value) : ((short) 0));
        }

        public static short ToInt16(long? value)
        {
            return (value.HasValue ? ((short) value.Value) : ((short) 0));
        }

        [CLSCompliant(false)]
        public static short ToInt16(sbyte? value)
        {
            return (value.HasValue ? ((short) value.Value) : ((short) 0));
        }

        public static short ToInt16(float? value)
        {
            return (value.HasValue ? ((short) value.Value) : ((short) 0));
        }

        [CLSCompliant(false)]
        public static short ToInt16(ushort? value)
        {
            return (value.HasValue ? ((short) value.Value) : ((short) 0));
        }

        [CLSCompliant(false)]
        public static short ToInt16(uint? value)
        {
            return (value.HasValue ? ((short) value.Value) : ((short) 0));
        }

        [CLSCompliant(false)]
        public static short ToInt16(ulong? value)
        {
            return (value.HasValue ? ((short) value.Value) : ((short) 0));
        }

        public static short ToInt16(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return 0;
            }
            if (value is short)
            {
                return (short) value;
            }
            if (value is string)
            {
                return ToInt16((string) value);
            }
            if (value is bool)
            {
                return ToInt16((bool) value);
            }
            if (value is char)
            {
                return ToInt16((char) value);
            }
            if (value is SqlInt16)
            {
                return ToInt16((SqlInt16) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(short));
            }
            return ((IConvertible) value).ToInt16(null);
        }

        [CLSCompliant(false)]
        public static short ToInt16(sbyte value)
        {
            return value;
        }

        public static short ToInt16(float value)
        {
            return (short) value;
        }

        public static short ToInt16(string value)
        {
            return ((value == null) ? ((short) 0) : short.Parse(value));
        }

        [CLSCompliant(false)]
        public static short ToInt16(ushort value)
        {
            return (short) value;
        }

        [CLSCompliant(false)]
        public static short ToInt16(uint value)
        {
            return (short) value;
        }

        [CLSCompliant(false)]
        public static short ToInt16(ulong value)
        {
            return (short) value;
        }

        public static int ToInt32(bool value)
        {
            return (value ? 1 : 0);
        }

        public static int ToInt32(byte value)
        {
            return value;
        }

        public static int ToInt32(char value)
        {
            return value;
        }

        public static int ToInt32(SqlBoolean value)
        {
            return (value.IsNull ? 0 : ToInt32(value.Value));
        }

        public static int ToInt32(SqlByte value)
        {
            return (value.IsNull ? 0 : ToInt32(value.Value));
        }

        public static int ToInt32(SqlDecimal value)
        {
            return (value.IsNull ? 0 : ToInt32(value.Value));
        }

        public static int ToInt32(SqlDouble value)
        {
            return (value.IsNull ? 0 : ToInt32(value.Value));
        }

        public static int ToInt32(SqlInt16 value)
        {
            return (value.IsNull ? 0 : ToInt32(value.Value));
        }

        public static int ToInt32(SqlInt32 value)
        {
            return (value.IsNull ? 0 : value.Value);
        }

        public static int ToInt32(SqlInt64 value)
        {
            return (value.IsNull ? 0 : ToInt32(value.Value));
        }

        public static int ToInt32(SqlMoney value)
        {
            return (value.IsNull ? 0 : ToInt32(value.Value));
        }

        public static int ToInt32(SqlSingle value)
        {
            return (value.IsNull ? 0 : ToInt32(value.Value));
        }

        public static int ToInt32(SqlString value)
        {
            return (value.IsNull ? 0 : ToInt32(value.Value));
        }

        public static int ToInt32(decimal value)
        {
            return (int) value;
        }

        public static int ToInt32(double value)
        {
            return (int) value;
        }

        public static int ToInt32(short value)
        {
            return value;
        }

        public static int ToInt32(long value)
        {
            return (int) value;
        }

        public static int ToInt32(bool? value)
        {
            return ((value.HasValue && value.Value) ? 1 : 0);
        }

        public static int ToInt32(byte? value)
        {
            return (value.HasValue ? ((int) value.Value) : 0);
        }

        public static int ToInt32(char? value)
        {
            return (value.HasValue ? ((int) value.Value) : 0);
        }

        public static int ToInt32(decimal? value)
        {
            return (value.HasValue ? ((int) value.Value) : 0);
        }

        public static int ToInt32(double? value)
        {
            return (value.HasValue ? ((int) value.Value) : 0);
        }

        public static int ToInt32(short? value)
        {
            return (value.HasValue ? ((int) value.Value) : 0);
        }

        public static int ToInt32(int? value)
        {
            return (value.HasValue ? value.Value : 0);
        }

        public static int ToInt32(long? value)
        {
            return (value.HasValue ? ((int) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static int ToInt32(sbyte? value)
        {
            return (value.HasValue ? ((int) value.Value) : 0);
        }

        public static int ToInt32(float? value)
        {
            return (value.HasValue ? ((int) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static int ToInt32(ushort? value)
        {
            return (value.HasValue ? ((int) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static int ToInt32(uint? value)
        {
            return (value.HasValue ? ((int) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static int ToInt32(ulong? value)
        {
            return (value.HasValue ? ((int) value.Value) : 0);
        }

        public static int ToInt32(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return 0;
            }
            if (value is int)
            {
                return (int) value;
            }
            if (value is string)
            {
                return ToInt32((string) value);
            }
            if (value is bool)
            {
                return ToInt32((bool) value);
            }
            if (value is char)
            {
                return ToInt32((char) value);
            }
            if (value is SqlInt32)
            {
                return ToInt32((SqlInt32) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(int));
            }
            return ((IConvertible) value).ToInt32(null);
        }

        [CLSCompliant(false)]
        public static int ToInt32(sbyte value)
        {
            return value;
        }

        public static int ToInt32(float value)
        {
            return (int) value;
        }

        public static int ToInt32(string value)
        {
            return ((value == null) ? 0 : int.Parse(value));
        }

        [CLSCompliant(false)]
        public static int ToInt32(ushort value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static int ToInt32(uint value)
        {
            return (int) value;
        }

        [CLSCompliant(false)]
        public static int ToInt32(ulong value)
        {
            return (int) value;
        }

        public static long ToInt64(bool value)
        {
            return (value ? ((long) 1) : ((long) 0));
        }

        public static long ToInt64(byte value)
        {
            return (long) value;
        }

        public static long ToInt64(char value)
        {
            return (long) value;
        }

        public static long ToInt64(SqlBoolean value)
        {
            return (value.IsNull ? 0L : ToInt64(value.Value));
        }

        public static long ToInt64(SqlByte value)
        {
            return (value.IsNull ? 0L : ToInt64(value.Value));
        }

        public static long ToInt64(SqlDateTime value)
        {
            return (value.IsNull ? 0L : ToInt64(value.Value));
        }

        public static long ToInt64(SqlDecimal value)
        {
            return (value.IsNull ? 0L : ToInt64(value.Value));
        }

        public static long ToInt64(SqlDouble value)
        {
            return (value.IsNull ? 0L : ToInt64(value.Value));
        }

        public static long ToInt64(SqlInt16 value)
        {
            return (value.IsNull ? 0L : ToInt64(value.Value));
        }

        public static long ToInt64(SqlInt32 value)
        {
            return (value.IsNull ? 0L : ToInt64(value.Value));
        }

        public static long ToInt64(SqlInt64 value)
        {
            return (value.IsNull ? 0L : value.Value);
        }

        public static long ToInt64(SqlMoney value)
        {
            return (value.IsNull ? 0L : ToInt64(value.Value));
        }

        public static long ToInt64(SqlSingle value)
        {
            return (value.IsNull ? 0L : ToInt64(value.Value));
        }

        public static long ToInt64(SqlString value)
        {
            return (value.IsNull ? 0L : ToInt64(value.Value));
        }

        public static long ToInt64(DateTime value)
        {
            TimeSpan span = (TimeSpan) (value - DateTime.MinValue);
            return span.Ticks;
        }

        public static long ToInt64(decimal value)
        {
            return (long) value;
        }

        public static long ToInt64(double value)
        {
            return (long) value;
        }

        public static long ToInt64(short value)
        {
            return (long) value;
        }

        public static long ToInt64(int value)
        {
            return (long) value;
        }

        public static long ToInt64(bool? value)
        {
            return ((value.HasValue && value.Value) ? ((long) 1) : ((long) 0));
        }

        public static long ToInt64(byte? value)
        {
            return (value.HasValue ? ((long) ((ulong) value.Value)) : 0L);
        }

        public static long ToInt64(char? value)
        {
            return (value.HasValue ? ((long) value.Value) : 0L);
        }

        public static long ToInt64(DateTime? value)
        {
            return (value.HasValue ? (value.Value - DateTime.MinValue).Ticks : 0L);
        }

        public static long ToInt64(decimal? value)
        {
            return (value.HasValue ? ((long) value.Value) : 0L);
        }

        public static long ToInt64(double? value)
        {
            return (value.HasValue ? ((long) value.Value) : 0L);
        }

        public static long ToInt64(short? value)
        {
            return (value.HasValue ? ((long) value.Value) : 0L);
        }

        public static long ToInt64(int? value)
        {
            return (value.HasValue ? ((long) value.Value) : 0L);
        }

        public static long ToInt64(long? value)
        {
            return (value.HasValue ? value.Value : 0L);
        }

        [CLSCompliant(false)]
        public static long ToInt64(sbyte? value)
        {
            return (value.HasValue ? ((long) value.Value) : 0L);
        }

        public static long ToInt64(float? value)
        {
            return (value.HasValue ? ((long) value.Value) : 0L);
        }

        public static long ToInt64(TimeSpan? value)
        {
            return (value.HasValue ? value.Value.Ticks : 0L);
        }

        [CLSCompliant(false)]
        public static long ToInt64(ushort? value)
        {
            return (value.HasValue ? ((long) ((ulong) value.Value)) : 0L);
        }

        [CLSCompliant(false)]
        public static long ToInt64(uint? value)
        {
            return (value.HasValue ? ((long) ((ulong) value.Value)) : 0L);
        }

        [CLSCompliant(false)]
        public static long ToInt64(ulong? value)
        {
            return (value.HasValue ? ((long) value.Value) : 0L);
        }

        public static long ToInt64(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return 0L;
            }
            if (value is long)
            {
                return (long) value;
            }
            if (value is string)
            {
                return ToInt64((string) value);
            }
            if (value is char)
            {
                return ToInt64((char) value);
            }
            if (value is bool)
            {
                return ToInt64((bool) value);
            }
            if (value is DateTime)
            {
                return ToInt64((DateTime) value);
            }
            if (value is TimeSpan)
            {
                return ToInt64((TimeSpan) value);
            }
            if (value is SqlInt64)
            {
                return ToInt64((SqlInt64) value);
            }
            if (value is SqlDateTime)
            {
                return ToInt64((SqlDateTime) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(long));
            }
            return ((IConvertible) value).ToInt64(null);
        }

        [CLSCompliant(false)]
        public static long ToInt64(sbyte value)
        {
            return (long) value;
        }

        public static long ToInt64(float value)
        {
            return (long) value;
        }

        public static long ToInt64(string value)
        {
            return ((value == null) ? 0L : long.Parse(value));
        }

        public static long ToInt64(TimeSpan value)
        {
            return value.Ticks;
        }

        [CLSCompliant(false)]
        public static long ToInt64(ushort value)
        {
            return (long) value;
        }

        [CLSCompliant(false)]
        public static long ToInt64(uint value)
        {
            return (long) value;
        }

        [CLSCompliant(false)]
        public static long ToInt64(ulong value)
        {
            return (long) value;
        }

        public static bool? ToNullableBoolean(bool value)
        {
            return new bool?(value);
        }

        public static bool? ToNullableBoolean(byte value)
        {
            return new bool?(ToBoolean(value));
        }

        public static bool? ToNullableBoolean(char value)
        {
            return new bool?(ToBoolean(value));
        }

        public static bool? ToNullableBoolean(SqlBoolean value)
        {
            return (value.IsNull ? null : new bool?(value.Value));
        }

        public static bool? ToNullableBoolean(SqlByte value)
        {
            return (value.IsNull ? null : new bool?(ToBoolean(value.Value)));
        }

        public static bool? ToNullableBoolean(SqlDecimal value)
        {
            return (value.IsNull ? null : new bool?(ToBoolean(value.Value)));
        }

        public static bool? ToNullableBoolean(SqlDouble value)
        {
            return (value.IsNull ? null : new bool?(ToBoolean(value.Value)));
        }

        public static bool? ToNullableBoolean(SqlInt16 value)
        {
            return (value.IsNull ? null : new bool?(ToBoolean(value.Value)));
        }

        public static bool? ToNullableBoolean(SqlInt32 value)
        {
            return (value.IsNull ? null : new bool?(ToBoolean(value.Value)));
        }

        public static bool? ToNullableBoolean(SqlInt64 value)
        {
            return (value.IsNull ? null : new bool?(ToBoolean(value.Value)));
        }

        public static bool? ToNullableBoolean(SqlMoney value)
        {
            return (value.IsNull ? null : new bool?(ToBoolean(value.Value)));
        }

        public static bool? ToNullableBoolean(SqlSingle value)
        {
            return (value.IsNull ? null : new bool?(ToBoolean(value.Value)));
        }

        public static bool? ToNullableBoolean(SqlString value)
        {
            return (value.IsNull ? null : new bool?(ToBoolean(value.Value)));
        }

        public static bool? ToNullableBoolean(decimal value)
        {
            return new bool?(ToBoolean(value));
        }

        public static bool? ToNullableBoolean(double value)
        {
            return new bool?(ToBoolean(value));
        }

        public static bool? ToNullableBoolean(short value)
        {
            return new bool?(ToBoolean(value));
        }

        public static bool? ToNullableBoolean(int value)
        {
            return new bool?(ToBoolean(value));
        }

        public static bool? ToNullableBoolean(long value)
        {
            return new bool?(ToBoolean(value));
        }

        public static bool? ToNullableBoolean(byte? value)
        {
            return (value.HasValue ? new bool?(ToBoolean(value.Value)) : null);
        }

        public static bool? ToNullableBoolean(char? value)
        {
            return (value.HasValue ? new bool?(ToBoolean(value.Value)) : null);
        }

        public static bool? ToNullableBoolean(decimal? value)
        {
            return (value.HasValue ? new bool?(ToBoolean(value.Value)) : null);
        }

        public static bool? ToNullableBoolean(double? value)
        {
            return (value.HasValue ? new bool?(ToBoolean(value.Value)) : null);
        }

        public static bool? ToNullableBoolean(short? value)
        {
            return (value.HasValue ? new bool?(ToBoolean(value.Value)) : null);
        }

        public static bool? ToNullableBoolean(int? value)
        {
            return (value.HasValue ? new bool?(ToBoolean(value.Value)) : null);
        }

        public static bool? ToNullableBoolean(long? value)
        {
            return (value.HasValue ? new bool?(ToBoolean(value.Value)) : null);
        }

        [CLSCompliant(false)]
        public static bool? ToNullableBoolean(sbyte? value)
        {
            return (value.HasValue ? new bool?(ToBoolean(value.Value)) : null);
        }

        public static bool? ToNullableBoolean(float? value)
        {
            return (value.HasValue ? new bool?(ToBoolean(value.Value)) : null);
        }

        [CLSCompliant(false)]
        public static bool? ToNullableBoolean(ushort? value)
        {
            return (value.HasValue ? new bool?(ToBoolean(value.Value)) : null);
        }

        [CLSCompliant(false)]
        public static bool? ToNullableBoolean(uint? value)
        {
            return (value.HasValue ? new bool?(ToBoolean(value.Value)) : null);
        }

        [CLSCompliant(false)]
        public static bool? ToNullableBoolean(ulong? value)
        {
            return (value.HasValue ? new bool?(ToBoolean(value.Value)) : null);
        }

        public static bool? ToNullableBoolean(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is bool)
            {
                return ToNullableBoolean((bool) value);
            }
            if (value is string)
            {
                return ToNullableBoolean((string) value);
            }
            if (value is char)
            {
                return ToNullableBoolean((char) value);
            }
            if (value is SqlBoolean)
            {
                return ToNullableBoolean((SqlBoolean) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(bool?));
            }
            return new bool?(((IConvertible) value).ToBoolean(null));
        }

        [CLSCompliant(false)]
        public static bool? ToNullableBoolean(sbyte value)
        {
            return new bool?(ToBoolean(value));
        }

        public static bool? ToNullableBoolean(float value)
        {
            return new bool?(ToBoolean(value));
        }

        public static bool? ToNullableBoolean(string value)
        {
            return ((value == null) ? null : new bool?(bool.Parse(value)));
        }

        [CLSCompliant(false)]
        public static bool? ToNullableBoolean(ushort value)
        {
            return new bool?(ToBoolean(value));
        }

        [CLSCompliant(false)]
        public static bool? ToNullableBoolean(uint value)
        {
            return new bool?(ToBoolean(value));
        }

        [CLSCompliant(false)]
        public static bool? ToNullableBoolean(ulong value)
        {
            return new bool?(ToBoolean(value));
        }

        public static byte? ToNullableByte(bool value)
        {
            return new byte?(value ? ((byte) 1) : ((byte) 0));
        }

        public static byte? ToNullableByte(byte value)
        {
            return new byte?(value);
        }

        public static byte? ToNullableByte(char value)
        {
            return new byte?((byte) value);
        }

        public static byte? ToNullableByte(SqlBoolean value)
        {
            return (value.IsNull ? null : ToNullableByte(value.Value));
        }

        public static byte? ToNullableByte(SqlByte value)
        {
            return (value.IsNull ? null : new byte?(value.Value));
        }

        public static byte? ToNullableByte(SqlDecimal value)
        {
            return (value.IsNull ? null : ToNullableByte(value.Value));
        }

        public static byte? ToNullableByte(SqlDouble value)
        {
            return (value.IsNull ? null : ToNullableByte(value.Value));
        }

        public static byte? ToNullableByte(SqlInt16 value)
        {
            return (value.IsNull ? null : ToNullableByte(value.Value));
        }

        public static byte? ToNullableByte(SqlInt32 value)
        {
            return (value.IsNull ? null : ToNullableByte(value.Value));
        }

        public static byte? ToNullableByte(SqlInt64 value)
        {
            return (value.IsNull ? null : ToNullableByte(value.Value));
        }

        public static byte? ToNullableByte(SqlMoney value)
        {
            return (value.IsNull ? null : ToNullableByte(value.Value));
        }

        public static byte? ToNullableByte(SqlSingle value)
        {
            return (value.IsNull ? null : ToNullableByte(value.Value));
        }

        public static byte? ToNullableByte(SqlString value)
        {
            return (value.IsNull ? null : ToNullableByte(value.Value));
        }

        public static byte? ToNullableByte(decimal value)
        {
            return new byte?((byte) value);
        }

        public static byte? ToNullableByte(double value)
        {
            return new byte?((byte) value);
        }

        public static byte? ToNullableByte(short value)
        {
            return new byte?((byte) value);
        }

        public static byte? ToNullableByte(int value)
        {
            return new byte?((byte) value);
        }

        public static byte? ToNullableByte(long value)
        {
            return new byte?((byte) value);
        }

        public static byte? ToNullableByte(bool? value)
        {
            return (value.HasValue ? new byte?(value.Value ? ((byte) 1) : ((byte) 0)) : null);
        }

        public static byte? ToNullableByte(char? value)
        {
            return (value.HasValue ? new byte?((byte) value.Value) : null);
        }

        public static byte? ToNullableByte(decimal? value)
        {
            return (value.HasValue ? new byte?((byte) value.Value) : null);
        }

        public static byte? ToNullableByte(double? value)
        {
            return (value.HasValue ? new byte?((byte) value.Value) : null);
        }

        public static byte? ToNullableByte(short? value)
        {
            return (value.HasValue ? new byte?((byte) value.Value) : null);
        }

        public static byte? ToNullableByte(int? value)
        {
            return (value.HasValue ? new byte?((byte) value.Value) : null);
        }

        public static byte? ToNullableByte(long? value)
        {
            return (value.HasValue ? new byte?((byte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static byte? ToNullableByte(sbyte? value)
        {
            return (value.HasValue ? new byte?((byte) value.Value) : null);
        }

        public static byte? ToNullableByte(float? value)
        {
            return (value.HasValue ? new byte?((byte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static byte? ToNullableByte(ushort? value)
        {
            return (value.HasValue ? new byte?((byte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static byte? ToNullableByte(uint? value)
        {
            return (value.HasValue ? new byte?((byte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static byte? ToNullableByte(ulong? value)
        {
            return (value.HasValue ? new byte?((byte) value.Value) : null);
        }

        public static byte? ToNullableByte(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is byte)
            {
                return ToNullableByte((byte) value);
            }
            if (value is string)
            {
                return ToNullableByte((string) value);
            }
            if (value is char)
            {
                return ToNullableByte((char) value);
            }
            if (value is bool)
            {
                return ToNullableByte((bool) value);
            }
            if (value is SqlByte)
            {
                return ToNullableByte((SqlByte) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(byte?));
            }
            return new byte?(((IConvertible) value).ToByte(null));
        }

        [CLSCompliant(false)]
        public static byte? ToNullableByte(sbyte value)
        {
            return new byte?((byte) value);
        }

        public static byte? ToNullableByte(float value)
        {
            return new byte?((byte) value);
        }

        public static byte? ToNullableByte(string value)
        {
            return ((value == null) ? null : new byte?(byte.Parse(value)));
        }

        [CLSCompliant(false)]
        public static byte? ToNullableByte(ushort value)
        {
            return new byte?((byte) value);
        }

        [CLSCompliant(false)]
        public static byte? ToNullableByte(uint value)
        {
            return new byte?((byte) value);
        }

        [CLSCompliant(false)]
        public static byte? ToNullableByte(ulong value)
        {
            return new byte?((byte) value);
        }

        public static char? ToNullableChar(bool value)
        {
            return new char?(value ? ((char) ((ushort) 1)) : ((char) ((ushort) 0)));
        }

        public static char? ToNullableChar(byte value)
        {
            return new char?((char) value);
        }

        public static char? ToNullableChar(char value)
        {
            return new char?(value);
        }

        public static char? ToNullableChar(SqlBoolean value)
        {
            return (value.IsNull ? null : ToNullableChar(value.Value));
        }

        public static char? ToNullableChar(SqlByte value)
        {
            return (value.IsNull ? null : ToNullableChar(value.Value));
        }

        public static char? ToNullableChar(SqlDecimal value)
        {
            return (value.IsNull ? null : ToNullableChar(value.Value));
        }

        public static char? ToNullableChar(SqlDouble value)
        {
            return (value.IsNull ? null : ToNullableChar(value.Value));
        }

        public static char? ToNullableChar(SqlInt16 value)
        {
            return (value.IsNull ? null : ToNullableChar(value.Value));
        }

        public static char? ToNullableChar(SqlInt32 value)
        {
            return (value.IsNull ? null : ToNullableChar(value.Value));
        }

        public static char? ToNullableChar(SqlInt64 value)
        {
            return (value.IsNull ? null : ToNullableChar(value.Value));
        }

        public static char? ToNullableChar(SqlMoney value)
        {
            return (value.IsNull ? null : ToNullableChar(value.Value));
        }

        public static char? ToNullableChar(SqlSingle value)
        {
            return (value.IsNull ? null : ToNullableChar(value.Value));
        }

        public static char? ToNullableChar(SqlString value)
        {
            return (value.IsNull ? null : ToNullableChar(value.Value));
        }

        public static char? ToNullableChar(decimal value)
        {
            return new char?((char) value);
        }

        public static char? ToNullableChar(double value)
        {
            return new char?((char) ((ushort) value));
        }

        public static char? ToNullableChar(short value)
        {
            return new char?((char) ((ushort) value));
        }

        public static char? ToNullableChar(int value)
        {
            return new char?((char) ((ushort) value));
        }

        public static char? ToNullableChar(long value)
        {
            return new char?((char) ((ushort) value));
        }

        public static char? ToNullableChar(bool? value)
        {
            return (value.HasValue ? new char?(value.Value ? ((char) ((ushort) 1)) : ((char) ((ushort) 0))) : null);
        }

        public static char? ToNullableChar(byte? value)
        {
            return (value.HasValue ? new char?(value.Value) : null);
        }

        public static char? ToNullableChar(decimal? value)
        {
            return (value.HasValue ? new char?((char) value.Value) : null);
        }

        public static char? ToNullableChar(double? value)
        {
            return (value.HasValue ? new char?((char) ((ushort) value.Value)) : null);
        }

        public static char? ToNullableChar(short? value)
        {
            return (value.HasValue ? new char?((char) ((ushort) value.Value)) : null);
        }

        public static char? ToNullableChar(int? value)
        {
            return (value.HasValue ? new char?((char) ((ushort) value.Value)) : null);
        }

        public static char? ToNullableChar(long? value)
        {
            return (value.HasValue ? new char?((char) ((ushort) value.Value)) : null);
        }

        [CLSCompliant(false)]
        public static char? ToNullableChar(sbyte? value)
        {
            return (value.HasValue ? new char?((char) ((ushort) value.Value)) : null);
        }

        public static char? ToNullableChar(float? value)
        {
            return (value.HasValue ? new char?((char) ((ushort) value.Value)) : null);
        }

        [CLSCompliant(false)]
        public static char? ToNullableChar(ushort? value)
        {
            return (value.HasValue ? new char?(value.Value) : null);
        }

        [CLSCompliant(false)]
        public static char? ToNullableChar(uint? value)
        {
            return (value.HasValue ? new char?((char) ((ushort) value.Value)) : null);
        }

        [CLSCompliant(false)]
        public static char? ToNullableChar(ulong? value)
        {
            return (value.HasValue ? new char?((char) ((ushort) value.Value)) : null);
        }

        public static char? ToNullableChar(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is char)
            {
                return ToNullableChar((char) value);
            }
            if (value is string)
            {
                return ToNullableChar((string) value);
            }
            if (value is bool)
            {
                return ToNullableChar((bool) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(char?));
            }
            return new char?(((IConvertible) value).ToChar(null));
        }

        [CLSCompliant(false)]
        public static char? ToNullableChar(sbyte value)
        {
            return new char?((char) ((ushort) value));
        }

        public static char? ToNullableChar(float value)
        {
            return new char?((char) ((ushort) value));
        }

        public static char? ToNullableChar(string value)
        {
            char ch;
            if (char.TryParse(value, out ch))
            {
                return new char?(ch);
            }
            return '\0';
        }

        [CLSCompliant(false)]
        public static char? ToNullableChar(ushort value)
        {
            return new char?((char) value);
        }

        [CLSCompliant(false)]
        public static char? ToNullableChar(uint value)
        {
            return new char?((char) ((ushort) value));
        }

        [CLSCompliant(false)]
        public static char? ToNullableChar(ulong value)
        {
            return new char?((char) ((ushort) value));
        }

        public static DateTime? ToNullableDateTime(SqlDateTime value)
        {
            return (value.IsNull ? null : new DateTime?(value.Value));
        }

        public static DateTime? ToNullableDateTime(SqlDouble value)
        {
            return (value.IsNull ? null : new DateTime?(DateTime.MinValue + TimeSpan.FromDays(value.Value)));
        }

        public static DateTime? ToNullableDateTime(SqlInt64 value)
        {
            return (value.IsNull ? null : new DateTime?(DateTime.MinValue + TimeSpan.FromTicks(value.Value)));
        }

        public static DateTime? ToNullableDateTime(SqlString value)
        {
            return (value.IsNull ? null : new DateTime?(ToDateTime(value.Value)));
        }

        public static DateTime? ToNullableDateTime(DateTime value)
        {
            return new DateTime?(value);
        }

        public static DateTime? ToNullableDateTime(double value)
        {
            return new DateTime?(DateTime.MinValue + TimeSpan.FromDays(value));
        }

        public static DateTime? ToNullableDateTime(long value)
        {
            return new DateTime?(DateTime.MinValue + TimeSpan.FromTicks(value));
        }

        public static DateTime? ToNullableDateTime(double? value)
        {
            return (value.HasValue ? new DateTime?(DateTime.MinValue + TimeSpan.FromDays(value.Value)) : null);
        }

        public static DateTime? ToNullableDateTime(long? value)
        {
            return (value.HasValue ? new DateTime?(DateTime.MinValue + TimeSpan.FromTicks(value.Value)) : null);
        }

        public static DateTime? ToNullableDateTime(TimeSpan? value)
        {
            return (value.HasValue ? new DateTime?(DateTime.MinValue + value.Value) : null);
        }

        public static DateTime? ToNullableDateTime(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is DateTime)
            {
                return ToNullableDateTime((DateTime) value);
            }
            if (value is string)
            {
                return ToNullableDateTime((string) value);
            }
            if (value is TimeSpan)
            {
                return ToNullableDateTime((TimeSpan) value);
            }
            if (value is long)
            {
                return ToNullableDateTime((long) value);
            }
            if (value is double)
            {
                return ToNullableDateTime((double) value);
            }
            if (value is SqlDateTime)
            {
                return ToNullableDateTime((SqlDateTime) value);
            }
            if (value is SqlString)
            {
                return ToNullableDateTime((SqlString) value);
            }
            if (value is SqlInt64)
            {
                return ToNullableDateTime((SqlInt64) value);
            }
            if (value is SqlDouble)
            {
                return ToNullableDateTime((SqlDouble) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(DateTime?));
            }
            return new DateTime?(((IConvertible) value).ToDateTime(null));
        }

        public static DateTime? ToNullableDateTime(string value)
        {
            return ((value == null) ? null : new DateTime?(DateTime.Parse(value)));
        }

        public static DateTime? ToNullableDateTime(TimeSpan value)
        {
            return new DateTime?(DateTime.MinValue + value);
        }

        public static decimal? ToNullableDecimal(bool value)
        {
            return new decimal?(value ? 1.0M : 0.0M);
        }

        public static decimal? ToNullableDecimal(byte value)
        {
            return new decimal?(value);
        }

        public static decimal? ToNullableDecimal(char value)
        {
            return new decimal?(value);
        }

        public static decimal? ToNullableDecimal(SqlBoolean value)
        {
            return (value.IsNull ? null : ToNullableDecimal(value.Value));
        }

        public static decimal? ToNullableDecimal(SqlByte value)
        {
            return (value.IsNull ? null : ToNullableDecimal(value.Value));
        }

        public static decimal? ToNullableDecimal(SqlDecimal value)
        {
            return (value.IsNull ? null : new decimal?(value.Value));
        }

        public static decimal? ToNullableDecimal(SqlDouble value)
        {
            return (value.IsNull ? null : ToNullableDecimal(value.Value));
        }

        public static decimal? ToNullableDecimal(SqlInt16 value)
        {
            return (value.IsNull ? null : ToNullableDecimal(value.Value));
        }

        public static decimal? ToNullableDecimal(SqlInt32 value)
        {
            return (value.IsNull ? null : ToNullableDecimal(value.Value));
        }

        public static decimal? ToNullableDecimal(SqlInt64 value)
        {
            return (value.IsNull ? null : ToNullableDecimal(value.Value));
        }

        public static decimal? ToNullableDecimal(SqlMoney value)
        {
            return (value.IsNull ? null : new decimal?(value.Value));
        }

        public static decimal? ToNullableDecimal(SqlSingle value)
        {
            return (value.IsNull ? null : ToNullableDecimal(value.Value));
        }

        public static decimal? ToNullableDecimal(SqlString value)
        {
            return (value.IsNull ? null : ToNullableDecimal(value.Value));
        }

        public static decimal? ToNullableDecimal(decimal value)
        {
            return new decimal?(value);
        }

        public static decimal? ToNullableDecimal(double value)
        {
            return new decimal?((decimal) value);
        }

        public static decimal? ToNullableDecimal(short value)
        {
            return new decimal?(value);
        }

        public static decimal? ToNullableDecimal(int value)
        {
            return new decimal?(value);
        }

        public static decimal? ToNullableDecimal(long value)
        {
            return new decimal?(value);
        }

        public static decimal? ToNullableDecimal(bool? value)
        {
            return (value.HasValue ? new decimal?(value.Value ? 1.0M : 0.0M) : null);
        }

        public static decimal? ToNullableDecimal(byte? value)
        {
            return (value.HasValue ? new decimal?(value.Value) : null);
        }

        public static decimal? ToNullableDecimal(char? value)
        {
            return (value.HasValue ? new decimal?(value.Value) : null);
        }

        public static decimal? ToNullableDecimal(double? value)
        {
            return (value.HasValue ? new decimal?((decimal) value.Value) : null);
        }

        public static decimal? ToNullableDecimal(short? value)
        {
            return (value.HasValue ? new decimal?(value.Value) : null);
        }

        public static decimal? ToNullableDecimal(int? value)
        {
            return (value.HasValue ? new decimal?(value.Value) : null);
        }

        public static decimal? ToNullableDecimal(long? value)
        {
            return (value.HasValue ? new decimal?(value.Value) : null);
        }

        [CLSCompliant(false)]
        public static decimal? ToNullableDecimal(sbyte? value)
        {
            return (value.HasValue ? new decimal?(value.Value) : null);
        }

        public static decimal? ToNullableDecimal(float? value)
        {
            return (value.HasValue ? new decimal?((decimal) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static decimal? ToNullableDecimal(ushort? value)
        {
            return (value.HasValue ? new decimal?(value.Value) : null);
        }

        [CLSCompliant(false)]
        public static decimal? ToNullableDecimal(uint? value)
        {
            return (value.HasValue ? new decimal?(value.Value) : null);
        }

        [CLSCompliant(false)]
        public static decimal? ToNullableDecimal(ulong? value)
        {
            return (value.HasValue ? new decimal?(value.Value) : null);
        }

        public static decimal? ToNullableDecimal(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if ((value is double) && double.IsNaN((double) value))
            {
                return null;
            }
            if (value is decimal)
            {
                return ToNullableDecimal((decimal) value);
            }
            if (value is string)
            {
                return ToNullableDecimal((string) value);
            }
            if (value is char)
            {
                return ToNullableDecimal((char) value);
            }
            if (value is bool)
            {
                return ToNullableDecimal((bool) value);
            }
            if (value is SqlDecimal)
            {
                return ToNullableDecimal((SqlDecimal) value);
            }
            if (value is SqlMoney)
            {
                return ToNullableDecimal((SqlMoney) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(decimal?));
            }
            return new decimal?(((IConvertible) value).ToDecimal(null));
        }

        [CLSCompliant(false)]
        public static decimal? ToNullableDecimal(sbyte value)
        {
            return new decimal?(value);
        }

        public static decimal? ToNullableDecimal(float value)
        {
            return new decimal?((decimal) value);
        }

        public static decimal? ToNullableDecimal(string value)
        {
            return ((value == null) ? null : new decimal?(decimal.Parse(value)));
        }

        [CLSCompliant(false)]
        public static decimal? ToNullableDecimal(ushort value)
        {
            return new decimal?(value);
        }

        [CLSCompliant(false)]
        public static decimal? ToNullableDecimal(uint value)
        {
            return new decimal?(value);
        }

        [CLSCompliant(false)]
        public static decimal? ToNullableDecimal(ulong value)
        {
            return new decimal?(value);
        }

        public static double? ToNullableDouble(bool value)
        {
            return new double?(value ? 1.0 : 0.0);
        }

        public static double? ToNullableDouble(byte value)
        {
            return new double?((double) value);
        }

        public static double? ToNullableDouble(char value)
        {
            return new double?((double) value);
        }

        public static double? ToNullableDouble(SqlBoolean value)
        {
            return (value.IsNull ? null : ToNullableDouble(value.Value));
        }

        public static double? ToNullableDouble(SqlByte value)
        {
            return (value.IsNull ? null : ToNullableDouble(value.Value));
        }

        public static double? ToNullableDouble(SqlDateTime value)
        {
            return (value.IsNull ? null : new double?((value.Value - DateTime.MinValue).TotalDays));
        }

        public static double? ToNullableDouble(SqlDecimal value)
        {
            return (value.IsNull ? null : ToNullableDouble(value.Value));
        }

        public static double? ToNullableDouble(SqlDouble value)
        {
            return (value.IsNull ? null : new double?(value.Value));
        }

        public static double? ToNullableDouble(SqlInt16 value)
        {
            return (value.IsNull ? null : ToNullableDouble(value.Value));
        }

        public static double? ToNullableDouble(SqlInt32 value)
        {
            return (value.IsNull ? null : ToNullableDouble(value.Value));
        }

        public static double? ToNullableDouble(SqlInt64 value)
        {
            return (value.IsNull ? null : ToNullableDouble(value.Value));
        }

        public static double? ToNullableDouble(SqlMoney value)
        {
            return (value.IsNull ? null : ToNullableDouble(value.Value));
        }

        public static double? ToNullableDouble(SqlSingle value)
        {
            return (value.IsNull ? null : ToNullableDouble(value.Value));
        }

        public static double? ToNullableDouble(SqlString value)
        {
            return (value.IsNull ? null : ToNullableDouble(value.Value));
        }

        public static double? ToNullableDouble(DateTime value)
        {
            TimeSpan span = (TimeSpan) (value - DateTime.MinValue);
            return new double?(span.TotalDays);
        }

        public static double? ToNullableDouble(decimal value)
        {
            return new double?((double) value);
        }

        public static double? ToNullableDouble(double value)
        {
            return new double?(value);
        }

        public static double? ToNullableDouble(short value)
        {
            return new double?((double) value);
        }

        public static double? ToNullableDouble(int value)
        {
            return new double?((double) value);
        }

        public static double? ToNullableDouble(long value)
        {
            return new double?((double) value);
        }

        public static double? ToNullableDouble(bool? value)
        {
            return (value.HasValue ? new double?(value.Value ? 1.0 : 0.0) : null);
        }

        public static double? ToNullableDouble(byte? value)
        {
            return (value.HasValue ? new double?((double) value.Value) : null);
        }

        public static double? ToNullableDouble(char? value)
        {
            return (value.HasValue ? new double?((double) value.Value) : null);
        }

        public static double? ToNullableDouble(DateTime? value)
        {
            return (value.HasValue ? new double?((value.Value - DateTime.MinValue).TotalDays) : null);
        }

        public static double? ToNullableDouble(decimal? value)
        {
            return (value.HasValue ? new double?((double) value.Value) : null);
        }

        public static double? ToNullableDouble(short? value)
        {
            return (value.HasValue ? new double?((double) value.Value) : null);
        }

        public static double? ToNullableDouble(int? value)
        {
            return (value.HasValue ? new double?((double) value.Value) : null);
        }

        public static double? ToNullableDouble(long? value)
        {
            return (value.HasValue ? new double?((double) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static double? ToNullableDouble(sbyte? value)
        {
            return (value.HasValue ? new double?((double) value.Value) : null);
        }

        public static double? ToNullableDouble(float? value)
        {
            return (value.HasValue ? new double?((double) value.Value) : null);
        }

        public static double? ToNullableDouble(TimeSpan? value)
        {
            return (value.HasValue ? new double?(value.Value.TotalDays) : null);
        }

        [CLSCompliant(false)]
        public static double? ToNullableDouble(ushort? value)
        {
            return (value.HasValue ? new double?((double) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static double? ToNullableDouble(uint? value)
        {
            return (value.HasValue ? new double?((double) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static double? ToNullableDouble(ulong? value)
        {
            return (value.HasValue ? new double?((double) value.Value) : null);
        }

        public static double? ToNullableDouble(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is double)
            {
                return ToNullableDouble((double) value);
            }
            if (value is string)
            {
                return ToNullableDouble((string) value);
            }
            if (value is char)
            {
                return ToNullableDouble((char) value);
            }
            if (value is bool)
            {
                return ToNullableDouble((bool) value);
            }
            if (value is DateTime)
            {
                return ToNullableDouble((DateTime) value);
            }
            if (value is TimeSpan)
            {
                return ToNullableDouble((TimeSpan) value);
            }
            if (value is SqlDouble)
            {
                return ToNullableDouble((SqlDouble) value);
            }
            if (value is SqlDateTime)
            {
                return ToNullableDouble((SqlDateTime) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(double?));
            }
            return new double?(((IConvertible) value).ToDouble(null));
        }

        [CLSCompliant(false)]
        public static double? ToNullableDouble(sbyte value)
        {
            return new double?((double) value);
        }

        public static double? ToNullableDouble(float value)
        {
            return new double?((double) value);
        }

        public static double? ToNullableDouble(string value)
        {
            return ((value == null) ? null : new double?(double.Parse(value)));
        }

        public static double? ToNullableDouble(TimeSpan value)
        {
            return new double?(value.TotalDays);
        }

        [CLSCompliant(false)]
        public static double? ToNullableDouble(ushort value)
        {
            return new double?((double) value);
        }

        [CLSCompliant(false)]
        public static double? ToNullableDouble(uint value)
        {
            return new double?((double) value);
        }

        [CLSCompliant(false)]
        public static double? ToNullableDouble(ulong value)
        {
            return new double?((double) value);
        }

        public static Guid? ToNullableGuid(SqlBinary value)
        {
            return (value.IsNull ? null : new Guid?(value.ToSqlGuid().Value));
        }

        public static Guid? ToNullableGuid(SqlGuid value)
        {
            return (value.IsNull ? null : new Guid?(value.Value));
        }

        public static Guid? ToNullableGuid(SqlString value)
        {
            return (value.IsNull ? null : ((Guid?) new Guid(value.Value)));
        }

        public static Guid? ToNullableGuid(Guid value)
        {
            return new Guid?(value);
        }

        public static Guid? ToNullableGuid(string value)
        {
            return ((value == null) ? null : ((Guid?) new Guid(value)));
        }

        public static Guid? ToNullableGuid(Type value)
        {
            return ((value == null) ? null : new Guid?(value.GUID));
        }

        public static Guid? ToNullableGuid(byte[] value)
        {
            return ((value == null) ? null : ((Guid?) new Guid(value)));
        }

        public static Guid? ToNullableGuid(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is Guid)
            {
                return ToNullableGuid((Guid) value);
            }
            if (value is string)
            {
                return ToNullableGuid((string) value);
            }
            if (value is SqlGuid)
            {
                return ToNullableGuid((SqlGuid) value);
            }
            if (value is SqlString)
            {
                return ToNullableGuid((SqlString) value);
            }
            if (value is SqlBinary)
            {
                return ToNullableGuid((SqlBinary) value);
            }
            if (value is Type)
            {
                return ToNullableGuid((Type) value);
            }
            if (!(value is byte[]))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(Guid?));
            }
            return ToNullableGuid((byte[]) value);
        }

        public static short? ToNullableInt16(bool value)
        {
            return new short?(value ? ((short) 1) : ((short) 0));
        }

        public static short? ToNullableInt16(byte value)
        {
            return new short?(value);
        }

        public static short? ToNullableInt16(char value)
        {
            return new short?((short) value);
        }

        public static short? ToNullableInt16(SqlBoolean value)
        {
            return (value.IsNull ? null : ToNullableInt16(value.Value));
        }

        public static short? ToNullableInt16(SqlByte value)
        {
            return (value.IsNull ? null : ToNullableInt16(value.Value));
        }

        public static short? ToNullableInt16(SqlDecimal value)
        {
            return (value.IsNull ? null : ToNullableInt16(value.Value));
        }

        public static short? ToNullableInt16(SqlDouble value)
        {
            return (value.IsNull ? null : ToNullableInt16(value.Value));
        }

        public static short? ToNullableInt16(SqlInt16 value)
        {
            return (value.IsNull ? null : new short?(value.Value));
        }

        public static short? ToNullableInt16(SqlInt32 value)
        {
            return (value.IsNull ? null : ToNullableInt16(value.Value));
        }

        public static short? ToNullableInt16(SqlInt64 value)
        {
            return (value.IsNull ? null : ToNullableInt16(value.Value));
        }

        public static short? ToNullableInt16(SqlMoney value)
        {
            return (value.IsNull ? null : ToNullableInt16(value.Value));
        }

        public static short? ToNullableInt16(SqlSingle value)
        {
            return (value.IsNull ? null : ToNullableInt16(value.Value));
        }

        public static short? ToNullableInt16(SqlString value)
        {
            return (value.IsNull ? null : ToNullableInt16(value.Value));
        }

        public static short? ToNullableInt16(decimal value)
        {
            return new short?((short) value);
        }

        public static short? ToNullableInt16(double value)
        {
            return new short?((short) value);
        }

        public static short? ToNullableInt16(short value)
        {
            return new short?(value);
        }

        public static short? ToNullableInt16(int value)
        {
            return new short?((short) value);
        }

        public static short? ToNullableInt16(long value)
        {
            return new short?((short) value);
        }

        public static short? ToNullableInt16(bool? value)
        {
            return (value.HasValue ? new short?(value.Value ? ((short) 1) : ((short) 0)) : null);
        }

        public static short? ToNullableInt16(byte? value)
        {
            return (value.HasValue ? new short?(value.Value) : null);
        }

        public static short? ToNullableInt16(char? value)
        {
            return (value.HasValue ? new short?((short) value.Value) : null);
        }

        public static short? ToNullableInt16(decimal? value)
        {
            return (value.HasValue ? new short?((short) value.Value) : null);
        }

        public static short? ToNullableInt16(double? value)
        {
            return (value.HasValue ? new short?((short) value.Value) : null);
        }

        public static short? ToNullableInt16(int? value)
        {
            return (value.HasValue ? new short?((short) value.Value) : null);
        }

        public static short? ToNullableInt16(long? value)
        {
            return (value.HasValue ? new short?((short) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static short? ToNullableInt16(sbyte? value)
        {
            return (value.HasValue ? new short?(value.Value) : null);
        }

        public static short? ToNullableInt16(float? value)
        {
            return (value.HasValue ? new short?((short) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static short? ToNullableInt16(ushort? value)
        {
            return (value.HasValue ? new short?((short) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static short? ToNullableInt16(uint? value)
        {
            return (value.HasValue ? new short?((short) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static short? ToNullableInt16(ulong? value)
        {
            return (value.HasValue ? new short?((short) value.Value) : null);
        }

        public static short? ToNullableInt16(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is short)
            {
                return ToNullableInt16((short) value);
            }
            if (value is string)
            {
                return ToNullableInt16((string) value);
            }
            if (value is char)
            {
                return ToNullableInt16((char) value);
            }
            if (value is bool)
            {
                return ToNullableInt16((bool) value);
            }
            if (value is SqlInt16)
            {
                return ToNullableInt16((SqlInt16) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(short?));
            }
            return new short?(((IConvertible) value).ToInt16(null));
        }

        [CLSCompliant(false)]
        public static short? ToNullableInt16(sbyte value)
        {
            return new short?(value);
        }

        public static short? ToNullableInt16(float value)
        {
            return new short?((short) value);
        }

        public static short? ToNullableInt16(string value)
        {
            return ((value == null) ? null : new short?(short.Parse(value)));
        }

        [CLSCompliant(false)]
        public static short? ToNullableInt16(ushort value)
        {
            return new short?((short) value);
        }

        [CLSCompliant(false)]
        public static short? ToNullableInt16(uint value)
        {
            return new short?((short) value);
        }

        [CLSCompliant(false)]
        public static short? ToNullableInt16(ulong value)
        {
            return new short?((short) value);
        }

        public static int? ToNullableInt32(bool value)
        {
            return new int?(value ? 1 : 0);
        }

        public static int? ToNullableInt32(byte value)
        {
            return new int?(value);
        }

        public static int? ToNullableInt32(char value)
        {
            return new int?(value);
        }

        public static int? ToNullableInt32(SqlBoolean value)
        {
            return (value.IsNull ? null : ToNullableInt32(value.Value));
        }

        public static int? ToNullableInt32(SqlByte value)
        {
            return (value.IsNull ? null : ToNullableInt32(value.Value));
        }

        public static int? ToNullableInt32(SqlDecimal value)
        {
            return (value.IsNull ? null : ToNullableInt32(value.Value));
        }

        public static int? ToNullableInt32(SqlDouble value)
        {
            return (value.IsNull ? null : ToNullableInt32(value.Value));
        }

        public static int? ToNullableInt32(SqlInt16 value)
        {
            return (value.IsNull ? null : ToNullableInt32(value.Value));
        }

        public static int? ToNullableInt32(SqlInt32 value)
        {
            return (value.IsNull ? null : new int?(value.Value));
        }

        public static int? ToNullableInt32(SqlInt64 value)
        {
            return (value.IsNull ? null : ToNullableInt32(value.Value));
        }

        public static int? ToNullableInt32(SqlMoney value)
        {
            return (value.IsNull ? null : ToNullableInt32(value.Value));
        }

        public static int? ToNullableInt32(SqlSingle value)
        {
            return (value.IsNull ? null : ToNullableInt32(value.Value));
        }

        public static int? ToNullableInt32(SqlString value)
        {
            return (value.IsNull ? null : ToNullableInt32(value.Value));
        }

        public static int? ToNullableInt32(decimal value)
        {
            return new int?((int) value);
        }

        public static int? ToNullableInt32(double value)
        {
            return new int?((int) value);
        }

        public static int? ToNullableInt32(short value)
        {
            return new int?(value);
        }

        public static int? ToNullableInt32(int value)
        {
            return new int?(value);
        }

        public static int? ToNullableInt32(long value)
        {
            return new int?((int) value);
        }

        public static int? ToNullableInt32(bool? value)
        {
            return (value.HasValue ? new int?(value.Value ? 1 : 0) : null);
        }

        public static int? ToNullableInt32(byte? value)
        {
            return (value.HasValue ? new int?(value.Value) : null);
        }

        public static int? ToNullableInt32(char? value)
        {
            return (value.HasValue ? new int?(value.Value) : null);
        }

        public static int? ToNullableInt32(decimal? value)
        {
            return (value.HasValue ? new int?((int) value.Value) : null);
        }

        public static int? ToNullableInt32(double? value)
        {
            return (value.HasValue ? new int?((int) value.Value) : null);
        }

        public static int? ToNullableInt32(short? value)
        {
            return (value.HasValue ? new int?(value.Value) : null);
        }

        public static int? ToNullableInt32(long? value)
        {
            return (value.HasValue ? new int?((int) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static int? ToNullableInt32(sbyte? value)
        {
            return (value.HasValue ? new int?(value.Value) : null);
        }

        public static int? ToNullableInt32(float? value)
        {
            return (value.HasValue ? new int?((int) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static int? ToNullableInt32(ushort? value)
        {
            return (value.HasValue ? new int?(value.Value) : null);
        }

        [CLSCompliant(false)]
        public static int? ToNullableInt32(uint? value)
        {
            return (value.HasValue ? new int?((int) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static int? ToNullableInt32(ulong? value)
        {
            return (value.HasValue ? new int?((int) value.Value) : null);
        }

        public static int? ToNullableInt32(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is int)
            {
                return ToNullableInt32((int) value);
            }
            if (value is string)
            {
                return ToNullableInt32((string) value);
            }
            if (value is char)
            {
                return ToNullableInt32((char) value);
            }
            if (value is bool)
            {
                return ToNullableInt32((bool) value);
            }
            if (value is SqlInt32)
            {
                return ToNullableInt32((SqlInt32) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(int?));
            }
            return new int?(((IConvertible) value).ToInt32(null));
        }

        [CLSCompliant(false)]
        public static int? ToNullableInt32(sbyte value)
        {
            return new int?(value);
        }

        public static int? ToNullableInt32(float value)
        {
            return new int?((int) value);
        }

        public static int? ToNullableInt32(string value)
        {
            return ((value == null) ? null : new int?(int.Parse(value)));
        }

        [CLSCompliant(false)]
        public static int? ToNullableInt32(ushort value)
        {
            return new int?(value);
        }

        [CLSCompliant(false)]
        public static int? ToNullableInt32(uint value)
        {
            return new int?((int) value);
        }

        [CLSCompliant(false)]
        public static int? ToNullableInt32(ulong value)
        {
            return new int?((int) value);
        }

        public static long? ToNullableInt64(bool value)
        {
            return new long?(value ? ((long) 1) : ((long) 0));
        }

        public static long? ToNullableInt64(byte value)
        {
            return new long?((long) value);
        }

        public static long? ToNullableInt64(char value)
        {
            return new long?((long) value);
        }

        public static long? ToNullableInt64(SqlBoolean value)
        {
            return (value.IsNull ? null : ToNullableInt64(value.Value));
        }

        public static long? ToNullableInt64(SqlByte value)
        {
            return (value.IsNull ? null : ToNullableInt64(value.Value));
        }

        public static long? ToNullableInt64(SqlDateTime value)
        {
            return (value.IsNull ? null : ToNullableInt64(value.Value));
        }

        public static long? ToNullableInt64(SqlDecimal value)
        {
            return (value.IsNull ? null : ToNullableInt64(value.Value));
        }

        public static long? ToNullableInt64(SqlDouble value)
        {
            return (value.IsNull ? null : ToNullableInt64(value.Value));
        }

        public static long? ToNullableInt64(SqlInt16 value)
        {
            return (value.IsNull ? null : ToNullableInt64(value.Value));
        }

        public static long? ToNullableInt64(SqlInt32 value)
        {
            return (value.IsNull ? null : ToNullableInt64(value.Value));
        }

        public static long? ToNullableInt64(SqlInt64 value)
        {
            return (value.IsNull ? null : new long?(value.Value));
        }

        public static long? ToNullableInt64(SqlMoney value)
        {
            return (value.IsNull ? null : ToNullableInt64(value.Value));
        }

        public static long? ToNullableInt64(SqlSingle value)
        {
            return (value.IsNull ? null : ToNullableInt64(value.Value));
        }

        public static long? ToNullableInt64(SqlString value)
        {
            return (value.IsNull ? null : ToNullableInt64(value.Value));
        }

        public static long? ToNullableInt64(DateTime value)
        {
            TimeSpan span = (TimeSpan) (value - DateTime.MinValue);
            return new long?(span.Ticks);
        }

        public static long? ToNullableInt64(decimal value)
        {
            return new long?((long) value);
        }

        public static long? ToNullableInt64(double value)
        {
            return new long?((long) value);
        }

        public static long? ToNullableInt64(short value)
        {
            return new long?((long) value);
        }

        public static long? ToNullableInt64(int value)
        {
            return new long?((long) value);
        }

        public static long? ToNullableInt64(long value)
        {
            return new long?(value);
        }

        public static long? ToNullableInt64(bool? value)
        {
            return (value.HasValue ? new long?(value.Value ? ((long) 1) : ((long) 0)) : null);
        }

        public static long? ToNullableInt64(byte? value)
        {
            return (value.HasValue ? new long?((long) ((ulong) value.Value)) : null);
        }

        public static long? ToNullableInt64(char? value)
        {
            return (value.HasValue ? new long?((long) value.Value) : null);
        }

        public static long? ToNullableInt64(DateTime? value)
        {
            return (value.HasValue ? new long?((value.Value - DateTime.MinValue).Ticks) : null);
        }

        public static long? ToNullableInt64(decimal? value)
        {
            return (value.HasValue ? new long?((long) value.Value) : null);
        }

        public static long? ToNullableInt64(double? value)
        {
            return (value.HasValue ? new long?((long) value.Value) : null);
        }

        public static long? ToNullableInt64(short? value)
        {
            return (value.HasValue ? new long?((long) value.Value) : null);
        }

        public static long? ToNullableInt64(int? value)
        {
            return (value.HasValue ? new long?((long) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static long? ToNullableInt64(sbyte? value)
        {
            return (value.HasValue ? new long?((long) value.Value) : null);
        }

        public static long? ToNullableInt64(float? value)
        {
            return (value.HasValue ? new long?((long) value.Value) : null);
        }

        public static long? ToNullableInt64(TimeSpan? value)
        {
            return (value.HasValue ? new long?(value.Value.Ticks) : null);
        }

        [CLSCompliant(false)]
        public static long? ToNullableInt64(ushort? value)
        {
            return (value.HasValue ? new long?((long) ((ulong) value.Value)) : null);
        }

        [CLSCompliant(false)]
        public static long? ToNullableInt64(uint? value)
        {
            return (value.HasValue ? new long?((long) ((ulong) value.Value)) : null);
        }

        [CLSCompliant(false)]
        public static long? ToNullableInt64(ulong? value)
        {
            return (value.HasValue ? new long?((long) value.Value) : null);
        }

        public static long? ToNullableInt64(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is long)
            {
                return ToNullableInt64((long) value);
            }
            if (value is string)
            {
                return ToNullableInt64((string) value);
            }
            if (value is char)
            {
                return ToNullableInt64((char) value);
            }
            if (value is bool)
            {
                return ToNullableInt64((bool) value);
            }
            if (value is DateTime)
            {
                return ToNullableInt64((DateTime) value);
            }
            if (value is TimeSpan)
            {
                return ToNullableInt64((TimeSpan) value);
            }
            if (value is SqlInt64)
            {
                return ToNullableInt64((SqlInt64) value);
            }
            if (value is SqlDateTime)
            {
                return ToNullableInt64((SqlDateTime) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(long?));
            }
            return new long?(((IConvertible) value).ToInt64(null));
        }

        [CLSCompliant(false)]
        public static long? ToNullableInt64(sbyte value)
        {
            return new long?((long) value);
        }

        public static long? ToNullableInt64(float value)
        {
            return new long?((long) value);
        }

        public static long? ToNullableInt64(string value)
        {
            return ((value == null) ? null : new long?(long.Parse(value)));
        }

        public static long? ToNullableInt64(TimeSpan value)
        {
            return new long?(value.Ticks);
        }

        [CLSCompliant(false)]
        public static long? ToNullableInt64(ushort value)
        {
            return new long?((long) value);
        }

        [CLSCompliant(false)]
        public static long? ToNullableInt64(uint value)
        {
            return new long?((long) value);
        }

        [CLSCompliant(false)]
        public static long? ToNullableInt64(ulong value)
        {
            return new long?((long) value);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(bool value)
        {
            return new sbyte?(value ? ((sbyte) 1) : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(byte value)
        {
            return new sbyte?((sbyte) value);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(char value)
        {
            return new sbyte?((sbyte) value);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(SqlBoolean value)
        {
            return (value.IsNull ? null : ToNullableSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(SqlByte value)
        {
            return (value.IsNull ? null : ToNullableSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(SqlDecimal value)
        {
            return (value.IsNull ? null : ToNullableSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(SqlDouble value)
        {
            return (value.IsNull ? null : ToNullableSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(SqlInt16 value)
        {
            return (value.IsNull ? null : ToNullableSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(SqlInt32 value)
        {
            return (value.IsNull ? null : ToNullableSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(SqlInt64 value)
        {
            return (value.IsNull ? null : ToNullableSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(SqlMoney value)
        {
            return (value.IsNull ? null : ToNullableSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(SqlSingle value)
        {
            return (value.IsNull ? null : ToNullableSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(SqlString value)
        {
            return (value.IsNull ? null : ToNullableSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(decimal value)
        {
            return new sbyte?((sbyte) value);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(double value)
        {
            return new sbyte?((sbyte) value);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(short value)
        {
            return new sbyte?((sbyte) value);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(int value)
        {
            return new sbyte?((sbyte) value);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(long value)
        {
            return new sbyte?((sbyte) value);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(bool? value)
        {
            return (value.HasValue ? new sbyte?(value.Value ? ((sbyte) 1) : ((sbyte) 0)) : null);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(byte? value)
        {
            return (value.HasValue ? new sbyte?((sbyte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(char? value)
        {
            return (value.HasValue ? new sbyte?((sbyte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(decimal? value)
        {
            return (value.HasValue ? new sbyte?((sbyte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(double? value)
        {
            return (value.HasValue ? new sbyte?((sbyte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(short? value)
        {
            return (value.HasValue ? new sbyte?((sbyte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(int? value)
        {
            return (value.HasValue ? new sbyte?((sbyte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(long? value)
        {
            return (value.HasValue ? new sbyte?((sbyte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(float? value)
        {
            return (value.HasValue ? new sbyte?((sbyte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(ushort? value)
        {
            return (value.HasValue ? new sbyte?((sbyte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(uint? value)
        {
            return (value.HasValue ? new sbyte?((sbyte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(ulong? value)
        {
            return (value.HasValue ? new sbyte?((sbyte) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is sbyte)
            {
                return ToNullableSByte((sbyte) value);
            }
            if (value is string)
            {
                return ToNullableSByte((string) value);
            }
            if (value is char)
            {
                return ToNullableSByte((char) value);
            }
            if (value is bool)
            {
                return ToNullableSByte((bool) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(sbyte?));
            }
            return new sbyte?(((IConvertible) value).ToSByte(null));
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(sbyte value)
        {
            return new sbyte?(value);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(float value)
        {
            return new sbyte?((sbyte) value);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(string value)
        {
            return ((value == null) ? null : new sbyte?(sbyte.Parse(value)));
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(ushort value)
        {
            return new sbyte?((sbyte) value);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(uint value)
        {
            return new sbyte?((sbyte) value);
        }

        [CLSCompliant(false)]
        public static sbyte? ToNullableSByte(ulong value)
        {
            return new sbyte?((sbyte) value);
        }

        public static float? ToNullableSingle(bool value)
        {
            return new float?(value ? 1f : 0f);
        }

        public static float? ToNullableSingle(byte value)
        {
            return new float?((float) value);
        }

        public static float? ToNullableSingle(char value)
        {
            return new float?((float) value);
        }

        public static float? ToNullableSingle(SqlBoolean value)
        {
            return (value.IsNull ? null : ToNullableSingle(value.Value));
        }

        public static float? ToNullableSingle(SqlByte value)
        {
            return (value.IsNull ? null : ToNullableSingle(value.Value));
        }

        public static float? ToNullableSingle(SqlDecimal value)
        {
            return (value.IsNull ? null : ToNullableSingle(value.Value));
        }

        public static float? ToNullableSingle(SqlDouble value)
        {
            return (value.IsNull ? null : ToNullableSingle(value.Value));
        }

        public static float? ToNullableSingle(SqlInt16 value)
        {
            return (value.IsNull ? null : ToNullableSingle(value.Value));
        }

        public static float? ToNullableSingle(SqlInt32 value)
        {
            return (value.IsNull ? null : ToNullableSingle(value.Value));
        }

        public static float? ToNullableSingle(SqlInt64 value)
        {
            return (value.IsNull ? null : ToNullableSingle(value.Value));
        }

        public static float? ToNullableSingle(SqlMoney value)
        {
            return (value.IsNull ? null : ToNullableSingle(value.Value));
        }

        public static float? ToNullableSingle(SqlSingle value)
        {
            return (value.IsNull ? null : new float?(value.Value));
        }

        public static float? ToNullableSingle(SqlString value)
        {
            return (value.IsNull ? null : ToNullableSingle(value.Value));
        }

        public static float? ToNullableSingle(decimal value)
        {
            return new float?((float) value);
        }

        public static float? ToNullableSingle(double value)
        {
            return new float?((float) value);
        }

        public static float? ToNullableSingle(short value)
        {
            return new float?((float) value);
        }

        public static float? ToNullableSingle(int value)
        {
            return new float?((float) value);
        }

        public static float? ToNullableSingle(long value)
        {
            return new float?((float) value);
        }

        public static float? ToNullableSingle(bool? value)
        {
            return (value.HasValue ? new float?(value.Value ? 1f : 0f) : null);
        }

        public static float? ToNullableSingle(byte? value)
        {
            return (value.HasValue ? new float?((float) value.Value) : null);
        }

        public static float? ToNullableSingle(char? value)
        {
            return (value.HasValue ? new float?((float) value.Value) : null);
        }

        public static float? ToNullableSingle(decimal? value)
        {
            return (value.HasValue ? new float?((float) value.Value) : null);
        }

        public static float? ToNullableSingle(double? value)
        {
            return (value.HasValue ? new float?((float) value.Value) : null);
        }

        public static float? ToNullableSingle(short? value)
        {
            return (value.HasValue ? new float?((float) value.Value) : null);
        }

        public static float? ToNullableSingle(int? value)
        {
            return (value.HasValue ? new float?((float) value.Value) : null);
        }

        public static float? ToNullableSingle(long? value)
        {
            return (value.HasValue ? new float?((float) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static float? ToNullableSingle(sbyte? value)
        {
            return (value.HasValue ? new float?((float) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static float? ToNullableSingle(ushort? value)
        {
            return (value.HasValue ? new float?((float) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static float? ToNullableSingle(uint? value)
        {
            return (value.HasValue ? new float?((float) ((double) value.Value)) : null);
        }

        [CLSCompliant(false)]
        public static float? ToNullableSingle(ulong? value)
        {
            return (value.HasValue ? new float?((float) ((double) value.Value)) : null);
        }

        public static float? ToNullableSingle(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is float)
            {
                return ToNullableSingle((float) value);
            }
            if (value is string)
            {
                return ToNullableSingle((string) value);
            }
            if (value is char)
            {
                return ToNullableSingle((char) value);
            }
            if (value is bool)
            {
                return ToNullableSingle((bool) value);
            }
            if (value is SqlSingle)
            {
                return ToNullableSingle((SqlSingle) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(float?));
            }
            return new float?(((IConvertible) value).ToSingle(null));
        }

        [CLSCompliant(false)]
        public static float? ToNullableSingle(sbyte value)
        {
            return new float?((float) value);
        }

        public static float? ToNullableSingle(float value)
        {
            return new float?(value);
        }

        public static float? ToNullableSingle(string value)
        {
            return ((value == null) ? null : new float?(float.Parse(value)));
        }

        [CLSCompliant(false)]
        public static float? ToNullableSingle(ushort value)
        {
            return new float?((float) value);
        }

        [CLSCompliant(false)]
        public static float? ToNullableSingle(uint value)
        {
            return new float?((float) value);
        }

        [CLSCompliant(false)]
        public static float? ToNullableSingle(ulong value)
        {
            return new float?((float) value);
        }

        public static TimeSpan? ToNullableTimeSpan(SqlDateTime value)
        {
            return (value.IsNull ? null : new TimeSpan?((TimeSpan) (value.Value - DateTime.MinValue)));
        }

        public static TimeSpan? ToNullableTimeSpan(SqlDouble value)
        {
            return (value.IsNull ? null : new TimeSpan?(TimeSpan.FromDays(value.Value)));
        }

        public static TimeSpan? ToNullableTimeSpan(SqlInt64 value)
        {
            return (value.IsNull ? null : new TimeSpan?(TimeSpan.FromTicks(value.Value)));
        }

        public static TimeSpan? ToNullableTimeSpan(SqlString value)
        {
            return (value.IsNull ? null : new TimeSpan?(TimeSpan.Parse(value.Value)));
        }

        public static TimeSpan? ToNullableTimeSpan(DateTime value)
        {
            return new TimeSpan?((TimeSpan) (value - DateTime.MinValue));
        }

        public static TimeSpan? ToNullableTimeSpan(double value)
        {
            return new TimeSpan?(TimeSpan.FromDays(value));
        }

        public static TimeSpan? ToNullableTimeSpan(long value)
        {
            return new TimeSpan?(TimeSpan.FromTicks(value));
        }

        public static TimeSpan? ToNullableTimeSpan(DateTime? value)
        {
            return (value.HasValue ? new TimeSpan?(value.Value - DateTime.MinValue) : null);
        }

        public static TimeSpan? ToNullableTimeSpan(double? value)
        {
            return (value.HasValue ? new TimeSpan?(TimeSpan.FromDays(value.Value)) : null);
        }

        public static TimeSpan? ToNullableTimeSpan(long? value)
        {
            return (value.HasValue ? new TimeSpan?(TimeSpan.FromTicks(value.Value)) : null);
        }

        public static TimeSpan? ToNullableTimeSpan(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is TimeSpan)
            {
                return ToNullableTimeSpan((TimeSpan) value);
            }
            if (value is string)
            {
                return ToNullableTimeSpan((string) value);
            }
            if (value is DateTime)
            {
                return ToNullableTimeSpan((DateTime) value);
            }
            if (value is long)
            {
                return ToNullableTimeSpan((long) value);
            }
            if (value is double)
            {
                return ToNullableTimeSpan((double) value);
            }
            if (value is SqlString)
            {
                return ToNullableTimeSpan((SqlString) value);
            }
            if (value is SqlDateTime)
            {
                return ToNullableTimeSpan((SqlDateTime) value);
            }
            if (value is SqlInt64)
            {
                return ToNullableTimeSpan((SqlInt64) value);
            }
            if (!(value is SqlDouble))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(TimeSpan?));
            }
            return ToNullableTimeSpan((SqlDouble) value);
        }

        public static TimeSpan? ToNullableTimeSpan(string value)
        {
            return ((value == null) ? null : new TimeSpan?(TimeSpan.Parse(value)));
        }

        public static TimeSpan? ToNullableTimeSpan(TimeSpan value)
        {
            return new TimeSpan?(value);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(bool value)
        {
            return new ushort?(value ? ((ushort) 1) : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(byte value)
        {
            return new ushort?(value);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(char value)
        {
            return new ushort?(value);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(SqlBoolean value)
        {
            return (value.IsNull ? null : ToNullableUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(SqlByte value)
        {
            return (value.IsNull ? null : ToNullableUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(SqlDecimal value)
        {
            return (value.IsNull ? null : ToNullableUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(SqlDouble value)
        {
            return (value.IsNull ? null : ToNullableUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(SqlInt16 value)
        {
            return (value.IsNull ? null : ToNullableUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(SqlInt32 value)
        {
            return (value.IsNull ? null : ToNullableUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(SqlInt64 value)
        {
            return (value.IsNull ? null : ToNullableUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(SqlMoney value)
        {
            return (value.IsNull ? null : ToNullableUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(SqlSingle value)
        {
            return (value.IsNull ? null : ToNullableUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(SqlString value)
        {
            return (value.IsNull ? null : ToNullableUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(decimal value)
        {
            return new ushort?((ushort) value);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(double value)
        {
            return new ushort?((ushort) value);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(short value)
        {
            return new ushort?((ushort) value);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(int value)
        {
            return new ushort?((ushort) value);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(long value)
        {
            return new ushort?((ushort) value);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(bool? value)
        {
            return (value.HasValue ? new ushort?(value.Value ? ((ushort) 1) : ((ushort) 0)) : null);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(byte? value)
        {
            return (value.HasValue ? new ushort?(value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(char? value)
        {
            return (value.HasValue ? new ushort?(value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(decimal? value)
        {
            return (value.HasValue ? new ushort?((ushort) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(double? value)
        {
            return (value.HasValue ? new ushort?((ushort) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(short? value)
        {
            return (value.HasValue ? new ushort?((ushort) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(int? value)
        {
            return (value.HasValue ? new ushort?((ushort) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(long? value)
        {
            return (value.HasValue ? new ushort?((ushort) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(sbyte? value)
        {
            return (value.HasValue ? new ushort?((ushort) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(float? value)
        {
            return (value.HasValue ? new ushort?((ushort) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(uint? value)
        {
            return (value.HasValue ? new ushort?((ushort) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(ulong? value)
        {
            return (value.HasValue ? new ushort?((ushort) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is ushort)
            {
                return ToNullableUInt16((ushort) value);
            }
            if (value is string)
            {
                return ToNullableUInt16((string) value);
            }
            if (value is char)
            {
                return ToNullableUInt16((char) value);
            }
            if (value is bool)
            {
                return ToNullableUInt16((bool) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(ushort?));
            }
            return new ushort?(((IConvertible) value).ToUInt16(null));
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(sbyte value)
        {
            return new ushort?((ushort) value);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(float value)
        {
            return new ushort?((ushort) value);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(string value)
        {
            return ((value == null) ? null : new ushort?(ushort.Parse(value)));
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(ushort value)
        {
            return new ushort?(value);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(uint value)
        {
            return new ushort?((ushort) value);
        }

        [CLSCompliant(false)]
        public static ushort? ToNullableUInt16(ulong value)
        {
            return new ushort?((ushort) value);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(bool value)
        {
            return new uint?(value ? 1 : 0);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(byte value)
        {
            return new uint?(value);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(char value)
        {
            return new uint?(value);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(SqlBoolean value)
        {
            return (value.IsNull ? null : ToNullableUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(SqlByte value)
        {
            return (value.IsNull ? null : ToNullableUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(SqlDecimal value)
        {
            return (value.IsNull ? null : ToNullableUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(SqlDouble value)
        {
            return (value.IsNull ? null : ToNullableUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(SqlInt16 value)
        {
            return (value.IsNull ? null : ToNullableUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(SqlInt32 value)
        {
            return (value.IsNull ? null : ToNullableUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(SqlInt64 value)
        {
            return (value.IsNull ? null : ToNullableUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(SqlMoney value)
        {
            return (value.IsNull ? null : ToNullableUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(SqlSingle value)
        {
            return (value.IsNull ? null : ToNullableUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(SqlString value)
        {
            return (value.IsNull ? null : ToNullableUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(decimal value)
        {
            return new uint?((uint) value);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(double value)
        {
            return new uint?((uint) value);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(short value)
        {
            return new uint?((uint) value);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(int value)
        {
            return new uint?((uint) value);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(long value)
        {
            return new uint?((uint) value);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(bool? value)
        {
            return (value.HasValue ? new uint?(value.Value ? 1 : 0) : null);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(byte? value)
        {
            return (value.HasValue ? new uint?(value.Value) : null);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(char? value)
        {
            return (value.HasValue ? new uint?(value.Value) : null);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(decimal? value)
        {
            return (value.HasValue ? new uint?((uint) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(double? value)
        {
            return (value.HasValue ? new uint?((uint) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(short? value)
        {
            return (value.HasValue ? new uint?((uint) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(int? value)
        {
            return (value.HasValue ? new uint?((uint) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(long? value)
        {
            return (value.HasValue ? new uint?((uint) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(sbyte? value)
        {
            return (value.HasValue ? new uint?((uint) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(float? value)
        {
            return (value.HasValue ? new uint?((uint) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(ushort? value)
        {
            return (value.HasValue ? new uint?(value.Value) : null);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(ulong? value)
        {
            return (value.HasValue ? new uint?((uint) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is uint)
            {
                return ToNullableUInt32((uint) value);
            }
            if (value is string)
            {
                return ToNullableUInt32((string) value);
            }
            if (value is char)
            {
                return ToNullableUInt32((char) value);
            }
            if (value is bool)
            {
                return ToNullableUInt32((bool) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(uint?));
            }
            return new uint?(((IConvertible) value).ToUInt32(null));
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(sbyte value)
        {
            return new uint?((uint) value);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(float value)
        {
            return new uint?((uint) value);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(string value)
        {
            return ((value == null) ? null : new uint?(uint.Parse(value)));
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(ushort value)
        {
            return new uint?(value);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(uint value)
        {
            return new uint?(value);
        }

        [CLSCompliant(false)]
        public static uint? ToNullableUInt32(ulong value)
        {
            return new uint?((uint) value);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(bool value)
        {
            return new ulong?(value ? ((ulong) 1) : ((ulong) 0));
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(byte value)
        {
            return new ulong?((ulong) value);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(char value)
        {
            return new ulong?((ulong) value);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(SqlBoolean value)
        {
            return (value.IsNull ? null : ToNullableUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(SqlByte value)
        {
            return (value.IsNull ? null : ToNullableUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(SqlDecimal value)
        {
            return (value.IsNull ? null : ToNullableUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(SqlDouble value)
        {
            return (value.IsNull ? null : ToNullableUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(SqlInt16 value)
        {
            return (value.IsNull ? null : ToNullableUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(SqlInt32 value)
        {
            return (value.IsNull ? null : ToNullableUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(SqlInt64 value)
        {
            return (value.IsNull ? null : ToNullableUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(SqlMoney value)
        {
            return (value.IsNull ? null : ToNullableUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(SqlSingle value)
        {
            return (value.IsNull ? null : ToNullableUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(SqlString value)
        {
            return (value.IsNull ? null : ToNullableUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(decimal value)
        {
            return new ulong?((ulong) value);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(double value)
        {
            return new ulong?((ulong) value);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(short value)
        {
            return new ulong?((ulong) value);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(int value)
        {
            return new ulong?((ulong) value);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(long value)
        {
            return new ulong?((ulong) value);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(bool? value)
        {
            return (value.HasValue ? new ulong?(value.Value ? ((ulong) 1) : ((ulong) 0)) : null);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(byte? value)
        {
            return (value.HasValue ? new ulong?((ulong) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(char? value)
        {
            return (value.HasValue ? new ulong?((ulong) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(decimal? value)
        {
            return (value.HasValue ? new ulong?((ulong) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(double? value)
        {
            return (value.HasValue ? new ulong?((ulong) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(short? value)
        {
            return (value.HasValue ? new ulong?((ulong) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(int? value)
        {
            return (value.HasValue ? new ulong?((ulong) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(long? value)
        {
            return (value.HasValue ? new ulong?((ulong) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(sbyte? value)
        {
            return (value.HasValue ? new ulong?((ulong) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(float? value)
        {
            return (value.HasValue ? new ulong?((ulong) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(ushort? value)
        {
            return (value.HasValue ? new ulong?((ulong) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(uint? value)
        {
            return (value.HasValue ? new ulong?((ulong) value.Value) : null);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return null;
            }
            if (value is ulong)
            {
                return ToNullableUInt64((ulong) value);
            }
            if (value is string)
            {
                return ToNullableUInt64((string) value);
            }
            if (value is char)
            {
                return ToNullableUInt64((char) value);
            }
            if (value is bool)
            {
                return ToNullableUInt64((bool) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(ulong?));
            }
            return new ulong?(((IConvertible) value).ToUInt64(null));
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(sbyte value)
        {
            return new ulong?((ulong) value);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(float value)
        {
            return new ulong?((ulong) value);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(string value)
        {
            return ((value == null) ? null : new ulong?(ulong.Parse(value)));
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(ushort value)
        {
            return new ulong?((ulong) value);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(uint value)
        {
            return new ulong?((ulong) value);
        }

        [CLSCompliant(false)]
        public static ulong? ToNullableUInt64(ulong value)
        {
            return new ulong?(value);
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(bool value)
        {
            return (value ? ((sbyte) 1) : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(byte value)
        {
            return (sbyte) value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(char value)
        {
            return (sbyte) value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(SqlBoolean value)
        {
            return (value.IsNull ? ((sbyte) 0) : ToSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(SqlByte value)
        {
            return (value.IsNull ? ((sbyte) 0) : ToSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(SqlDecimal value)
        {
            return (value.IsNull ? ((sbyte) 0) : ToSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(SqlDouble value)
        {
            return (value.IsNull ? ((sbyte) 0) : ToSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(SqlInt16 value)
        {
            return (value.IsNull ? ((sbyte) 0) : ToSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(SqlInt32 value)
        {
            return (value.IsNull ? ((sbyte) 0) : ToSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(SqlInt64 value)
        {
            return (value.IsNull ? ((sbyte) 0) : ToSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(SqlMoney value)
        {
            return (value.IsNull ? ((sbyte) 0) : ToSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(SqlSingle value)
        {
            return (value.IsNull ? ((sbyte) 0) : ToSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(SqlString value)
        {
            return (value.IsNull ? ((sbyte) 0) : ToSByte(value.Value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(decimal value)
        {
            return (sbyte) value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(double value)
        {
            return (sbyte) value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(short value)
        {
            return (sbyte) value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(int value)
        {
            return (sbyte) value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(long value)
        {
            return (sbyte) value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(bool? value)
        {
            return ((value.HasValue && value.Value) ? ((sbyte) 1) : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(byte? value)
        {
            return (value.HasValue ? ((sbyte) value.Value) : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(char? value)
        {
            return (value.HasValue ? ((sbyte) value.Value) : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(decimal? value)
        {
            return (value.HasValue ? ((sbyte) value.Value) : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(double? value)
        {
            return (value.HasValue ? ((sbyte) value.Value) : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(short? value)
        {
            return (value.HasValue ? ((sbyte) value.Value) : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(int? value)
        {
            return (value.HasValue ? ((sbyte) value.Value) : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(long? value)
        {
            return (value.HasValue ? ((sbyte) value.Value) : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(sbyte? value)
        {
            return (value.HasValue ? value.Value : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(float? value)
        {
            return (value.HasValue ? ((sbyte) value.Value) : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(ushort? value)
        {
            return (value.HasValue ? ((sbyte) value.Value) : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(uint? value)
        {
            return (value.HasValue ? ((sbyte) value.Value) : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(ulong? value)
        {
            return (value.HasValue ? ((sbyte) value.Value) : ((sbyte) 0));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return 0;
            }
            if (value is sbyte)
            {
                return (sbyte) value;
            }
            if (value is string)
            {
                return ToSByte((string) value);
            }
            if (value is bool)
            {
                return ToSByte((bool) value);
            }
            if (value is char)
            {
                return ToSByte((char) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(sbyte));
            }
            return ((IConvertible) value).ToSByte(null);
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(float value)
        {
            return (sbyte) value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(string value)
        {
            return ((value == null) ? ((sbyte) 0) : sbyte.Parse(value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(ushort value)
        {
            return (sbyte) value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(uint value)
        {
            return (sbyte) value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(ulong value)
        {
            return (sbyte) value;
        }

        public static float ToSingle(bool value)
        {
            return (value ? 1f : 0f);
        }

        public static float ToSingle(byte value)
        {
            return (float) value;
        }

        public static float ToSingle(char value)
        {
            return (float) value;
        }

        public static float ToSingle(SqlBoolean value)
        {
            return (value.IsNull ? 0f : ToSingle(value.Value));
        }

        public static float ToSingle(SqlByte value)
        {
            return (value.IsNull ? 0f : ToSingle(value.Value));
        }

        public static float ToSingle(SqlDecimal value)
        {
            return (value.IsNull ? 0f : ToSingle(value.Value));
        }

        public static float ToSingle(SqlDouble value)
        {
            return (value.IsNull ? 0f : ToSingle(value.Value));
        }

        public static float ToSingle(SqlInt16 value)
        {
            return (value.IsNull ? 0f : ToSingle(value.Value));
        }

        public static float ToSingle(SqlInt32 value)
        {
            return (value.IsNull ? 0f : ToSingle(value.Value));
        }

        public static float ToSingle(SqlInt64 value)
        {
            return (value.IsNull ? 0f : ToSingle(value.Value));
        }

        public static float ToSingle(SqlMoney value)
        {
            return (value.IsNull ? 0f : ToSingle(value.Value));
        }

        public static float ToSingle(SqlSingle value)
        {
            return (value.IsNull ? 0f : value.Value);
        }

        public static float ToSingle(SqlString value)
        {
            return (value.IsNull ? 0f : ToSingle(value.Value));
        }

        public static float ToSingle(decimal value)
        {
            return (float) value;
        }

        public static float ToSingle(double value)
        {
            return (float) value;
        }

        public static float ToSingle(short value)
        {
            return (float) value;
        }

        public static float ToSingle(int value)
        {
            return (float) value;
        }

        public static float ToSingle(long value)
        {
            return (float) value;
        }

        public static float ToSingle(bool? value)
        {
            return ((value.HasValue && value.Value) ? 1f : 0f);
        }

        public static float ToSingle(byte? value)
        {
            return (value.HasValue ? ((float) value.Value) : 0f);
        }

        public static float ToSingle(char? value)
        {
            return (value.HasValue ? ((float) value.Value) : 0f);
        }

        public static float ToSingle(decimal? value)
        {
            return (value.HasValue ? ((float) value.Value) : 0f);
        }

        public static float ToSingle(double? value)
        {
            return (value.HasValue ? ((float) value.Value) : 0f);
        }

        public static float ToSingle(short? value)
        {
            return (value.HasValue ? ((float) value.Value) : 0f);
        }

        public static float ToSingle(int? value)
        {
            return (value.HasValue ? ((float) value.Value) : 0f);
        }

        public static float ToSingle(long? value)
        {
            return (value.HasValue ? ((float) value.Value) : 0f);
        }

        [CLSCompliant(false)]
        public static float ToSingle(sbyte? value)
        {
            return (value.HasValue ? ((float) value.Value) : 0f);
        }

        public static float ToSingle(float? value)
        {
            return (value.HasValue ? value.Value : 0f);
        }

        [CLSCompliant(false)]
        public static float ToSingle(ushort? value)
        {
            return (value.HasValue ? ((float) value.Value) : 0f);
        }

        [CLSCompliant(false)]
        public static float ToSingle(uint? value)
        {
            return (value.HasValue ? ((float) ((double) value.Value)) : 0f);
        }

        [CLSCompliant(false)]
        public static float ToSingle(ulong? value)
        {
            return (value.HasValue ? ((float) ((double) value.Value)) : 0f);
        }

        public static float ToSingle(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return 0f;
            }
            if (value is float)
            {
                return (float) value;
            }
            if (value is string)
            {
                return ToSingle((string) value);
            }
            if (value is bool)
            {
                return ToSingle((bool) value);
            }
            if (value is char)
            {
                return ToSingle((char) value);
            }
            if (value is SqlSingle)
            {
                return ToSingle((SqlSingle) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(float));
            }
            return ((IConvertible) value).ToSingle(null);
        }

        [CLSCompliant(false)]
        public static float ToSingle(sbyte value)
        {
            return (float) value;
        }

        public static float ToSingle(string value)
        {
            return ((value == null) ? 0f : float.Parse(value));
        }

        [CLSCompliant(false)]
        public static float ToSingle(ushort value)
        {
            return (float) value;
        }

        [CLSCompliant(false)]
        public static float ToSingle(uint value)
        {
            return (float) value;
        }

        [CLSCompliant(false)]
        public static float ToSingle(ulong value)
        {
            return (float) value;
        }

        public static SqlBinary ToSqlBinary(SqlBytes value)
        {
            return value.ToSqlBinary();
        }

        public static SqlBinary ToSqlBinary(SqlGuid value)
        {
            return value.ToSqlBinary();
        }

        public static SqlBinary ToSqlBinary(Guid value)
        {
            return ((value == Guid.Empty) ? SqlBinary.Null : new SqlGuid(value).ToSqlBinary());
        }

        public static SqlBinary ToSqlBinary(Guid? value)
        {
            return (value.HasValue ? new SqlGuid(value.Value).ToSqlBinary() : SqlBinary.Null);
        }

        public static SqlBinary ToSqlBinary(byte[] value)
        {
            return value;
        }

        public static SqlBinary ToSqlBinary(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlBinary.Null;
            }
            if (value is SqlBinary)
            {
                return (SqlBinary) value;
            }
            if (value is byte[])
            {
                return ToSqlBinary((byte[]) value);
            }
            if (value is Guid)
            {
                return ToSqlBinary((Guid) value);
            }
            if (value is SqlBytes)
            {
                return ToSqlBinary((SqlBytes) value);
            }
            if (value is SqlGuid)
            {
                return ToSqlBinary((SqlGuid) value);
            }
            return ToByteArray(value);
        }

        public static SqlBoolean ToSqlBoolean(bool value)
        {
            return value;
        }

        public static SqlBoolean ToSqlBoolean(byte value)
        {
            return (value != 0);
        }

        public static SqlBoolean ToSqlBoolean(char value)
        {
            return (value != '\0');
        }

        public static SqlBoolean ToSqlBoolean(SqlByte value)
        {
            return value.ToSqlBoolean();
        }

        public static SqlBoolean ToSqlBoolean(SqlDecimal value)
        {
            return value.ToSqlBoolean();
        }

        public static SqlBoolean ToSqlBoolean(SqlDouble value)
        {
            return value.ToSqlBoolean();
        }

        public static SqlBoolean ToSqlBoolean(SqlInt16 value)
        {
            return value.ToSqlBoolean();
        }

        public static SqlBoolean ToSqlBoolean(SqlInt32 value)
        {
            return value.ToSqlBoolean();
        }

        public static SqlBoolean ToSqlBoolean(SqlInt64 value)
        {
            return value.ToSqlBoolean();
        }

        public static SqlBoolean ToSqlBoolean(SqlMoney value)
        {
            return value.ToSqlBoolean();
        }

        public static SqlBoolean ToSqlBoolean(SqlSingle value)
        {
            return value.ToSqlBoolean();
        }

        public static SqlBoolean ToSqlBoolean(SqlString value)
        {
            return value.ToSqlBoolean();
        }

        public static SqlBoolean ToSqlBoolean(decimal value)
        {
            return (value != 0M);
        }

        public static SqlBoolean ToSqlBoolean(double value)
        {
            return (value != 0.0);
        }

        public static SqlBoolean ToSqlBoolean(short value)
        {
            return (value != 0);
        }

        public static SqlBoolean ToSqlBoolean(int value)
        {
            return (value != 0);
        }

        public static SqlBoolean ToSqlBoolean(long value)
        {
            return (value != 0L);
        }

        public static SqlBoolean ToSqlBoolean(bool? value)
        {
            return (value.HasValue ? ((SqlBoolean) value.Value) : SqlBoolean.Null);
        }

        public static SqlBoolean ToSqlBoolean(byte? value)
        {
            return (value.HasValue ? ToBoolean(value.Value) : SqlBoolean.Null);
        }

        public static SqlBoolean ToSqlBoolean(char? value)
        {
            return (value.HasValue ? ToBoolean(value.Value) : SqlBoolean.Null);
        }

        public static SqlBoolean ToSqlBoolean(decimal? value)
        {
            return (value.HasValue ? ToBoolean(value.Value) : SqlBoolean.Null);
        }

        public static SqlBoolean ToSqlBoolean(double? value)
        {
            return (value.HasValue ? ToBoolean(value.Value) : SqlBoolean.Null);
        }

        public static SqlBoolean ToSqlBoolean(short? value)
        {
            return (value.HasValue ? ToBoolean(value.Value) : SqlBoolean.Null);
        }

        public static SqlBoolean ToSqlBoolean(int? value)
        {
            return (value.HasValue ? ToBoolean(value.Value) : SqlBoolean.Null);
        }

        public static SqlBoolean ToSqlBoolean(long? value)
        {
            return (value.HasValue ? ToBoolean(value.Value) : SqlBoolean.Null);
        }

        [CLSCompliant(false)]
        public static SqlBoolean ToSqlBoolean(sbyte? value)
        {
            return (value.HasValue ? ToBoolean(value.Value) : SqlBoolean.Null);
        }

        public static SqlBoolean ToSqlBoolean(float? value)
        {
            return (value.HasValue ? ToBoolean(value.Value) : SqlBoolean.Null);
        }

        [CLSCompliant(false)]
        public static SqlBoolean ToSqlBoolean(ushort? value)
        {
            return (value.HasValue ? ToBoolean(value.Value) : SqlBoolean.Null);
        }

        [CLSCompliant(false)]
        public static SqlBoolean ToSqlBoolean(uint? value)
        {
            return (value.HasValue ? ToBoolean(value.Value) : SqlBoolean.Null);
        }

        [CLSCompliant(false)]
        public static SqlBoolean ToSqlBoolean(ulong? value)
        {
            return (value.HasValue ? ToBoolean(value.Value) : SqlBoolean.Null);
        }

        public static SqlBoolean ToSqlBoolean(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlBoolean.Null;
            }
            if (value is SqlBoolean)
            {
                return (SqlBoolean) value;
            }
            if (value is bool)
            {
                return ToSqlBoolean((bool) value);
            }
            if (value is string)
            {
                return ToSqlBoolean((string) value);
            }
            if (value is char)
            {
                return ToSqlBoolean((char) value);
            }
            return ToBoolean(value);
        }

        [CLSCompliant(false)]
        public static SqlBoolean ToSqlBoolean(sbyte value)
        {
            return (value != 0);
        }

        public static SqlBoolean ToSqlBoolean(float value)
        {
            return (value != 0f);
        }

        public static SqlBoolean ToSqlBoolean(string value)
        {
            return ((value == null) ? SqlBoolean.Null : SqlBoolean.Parse(value));
        }

        [CLSCompliant(false)]
        public static SqlBoolean ToSqlBoolean(ushort value)
        {
            return (value != 0);
        }

        [CLSCompliant(false)]
        public static SqlBoolean ToSqlBoolean(uint value)
        {
            return (value != 0);
        }

        [CLSCompliant(false)]
        public static SqlBoolean ToSqlBoolean(ulong value)
        {
            return (value != 0L);
        }

        public static SqlByte ToSqlByte(bool value)
        {
            return (value ? ((byte) 1) : ((byte) 0));
        }

        public static SqlByte ToSqlByte(byte value)
        {
            return value;
        }

        public static SqlByte ToSqlByte(char value)
        {
            return (byte) value;
        }

        public static SqlByte ToSqlByte(SqlBoolean value)
        {
            return value.ToSqlByte();
        }

        public static SqlByte ToSqlByte(SqlDateTime value)
        {
            return (value.IsNull ? SqlByte.Null : ToByte(value.Value));
        }

        public static SqlByte ToSqlByte(SqlDecimal value)
        {
            return value.ToSqlByte();
        }

        public static SqlByte ToSqlByte(SqlDouble value)
        {
            return value.ToSqlByte();
        }

        public static SqlByte ToSqlByte(SqlInt16 value)
        {
            return value.ToSqlByte();
        }

        public static SqlByte ToSqlByte(SqlInt32 value)
        {
            return value.ToSqlByte();
        }

        public static SqlByte ToSqlByte(SqlInt64 value)
        {
            return value.ToSqlByte();
        }

        public static SqlByte ToSqlByte(SqlMoney value)
        {
            return value.ToSqlByte();
        }

        public static SqlByte ToSqlByte(SqlSingle value)
        {
            return value.ToSqlByte();
        }

        public static SqlByte ToSqlByte(SqlString value)
        {
            return value.ToSqlByte();
        }

        public static SqlByte ToSqlByte(decimal value)
        {
            return (byte) value;
        }

        public static SqlByte ToSqlByte(double value)
        {
            return (byte) value;
        }

        public static SqlByte ToSqlByte(short value)
        {
            return (byte) value;
        }

        public static SqlByte ToSqlByte(int value)
        {
            return (byte) value;
        }

        public static SqlByte ToSqlByte(long value)
        {
            return (byte) value;
        }

        public static SqlByte ToSqlByte(bool? value)
        {
            return (value.HasValue ? ToByte(value.Value) : SqlByte.Null);
        }

        public static SqlByte ToSqlByte(byte? value)
        {
            return (value.HasValue ? ((SqlByte) value.Value) : SqlByte.Null);
        }

        public static SqlByte ToSqlByte(char? value)
        {
            return (value.HasValue ? ToByte(value.Value) : SqlByte.Null);
        }

        public static SqlByte ToSqlByte(decimal? value)
        {
            return (value.HasValue ? ToByte(value.Value) : SqlByte.Null);
        }

        public static SqlByte ToSqlByte(double? value)
        {
            return (value.HasValue ? ToByte(value.Value) : SqlByte.Null);
        }

        public static SqlByte ToSqlByte(short? value)
        {
            return (value.HasValue ? ToByte(value.Value) : SqlByte.Null);
        }

        public static SqlByte ToSqlByte(int? value)
        {
            return (value.HasValue ? ToByte(value.Value) : SqlByte.Null);
        }

        public static SqlByte ToSqlByte(long? value)
        {
            return (value.HasValue ? ToByte(value.Value) : SqlByte.Null);
        }

        [CLSCompliant(false)]
        public static SqlByte ToSqlByte(sbyte? value)
        {
            return (value.HasValue ? ToByte(value.Value) : SqlByte.Null);
        }

        public static SqlByte ToSqlByte(float? value)
        {
            return (value.HasValue ? ToByte(value.Value) : SqlByte.Null);
        }

        [CLSCompliant(false)]
        public static SqlByte ToSqlByte(ushort? value)
        {
            return (value.HasValue ? ToByte(value.Value) : SqlByte.Null);
        }

        [CLSCompliant(false)]
        public static SqlByte ToSqlByte(uint? value)
        {
            return (value.HasValue ? ToByte(value.Value) : SqlByte.Null);
        }

        [CLSCompliant(false)]
        public static SqlByte ToSqlByte(ulong? value)
        {
            return (value.HasValue ? ToByte(value.Value) : SqlByte.Null);
        }

        public static SqlByte ToSqlByte(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlByte.Null;
            }
            if (value is SqlByte)
            {
                return (SqlByte) value;
            }
            if (value is byte)
            {
                return ToSqlByte((byte) value);
            }
            if (value is string)
            {
                return ToSqlByte((string) value);
            }
            if (value is char)
            {
                return ToSqlByte((char) value);
            }
            if (value is bool)
            {
                return ToSqlByte((bool) value);
            }
            if (value is SqlDateTime)
            {
                return ToSqlByte((SqlDateTime) value);
            }
            return ToByte(value);
        }

        [CLSCompliant(false)]
        public static SqlByte ToSqlByte(sbyte value)
        {
            return (byte) value;
        }

        public static SqlByte ToSqlByte(float value)
        {
            return (byte) value;
        }

        public static SqlByte ToSqlByte(string value)
        {
            return ((value == null) ? SqlByte.Null : SqlByte.Parse(value));
        }

        [CLSCompliant(false)]
        public static SqlByte ToSqlByte(ushort value)
        {
            return (byte) value;
        }

        [CLSCompliant(false)]
        public static SqlByte ToSqlByte(uint value)
        {
            return (byte) value;
        }

        [CLSCompliant(false)]
        public static SqlByte ToSqlByte(ulong value)
        {
            return (byte) value;
        }

        public static SqlBytes ToSqlBytes(byte[] value)
        {
            return ((value == null) ? SqlBytes.Null : new SqlBytes(value));
        }

        public static SqlBytes ToSqlBytes(SqlBinary value)
        {
            return (value.IsNull ? SqlBytes.Null : new SqlBytes(value));
        }

        public static SqlBytes ToSqlBytes(SqlGuid value)
        {
            return (value.IsNull ? SqlBytes.Null : new SqlBytes(value.ToByteArray()));
        }

        public static SqlBytes ToSqlBytes(Guid value)
        {
            return ((value == Guid.Empty) ? SqlBytes.Null : new SqlBytes(value.ToByteArray()));
        }

        public static SqlBytes ToSqlBytes(Stream value)
        {
            return ((value == null) ? SqlBytes.Null : new SqlBytes(value));
        }

        public static SqlBytes ToSqlBytes(Guid? value)
        {
            return (value.HasValue ? new SqlBytes(value.Value.ToByteArray()) : SqlBytes.Null);
        }

        public static SqlBytes ToSqlBytes(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlBytes.Null;
            }
            if (value is SqlBytes)
            {
                return (SqlBytes) value;
            }
            if (value is byte[])
            {
                return ToSqlBytes((byte[]) value);
            }
            if (value is Stream)
            {
                return ToSqlBytes((Stream) value);
            }
            if (value is Guid)
            {
                return ToSqlBytes((Guid) value);
            }
            if (value is SqlBinary)
            {
                return ToSqlBytes((SqlBinary) value);
            }
            if (value is SqlGuid)
            {
                return ToSqlBytes((SqlGuid) value);
            }
            return new SqlBytes(ToByteArray(value));
        }

        public static SqlChars ToSqlChars(bool value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        public static SqlChars ToSqlChars(byte value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        public static SqlChars ToSqlChars(char value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        public static SqlChars ToSqlChars(SqlBinary value)
        {
            return (value.IsNull ? SqlChars.Null : new SqlChars(value.ToString().ToCharArray()));
        }

        public static SqlChars ToSqlChars(SqlBoolean value)
        {
            return (SqlChars) value.ToSqlString();
        }

        public static SqlChars ToSqlChars(SqlByte value)
        {
            return (SqlChars) value.ToSqlString();
        }

        public static SqlChars ToSqlChars(SqlDateTime value)
        {
            return (SqlChars) value.ToSqlString();
        }

        public static SqlChars ToSqlChars(SqlDecimal value)
        {
            return (SqlChars) value.ToSqlString();
        }

        public static SqlChars ToSqlChars(SqlDouble value)
        {
            return (SqlChars) value.ToSqlString();
        }

        public static SqlChars ToSqlChars(SqlGuid value)
        {
            return (SqlChars) value.ToSqlString();
        }

        public static SqlChars ToSqlChars(SqlInt16 value)
        {
            return (SqlChars) value.ToSqlString();
        }

        public static SqlChars ToSqlChars(SqlInt32 value)
        {
            return (SqlChars) value.ToSqlString();
        }

        public static SqlChars ToSqlChars(SqlInt64 value)
        {
            return (SqlChars) value.ToSqlString();
        }

        public static SqlChars ToSqlChars(SqlMoney value)
        {
            return (SqlChars) value.ToSqlString();
        }

        public static SqlChars ToSqlChars(SqlSingle value)
        {
            return (SqlChars) value.ToSqlString();
        }

        public static SqlChars ToSqlChars(SqlString value)
        {
            return (SqlChars) value;
        }

        public static SqlChars ToSqlChars(DateTime value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        public static SqlChars ToSqlChars(decimal value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        public static SqlChars ToSqlChars(double value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        public static SqlChars ToSqlChars(Guid value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        public static SqlChars ToSqlChars(short value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        public static SqlChars ToSqlChars(int value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        public static SqlChars ToSqlChars(long value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        public static SqlChars ToSqlChars(bool? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        public static SqlChars ToSqlChars(byte? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        public static SqlChars ToSqlChars(char? value)
        {
            return (value.HasValue ? new SqlChars(new char[] { value.Value }) : SqlChars.Null);
        }

        public static SqlChars ToSqlChars(DateTime? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        public static SqlChars ToSqlChars(decimal? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        public static SqlChars ToSqlChars(double? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        public static SqlChars ToSqlChars(Guid? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        public static SqlChars ToSqlChars(short? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        public static SqlChars ToSqlChars(int? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        public static SqlChars ToSqlChars(long? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        [CLSCompliant(false)]
        public static SqlChars ToSqlChars(sbyte? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        public static SqlChars ToSqlChars(float? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        public static SqlChars ToSqlChars(TimeSpan? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        [CLSCompliant(false)]
        public static SqlChars ToSqlChars(ushort? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        [CLSCompliant(false)]
        public static SqlChars ToSqlChars(uint? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        [CLSCompliant(false)]
        public static SqlChars ToSqlChars(ulong? value)
        {
            return (value.HasValue ? new SqlChars(value.ToString().ToCharArray()) : SqlChars.Null);
        }

        public static SqlChars ToSqlChars(char[] value)
        {
            return ((value == null) ? SqlChars.Null : new SqlChars(value));
        }

        public static SqlChars ToSqlChars(object value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        [CLSCompliant(false)]
        public static SqlChars ToSqlChars(sbyte value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        public static SqlChars ToSqlChars(float value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        public static SqlChars ToSqlChars(string value)
        {
            return ((value == null) ? SqlChars.Null : new SqlChars(value.ToCharArray()));
        }

        public static SqlChars ToSqlChars(TimeSpan value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        public static SqlChars ToSqlChars(Type value)
        {
            return ((value == null) ? SqlChars.Null : new SqlChars(value.FullName.ToCharArray()));
        }

        [CLSCompliant(false)]
        public static SqlChars ToSqlChars(ushort value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        [CLSCompliant(false)]
        public static SqlChars ToSqlChars(uint value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        [CLSCompliant(false)]
        public static SqlChars ToSqlChars(ulong value)
        {
            return new SqlChars(ToString(value).ToCharArray());
        }

        public static SqlDateTime ToSqlDateTime(SqlDouble value)
        {
            return (value.IsNull ? SqlDateTime.Null : ToDateTime(value));
        }

        public static SqlDateTime ToSqlDateTime(SqlInt64 value)
        {
            return (value.IsNull ? SqlDateTime.Null : ToDateTime(value));
        }

        public static SqlDateTime ToSqlDateTime(SqlString value)
        {
            return value.ToSqlDateTime();
        }

        public static SqlDateTime ToSqlDateTime(DateTime value)
        {
            return value;
        }

        public static SqlDateTime ToSqlDateTime(double value)
        {
            return ToDateTime(value);
        }

        public static SqlDateTime ToSqlDateTime(long value)
        {
            return ToDateTime(value);
        }

        public static SqlDateTime ToSqlDateTime(DateTime? value)
        {
            return (value.HasValue ? ((SqlDateTime) value.Value) : SqlDateTime.Null);
        }

        public static SqlDateTime ToSqlDateTime(double? value)
        {
            return (value.HasValue ? ToDateTime(value.Value) : SqlDateTime.Null);
        }

        public static SqlDateTime ToSqlDateTime(long? value)
        {
            return (value.HasValue ? ToDateTime(value.Value) : SqlDateTime.Null);
        }

        public static SqlDateTime ToSqlDateTime(TimeSpan? value)
        {
            return (value.HasValue ? ToDateTime(value.Value) : SqlDateTime.Null);
        }

        public static SqlDateTime ToSqlDateTime(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlDateTime.Null;
            }
            if (value is SqlDateTime)
            {
                return (SqlDateTime) value;
            }
            if (value is string)
            {
                return ToSqlDateTime((string) value);
            }
            if (value is DateTime)
            {
                return ToSqlDateTime((DateTime) value);
            }
            if (value is TimeSpan)
            {
                return ToSqlDateTime((TimeSpan) value);
            }
            if (value is long)
            {
                return ToSqlDateTime((long) value);
            }
            if (value is double)
            {
                return ToSqlDateTime((double) value);
            }
            if (value is SqlString)
            {
                return ToSqlDateTime((SqlString) value);
            }
            if (value is SqlInt64)
            {
                return ToSqlDateTime((SqlInt64) value);
            }
            if (value is SqlDouble)
            {
                return ToSqlDateTime((SqlDouble) value);
            }
            return ToDateTime(value);
        }

        public static SqlDateTime ToSqlDateTime(string value)
        {
            return ((value == null) ? SqlDateTime.Null : SqlDateTime.Parse(value));
        }

        public static SqlDateTime ToSqlDateTime(TimeSpan value)
        {
            return ToDateTime(value);
        }

        public static SqlDecimal ToSqlDecimal(bool value)
        {
            return (value ? 1.0M : 0.0M);
        }

        public static SqlDecimal ToSqlDecimal(byte value)
        {
            return (SqlDecimal) value;
        }

        public static SqlDecimal ToSqlDecimal(char value)
        {
            return (SqlDecimal) value;
        }

        public static SqlDecimal ToSqlDecimal(SqlBoolean value)
        {
            return value.ToSqlDecimal();
        }

        public static SqlDecimal ToSqlDecimal(SqlByte value)
        {
            return value.ToSqlDecimal();
        }

        public static SqlDecimal ToSqlDecimal(SqlDouble value)
        {
            return value.ToSqlDecimal();
        }

        public static SqlDecimal ToSqlDecimal(SqlInt16 value)
        {
            return value.ToSqlDecimal();
        }

        public static SqlDecimal ToSqlDecimal(SqlInt32 value)
        {
            return value.ToSqlDecimal();
        }

        public static SqlDecimal ToSqlDecimal(SqlInt64 value)
        {
            return value.ToSqlDecimal();
        }

        public static SqlDecimal ToSqlDecimal(SqlMoney value)
        {
            return value.ToSqlDecimal();
        }

        public static SqlDecimal ToSqlDecimal(SqlSingle value)
        {
            return value.ToSqlDecimal();
        }

        public static SqlDecimal ToSqlDecimal(SqlString value)
        {
            return value.ToSqlDecimal();
        }

        public static SqlDecimal ToSqlDecimal(decimal value)
        {
            return value;
        }

        public static SqlDecimal ToSqlDecimal(double value)
        {
            return (decimal) value;
        }

        public static SqlDecimal ToSqlDecimal(short value)
        {
            return (SqlDecimal) value;
        }

        public static SqlDecimal ToSqlDecimal(int value)
        {
            return (SqlDecimal) value;
        }

        public static SqlDecimal ToSqlDecimal(long value)
        {
            return value;
        }

        public static SqlDecimal ToSqlDecimal(bool? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlDecimal.Null);
        }

        public static SqlDecimal ToSqlDecimal(byte? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlDecimal.Null);
        }

        public static SqlDecimal ToSqlDecimal(decimal? value)
        {
            return (value.HasValue ? ((SqlDecimal) value.Value) : SqlDecimal.Null);
        }

        public static SqlDecimal ToSqlDecimal(double? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlDecimal.Null);
        }

        public static SqlDecimal ToSqlDecimal(short? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlDecimal.Null);
        }

        public static SqlDecimal ToSqlDecimal(int? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlDecimal.Null);
        }

        public static SqlDecimal ToSqlDecimal(long? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlDecimal.Null);
        }

        [CLSCompliant(false)]
        public static SqlDecimal ToSqlDecimal(sbyte? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlDecimal.Null);
        }

        public static SqlDecimal ToSqlDecimal(float? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlDecimal.Null);
        }

        [CLSCompliant(false)]
        public static SqlDecimal ToSqlDecimal(ushort? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlDecimal.Null);
        }

        [CLSCompliant(false)]
        public static SqlDecimal ToSqlDecimal(uint? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlDecimal.Null);
        }

        [CLSCompliant(false)]
        public static SqlDecimal ToSqlDecimal(ulong? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlDecimal.Null);
        }

        public static SqlDecimal ToSqlDecimal(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlDecimal.Null;
            }
            if (value is SqlDecimal)
            {
                return (SqlDecimal) value;
            }
            if (value is decimal)
            {
                return ToSqlDecimal((decimal) value);
            }
            if (value is string)
            {
                return ToSqlDecimal((string) value);
            }
            if (value is char)
            {
                return ToSqlDecimal((char) value);
            }
            if (value is bool)
            {
                return ToSqlDecimal((bool) value);
            }
            return ToDecimal(value);
        }

        [CLSCompliant(false)]
        public static SqlDecimal ToSqlDecimal(sbyte value)
        {
            return (SqlDecimal) value;
        }

        public static SqlDecimal ToSqlDecimal(float value)
        {
            return (decimal) value;
        }

        public static SqlDecimal ToSqlDecimal(string value)
        {
            return ((value == null) ? SqlDecimal.Null : SqlDecimal.Parse(value));
        }

        [CLSCompliant(false)]
        public static SqlDecimal ToSqlDecimal(ushort value)
        {
            return (SqlDecimal) value;
        }

        [CLSCompliant(false)]
        public static SqlDecimal ToSqlDecimal(uint value)
        {
            return (SqlDecimal) value;
        }

        [CLSCompliant(false)]
        public static SqlDecimal ToSqlDecimal(ulong value)
        {
            return (SqlDecimal) value;
        }

        public static SqlDouble ToSqlDouble(bool value)
        {
            return (value ? 1.0 : 0.0);
        }

        public static SqlDouble ToSqlDouble(byte value)
        {
            return (double) value;
        }

        public static SqlDouble ToSqlDouble(char value)
        {
            return (double) value;
        }

        public static SqlDouble ToSqlDouble(SqlBoolean value)
        {
            return value.ToSqlDouble();
        }

        public static SqlDouble ToSqlDouble(SqlByte value)
        {
            return value.ToSqlDouble();
        }

        public static SqlDouble ToSqlDouble(SqlDateTime value)
        {
            return (value.IsNull ? SqlDouble.Null : ToDouble(value.Value));
        }

        public static SqlDouble ToSqlDouble(SqlDecimal value)
        {
            return value.ToSqlDouble();
        }

        public static SqlDouble ToSqlDouble(SqlInt16 value)
        {
            return value.ToSqlDouble();
        }

        public static SqlDouble ToSqlDouble(SqlInt32 value)
        {
            return value.ToSqlDouble();
        }

        public static SqlDouble ToSqlDouble(SqlInt64 value)
        {
            return value.ToSqlDouble();
        }

        public static SqlDouble ToSqlDouble(SqlMoney value)
        {
            return value.ToSqlDouble();
        }

        public static SqlDouble ToSqlDouble(SqlSingle value)
        {
            return value.ToSqlDouble();
        }

        public static SqlDouble ToSqlDouble(SqlString value)
        {
            return value.ToSqlDouble();
        }

        public static SqlDouble ToSqlDouble(DateTime value)
        {
            TimeSpan span = (TimeSpan) (value - DateTime.MinValue);
            return span.TotalDays;
        }

        public static SqlDouble ToSqlDouble(decimal value)
        {
            return (double) value;
        }

        public static SqlDouble ToSqlDouble(double value)
        {
            return value;
        }

        public static SqlDouble ToSqlDouble(short value)
        {
            return (double) value;
        }

        public static SqlDouble ToSqlDouble(int value)
        {
            return (double) value;
        }

        public static SqlDouble ToSqlDouble(long value)
        {
            return (double) value;
        }

        public static SqlDouble ToSqlDouble(bool? value)
        {
            return (value.HasValue ? ToDouble(value.Value) : SqlDouble.Null);
        }

        public static SqlDouble ToSqlDouble(byte? value)
        {
            return (value.HasValue ? ToDouble(value.Value) : SqlDouble.Null);
        }

        public static SqlDouble ToSqlDouble(DateTime? value)
        {
            return (value.HasValue ? ToDouble(value.Value) : SqlDouble.Null);
        }

        public static SqlDouble ToSqlDouble(decimal? value)
        {
            return (value.HasValue ? ToDouble(value.Value) : SqlDouble.Null);
        }

        public static SqlDouble ToSqlDouble(double? value)
        {
            return (value.HasValue ? ((SqlDouble) value.Value) : SqlDouble.Null);
        }

        public static SqlDouble ToSqlDouble(short? value)
        {
            return (value.HasValue ? ToDouble(value.Value) : SqlDouble.Null);
        }

        public static SqlDouble ToSqlDouble(int? value)
        {
            return (value.HasValue ? ToDouble(value.Value) : SqlDouble.Null);
        }

        public static SqlDouble ToSqlDouble(long? value)
        {
            return (value.HasValue ? ToDouble(value.Value) : SqlDouble.Null);
        }

        [CLSCompliant(false)]
        public static SqlDouble ToSqlDouble(sbyte? value)
        {
            return (value.HasValue ? ToDouble(value.Value) : SqlDouble.Null);
        }

        public static SqlDouble ToSqlDouble(float? value)
        {
            return (value.HasValue ? ToDouble(value.Value) : SqlDouble.Null);
        }

        public static SqlDouble ToSqlDouble(TimeSpan? value)
        {
            return (value.HasValue ? ToDouble(value.Value) : SqlDouble.Null);
        }

        [CLSCompliant(false)]
        public static SqlDouble ToSqlDouble(ushort? value)
        {
            return (value.HasValue ? ToDouble(value.Value) : SqlDouble.Null);
        }

        [CLSCompliant(false)]
        public static SqlDouble ToSqlDouble(uint? value)
        {
            return (value.HasValue ? ToDouble(value.Value) : SqlDouble.Null);
        }

        [CLSCompliant(false)]
        public static SqlDouble ToSqlDouble(ulong? value)
        {
            return (value.HasValue ? ToDouble(value.Value) : SqlDouble.Null);
        }

        public static SqlDouble ToSqlDouble(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlDouble.Null;
            }
            if (value is SqlDouble)
            {
                return (SqlDouble) value;
            }
            if (value is double)
            {
                return ToSqlDouble((double) value);
            }
            if (value is string)
            {
                return ToSqlDouble((string) value);
            }
            if (value is char)
            {
                return ToSqlDouble((char) value);
            }
            if (value is bool)
            {
                return ToSqlDouble((bool) value);
            }
            if (value is DateTime)
            {
                return ToSqlDouble((DateTime) value);
            }
            if (value is TimeSpan)
            {
                return ToSqlDouble((TimeSpan) value);
            }
            if (value is SqlDateTime)
            {
                return ToSqlDouble((SqlDateTime) value);
            }
            return ToDouble(value);
        }

        [CLSCompliant(false)]
        public static SqlDouble ToSqlDouble(sbyte value)
        {
            return (double) value;
        }

        public static SqlDouble ToSqlDouble(float value)
        {
            return (double) value;
        }

        public static SqlDouble ToSqlDouble(string value)
        {
            return ((value == null) ? SqlDouble.Null : SqlDouble.Parse(value));
        }

        public static SqlDouble ToSqlDouble(TimeSpan value)
        {
            return value.TotalDays;
        }

        [CLSCompliant(false)]
        public static SqlDouble ToSqlDouble(ushort value)
        {
            return (double) value;
        }

        [CLSCompliant(false)]
        public static SqlDouble ToSqlDouble(uint value)
        {
            return (double) value;
        }

        [CLSCompliant(false)]
        public static SqlDouble ToSqlDouble(ulong value)
        {
            return (double) value;
        }

        public static SqlGuid ToSqlGuid(SqlBinary value)
        {
            return value.ToSqlGuid();
        }

        public static SqlGuid ToSqlGuid(SqlBytes value)
        {
            return value.ToSqlBinary().ToSqlGuid();
        }

        public static SqlGuid ToSqlGuid(SqlString value)
        {
            return value.ToSqlGuid();
        }

        public static SqlGuid ToSqlGuid(Guid value)
        {
            return value;
        }

        public static SqlGuid ToSqlGuid(Guid? value)
        {
            return (value.HasValue ? ((SqlGuid) value.Value) : SqlGuid.Null);
        }

        public static SqlGuid ToSqlGuid(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlGuid.Null;
            }
            if (value is SqlGuid)
            {
                return (SqlGuid) value;
            }
            if (value is Guid)
            {
                return ToSqlGuid((Guid) value);
            }
            if (value is string)
            {
                return ToSqlGuid((string) value);
            }
            if (value is SqlBinary)
            {
                return ToSqlGuid((SqlBinary) value);
            }
            if (value is SqlBytes)
            {
                return ToSqlGuid((SqlBytes) value);
            }
            if (value is SqlString)
            {
                return ToSqlGuid((SqlString) value);
            }
            if (value is Type)
            {
                return ToSqlGuid((Type) value);
            }
            if (value is byte[])
            {
                return ToSqlGuid((byte[]) value);
            }
            return ToGuid(value);
        }

        public static SqlGuid ToSqlGuid(string value)
        {
            return ((value == null) ? SqlGuid.Null : SqlGuid.Parse(value));
        }

        public static SqlGuid ToSqlGuid(Type value)
        {
            return ((value == null) ? SqlGuid.Null : value.GUID);
        }

        public static SqlGuid ToSqlGuid(byte[] value)
        {
            return ((value == null) ? SqlGuid.Null : new SqlGuid(value));
        }

        public static SqlInt16 ToSqlInt16(bool value)
        {
            return (value ? ((short) 1) : ((short) 0));
        }

        public static SqlInt16 ToSqlInt16(byte value)
        {
            return (SqlInt16) value;
        }

        public static SqlInt16 ToSqlInt16(char value)
        {
            return (short) value;
        }

        public static SqlInt16 ToSqlInt16(SqlBoolean value)
        {
            return value.ToSqlInt16();
        }

        public static SqlInt16 ToSqlInt16(SqlByte value)
        {
            return value.ToSqlInt16();
        }

        public static SqlInt16 ToSqlInt16(SqlDateTime value)
        {
            return (value.IsNull ? SqlInt16.Null : ToInt16(value.Value));
        }

        public static SqlInt16 ToSqlInt16(SqlDecimal value)
        {
            return value.ToSqlInt16();
        }

        public static SqlInt16 ToSqlInt16(SqlDouble value)
        {
            return value.ToSqlInt16();
        }

        public static SqlInt16 ToSqlInt16(SqlInt32 value)
        {
            return value.ToSqlInt16();
        }

        public static SqlInt16 ToSqlInt16(SqlInt64 value)
        {
            return value.ToSqlInt16();
        }

        public static SqlInt16 ToSqlInt16(SqlMoney value)
        {
            return value.ToSqlInt16();
        }

        public static SqlInt16 ToSqlInt16(SqlSingle value)
        {
            return value.ToSqlInt16();
        }

        public static SqlInt16 ToSqlInt16(SqlString value)
        {
            return value.ToSqlInt16();
        }

        public static SqlInt16 ToSqlInt16(decimal value)
        {
            return (short) value;
        }

        public static SqlInt16 ToSqlInt16(double value)
        {
            return (short) value;
        }

        public static SqlInt16 ToSqlInt16(short value)
        {
            return value;
        }

        public static SqlInt16 ToSqlInt16(int value)
        {
            return (short) value;
        }

        public static SqlInt16 ToSqlInt16(long value)
        {
            return (short) value;
        }

        public static SqlInt16 ToSqlInt16(bool? value)
        {
            return (value.HasValue ? ToInt16(value.Value) : SqlInt16.Null);
        }

        public static SqlInt16 ToSqlInt16(byte? value)
        {
            return (value.HasValue ? ToInt16(value.Value) : SqlInt16.Null);
        }

        public static SqlInt16 ToSqlInt16(char? value)
        {
            return (value.HasValue ? ToInt16(value.Value) : SqlInt16.Null);
        }

        public static SqlInt16 ToSqlInt16(decimal? value)
        {
            return (value.HasValue ? ToInt16(value.Value) : SqlInt16.Null);
        }

        public static SqlInt16 ToSqlInt16(double? value)
        {
            return (value.HasValue ? ToInt16(value.Value) : SqlInt16.Null);
        }

        public static SqlInt16 ToSqlInt16(short? value)
        {
            return (value.HasValue ? ((SqlInt16) value.Value) : SqlInt16.Null);
        }

        public static SqlInt16 ToSqlInt16(int? value)
        {
            return (value.HasValue ? ToInt16(value.Value) : SqlInt16.Null);
        }

        public static SqlInt16 ToSqlInt16(long? value)
        {
            return (value.HasValue ? ToInt16(value.Value) : SqlInt16.Null);
        }

        [CLSCompliant(false)]
        public static SqlInt16 ToSqlInt16(sbyte? value)
        {
            return (value.HasValue ? ToInt16(value.Value) : SqlInt16.Null);
        }

        public static SqlInt16 ToSqlInt16(float? value)
        {
            return (value.HasValue ? ToInt16(value.Value) : SqlInt16.Null);
        }

        [CLSCompliant(false)]
        public static SqlInt16 ToSqlInt16(ushort? value)
        {
            return (value.HasValue ? ToInt16(value.Value) : SqlInt16.Null);
        }

        [CLSCompliant(false)]
        public static SqlInt16 ToSqlInt16(uint? value)
        {
            return (value.HasValue ? ToInt16(value.Value) : SqlInt16.Null);
        }

        [CLSCompliant(false)]
        public static SqlInt16 ToSqlInt16(ulong? value)
        {
            return (value.HasValue ? ToInt16(value.Value) : SqlInt16.Null);
        }

        public static SqlInt16 ToSqlInt16(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlInt16.Null;
            }
            if (value is SqlInt16)
            {
                return (SqlInt16) value;
            }
            if (value is short)
            {
                return ToSqlInt16((short) value);
            }
            if (value is string)
            {
                return ToSqlInt16((string) value);
            }
            if (value is char)
            {
                return ToSqlInt16((char) value);
            }
            if (value is bool)
            {
                return ToSqlInt16((bool) value);
            }
            if (value is SqlDateTime)
            {
                return ToSqlInt16((SqlDateTime) value);
            }
            return ToInt16(value);
        }

        [CLSCompliant(false)]
        public static SqlInt16 ToSqlInt16(sbyte value)
        {
            return (SqlInt16) value;
        }

        public static SqlInt16 ToSqlInt16(float value)
        {
            return (short) value;
        }

        public static SqlInt16 ToSqlInt16(string value)
        {
            return ((value == null) ? SqlInt16.Null : SqlInt16.Parse(value));
        }

        [CLSCompliant(false)]
        public static SqlInt16 ToSqlInt16(ushort value)
        {
            return (short) value;
        }

        [CLSCompliant(false)]
        public static SqlInt16 ToSqlInt16(uint value)
        {
            return (short) value;
        }

        [CLSCompliant(false)]
        public static SqlInt16 ToSqlInt16(ulong value)
        {
            return (short) value;
        }

        public static SqlInt32 ToSqlInt32(bool value)
        {
            return (value ? 1 : 0);
        }

        public static SqlInt32 ToSqlInt32(byte value)
        {
            return (SqlInt32) value;
        }

        public static SqlInt32 ToSqlInt32(char value)
        {
            return (SqlInt32) value;
        }

        public static SqlInt32 ToSqlInt32(SqlBoolean value)
        {
            return value.ToSqlInt32();
        }

        public static SqlInt32 ToSqlInt32(SqlByte value)
        {
            return value.ToSqlInt32();
        }

        public static SqlInt32 ToSqlInt32(SqlDateTime value)
        {
            return (value.IsNull ? SqlInt32.Null : ToInt32(value.Value));
        }

        public static SqlInt32 ToSqlInt32(SqlDecimal value)
        {
            return value.ToSqlInt32();
        }

        public static SqlInt32 ToSqlInt32(SqlDouble value)
        {
            return value.ToSqlInt32();
        }

        public static SqlInt32 ToSqlInt32(SqlInt16 value)
        {
            return value.ToSqlInt32();
        }

        public static SqlInt32 ToSqlInt32(SqlInt64 value)
        {
            return value.ToSqlInt32();
        }

        public static SqlInt32 ToSqlInt32(SqlMoney value)
        {
            return value.ToSqlInt32();
        }

        public static SqlInt32 ToSqlInt32(SqlSingle value)
        {
            return value.ToSqlInt32();
        }

        public static SqlInt32 ToSqlInt32(SqlString value)
        {
            return value.ToSqlInt32();
        }

        public static SqlInt32 ToSqlInt32(decimal value)
        {
            return (int) value;
        }

        public static SqlInt32 ToSqlInt32(double value)
        {
            return (int) value;
        }

        public static SqlInt32 ToSqlInt32(short value)
        {
            return (SqlInt32) value;
        }

        public static SqlInt32 ToSqlInt32(int value)
        {
            return value;
        }

        public static SqlInt32 ToSqlInt32(long value)
        {
            return (int) value;
        }

        public static SqlInt32 ToSqlInt32(bool? value)
        {
            return (value.HasValue ? ToInt32(value.Value) : SqlInt32.Null);
        }

        public static SqlInt32 ToSqlInt32(byte? value)
        {
            return (value.HasValue ? ToInt32(value.Value) : SqlInt32.Null);
        }

        public static SqlInt32 ToSqlInt32(char? value)
        {
            return (value.HasValue ? ToInt32(value.Value) : SqlInt32.Null);
        }

        public static SqlInt32 ToSqlInt32(decimal? value)
        {
            return (value.HasValue ? ToInt32(value.Value) : SqlInt32.Null);
        }

        public static SqlInt32 ToSqlInt32(double? value)
        {
            return (value.HasValue ? ToInt32(value.Value) : SqlInt32.Null);
        }

        public static SqlInt32 ToSqlInt32(short? value)
        {
            return (value.HasValue ? ToInt32(value.Value) : SqlInt32.Null);
        }

        public static SqlInt32 ToSqlInt32(int? value)
        {
            return (value.HasValue ? ((SqlInt32) value.Value) : SqlInt32.Null);
        }

        public static SqlInt32 ToSqlInt32(long? value)
        {
            return (value.HasValue ? ToInt32(value.Value) : SqlInt32.Null);
        }

        [CLSCompliant(false)]
        public static SqlInt32 ToSqlInt32(sbyte? value)
        {
            return (value.HasValue ? ToInt32(value.Value) : SqlInt32.Null);
        }

        public static SqlInt32 ToSqlInt32(float? value)
        {
            return (value.HasValue ? ToInt32(value.Value) : SqlInt32.Null);
        }

        [CLSCompliant(false)]
        public static SqlInt32 ToSqlInt32(ushort? value)
        {
            return (value.HasValue ? ToInt32(value.Value) : SqlInt32.Null);
        }

        [CLSCompliant(false)]
        public static SqlInt32 ToSqlInt32(uint? value)
        {
            return (value.HasValue ? ToInt32(value.Value) : SqlInt32.Null);
        }

        [CLSCompliant(false)]
        public static SqlInt32 ToSqlInt32(ulong? value)
        {
            return (value.HasValue ? ToInt32(value.Value) : SqlInt32.Null);
        }

        public static SqlInt32 ToSqlInt32(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlInt32.Null;
            }
            if (value is SqlInt32)
            {
                return (SqlInt32) value;
            }
            if (value is int)
            {
                return ToSqlInt32((int) value);
            }
            if (value is string)
            {
                return ToSqlInt32((string) value);
            }
            if (value is char)
            {
                return ToSqlInt32((char) value);
            }
            if (value is bool)
            {
                return ToSqlInt32((bool) value);
            }
            if (value is SqlDateTime)
            {
                return ToSqlInt32((SqlDateTime) value);
            }
            return ToInt32(value);
        }

        [CLSCompliant(false)]
        public static SqlInt32 ToSqlInt32(sbyte value)
        {
            return (SqlInt32) value;
        }

        public static SqlInt32 ToSqlInt32(float value)
        {
            return (int) value;
        }

        public static SqlInt32 ToSqlInt32(string value)
        {
            return ((value == null) ? SqlInt32.Null : SqlInt32.Parse(value));
        }

        [CLSCompliant(false)]
        public static SqlInt32 ToSqlInt32(ushort value)
        {
            return (SqlInt32) value;
        }

        [CLSCompliant(false)]
        public static SqlInt32 ToSqlInt32(uint value)
        {
            return (int) value;
        }

        [CLSCompliant(false)]
        public static SqlInt32 ToSqlInt32(ulong value)
        {
            return (int) value;
        }

        public static SqlInt64 ToSqlInt64(bool value)
        {
            return (value ? ((long) 1) : ((long) 0));
        }

        public static SqlInt64 ToSqlInt64(byte value)
        {
            return (SqlInt64) value;
        }

        public static SqlInt64 ToSqlInt64(char value)
        {
            return (long) value;
        }

        public static SqlInt64 ToSqlInt64(SqlBoolean value)
        {
            return value.ToSqlInt64();
        }

        public static SqlInt64 ToSqlInt64(SqlByte value)
        {
            return value.ToSqlInt64();
        }

        public static SqlInt64 ToSqlInt64(SqlDateTime value)
        {
            return (value.IsNull ? SqlInt64.Null : ToInt64(value.Value));
        }

        public static SqlInt64 ToSqlInt64(SqlDecimal value)
        {
            return value.ToSqlInt64();
        }

        public static SqlInt64 ToSqlInt64(SqlDouble value)
        {
            return value.ToSqlInt64();
        }

        public static SqlInt64 ToSqlInt64(SqlInt16 value)
        {
            return value.ToSqlInt64();
        }

        public static SqlInt64 ToSqlInt64(SqlInt32 value)
        {
            return value.ToSqlInt64();
        }

        public static SqlInt64 ToSqlInt64(SqlMoney value)
        {
            return value.ToSqlInt64();
        }

        public static SqlInt64 ToSqlInt64(SqlSingle value)
        {
            return value.ToSqlInt64();
        }

        public static SqlInt64 ToSqlInt64(SqlString value)
        {
            return value.ToSqlInt64();
        }

        public static SqlInt64 ToSqlInt64(DateTime value)
        {
            TimeSpan span = (TimeSpan) (value - DateTime.MinValue);
            return span.Ticks;
        }

        public static SqlInt64 ToSqlInt64(decimal value)
        {
            return (long) value;
        }

        public static SqlInt64 ToSqlInt64(double value)
        {
            return (long) value;
        }

        public static SqlInt64 ToSqlInt64(short value)
        {
            return (long) value;
        }

        public static SqlInt64 ToSqlInt64(int value)
        {
            return (long) value;
        }

        public static SqlInt64 ToSqlInt64(long value)
        {
            return value;
        }

        public static SqlInt64 ToSqlInt64(bool? value)
        {
            return (value.HasValue ? ToInt64(value.Value) : SqlInt64.Null);
        }

        public static SqlInt64 ToSqlInt64(byte? value)
        {
            return (value.HasValue ? ToInt64(value.Value) : SqlInt64.Null);
        }

        public static SqlInt64 ToSqlInt64(char? value)
        {
            return (value.HasValue ? ToInt64(value.Value) : SqlInt64.Null);
        }

        public static SqlInt64 ToSqlInt64(DateTime? value)
        {
            return (value.HasValue ? ToInt64(value.Value) : SqlInt64.Null);
        }

        public static SqlInt64 ToSqlInt64(decimal? value)
        {
            return (value.HasValue ? ToInt64(value.Value) : SqlInt64.Null);
        }

        public static SqlInt64 ToSqlInt64(double? value)
        {
            return (value.HasValue ? ToInt64(value.Value) : SqlInt64.Null);
        }

        public static SqlInt64 ToSqlInt64(short? value)
        {
            return (value.HasValue ? ToInt64(value.Value) : SqlInt64.Null);
        }

        public static SqlInt64 ToSqlInt64(int? value)
        {
            return (value.HasValue ? ToInt64(value.Value) : SqlInt64.Null);
        }

        public static SqlInt64 ToSqlInt64(long? value)
        {
            return (value.HasValue ? ((SqlInt64) value.Value) : SqlInt64.Null);
        }

        [CLSCompliant(false)]
        public static SqlInt64 ToSqlInt64(sbyte? value)
        {
            return (value.HasValue ? ToInt64(value.Value) : SqlInt64.Null);
        }

        public static SqlInt64 ToSqlInt64(float? value)
        {
            return (value.HasValue ? ToInt64(value.Value) : SqlInt64.Null);
        }

        public static SqlInt64 ToSqlInt64(TimeSpan? value)
        {
            return (value.HasValue ? ToInt64(value.Value) : SqlInt64.Null);
        }

        [CLSCompliant(false)]
        public static SqlInt64 ToSqlInt64(ushort? value)
        {
            return (value.HasValue ? ToInt64(value.Value) : SqlInt64.Null);
        }

        [CLSCompliant(false)]
        public static SqlInt64 ToSqlInt64(uint? value)
        {
            return (value.HasValue ? ToInt64(value.Value) : SqlInt64.Null);
        }

        [CLSCompliant(false)]
        public static SqlInt64 ToSqlInt64(ulong? value)
        {
            return (value.HasValue ? ToInt64(value.Value) : SqlInt64.Null);
        }

        public static SqlInt64 ToSqlInt64(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlInt64.Null;
            }
            if (value is SqlInt64)
            {
                return (SqlInt64) value;
            }
            if (value is long)
            {
                return ToSqlInt64((long) value);
            }
            if (value is string)
            {
                return ToSqlInt64((string) value);
            }
            if (value is char)
            {
                return ToSqlInt64((char) value);
            }
            if (value is bool)
            {
                return ToSqlInt64((bool) value);
            }
            if (value is DateTime)
            {
                return ToSqlInt64((DateTime) value);
            }
            if (value is TimeSpan)
            {
                return ToSqlInt64((TimeSpan) value);
            }
            if (value is SqlDateTime)
            {
                return ToSqlInt64((SqlDateTime) value);
            }
            return ToInt64(value);
        }

        [CLSCompliant(false)]
        public static SqlInt64 ToSqlInt64(sbyte value)
        {
            return (long) value;
        }

        public static SqlInt64 ToSqlInt64(float value)
        {
            return (long) value;
        }

        public static SqlInt64 ToSqlInt64(string value)
        {
            return ((value == null) ? SqlInt64.Null : SqlInt64.Parse(value));
        }

        public static SqlInt64 ToSqlInt64(TimeSpan value)
        {
            return value.Ticks;
        }

        [CLSCompliant(false)]
        public static SqlInt64 ToSqlInt64(ushort value)
        {
            return (SqlInt64) value;
        }

        [CLSCompliant(false)]
        public static SqlInt64 ToSqlInt64(uint value)
        {
            return (SqlInt64) value;
        }

        [CLSCompliant(false)]
        public static SqlInt64 ToSqlInt64(ulong value)
        {
            return (long) value;
        }

        public static SqlMoney ToSqlMoney(bool value)
        {
            return (value ? 1.0M : 0.0M);
        }

        public static SqlMoney ToSqlMoney(byte value)
        {
            return (SqlMoney) value;
        }

        public static SqlMoney ToSqlMoney(char value)
        {
            return (SqlMoney) value;
        }

        public static SqlMoney ToSqlMoney(SqlBoolean value)
        {
            return value.ToSqlMoney();
        }

        public static SqlMoney ToSqlMoney(SqlByte value)
        {
            return value.ToSqlMoney();
        }

        public static SqlMoney ToSqlMoney(SqlDecimal value)
        {
            return value.ToSqlMoney();
        }

        public static SqlMoney ToSqlMoney(SqlDouble value)
        {
            return value.ToSqlMoney();
        }

        public static SqlMoney ToSqlMoney(SqlInt16 value)
        {
            return value.ToSqlMoney();
        }

        public static SqlMoney ToSqlMoney(SqlInt32 value)
        {
            return value.ToSqlMoney();
        }

        public static SqlMoney ToSqlMoney(SqlInt64 value)
        {
            return value.ToSqlMoney();
        }

        public static SqlMoney ToSqlMoney(SqlSingle value)
        {
            return value.ToSqlMoney();
        }

        public static SqlMoney ToSqlMoney(SqlString value)
        {
            return value.ToSqlMoney();
        }

        public static SqlMoney ToSqlMoney(decimal value)
        {
            return value;
        }

        public static SqlMoney ToSqlMoney(double value)
        {
            return (decimal) value;
        }

        public static SqlMoney ToSqlMoney(short value)
        {
            return (SqlMoney) value;
        }

        public static SqlMoney ToSqlMoney(int value)
        {
            return (SqlMoney) value;
        }

        public static SqlMoney ToSqlMoney(long value)
        {
            return value;
        }

        public static SqlMoney ToSqlMoney(bool? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlMoney.Null);
        }

        public static SqlMoney ToSqlMoney(byte? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlMoney.Null);
        }

        public static SqlMoney ToSqlMoney(decimal? value)
        {
            return (value.HasValue ? ((SqlMoney) value.Value) : SqlMoney.Null);
        }

        public static SqlMoney ToSqlMoney(double? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlMoney.Null);
        }

        public static SqlMoney ToSqlMoney(short? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlMoney.Null);
        }

        public static SqlMoney ToSqlMoney(int? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlMoney.Null);
        }

        public static SqlMoney ToSqlMoney(long? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlMoney.Null);
        }

        [CLSCompliant(false)]
        public static SqlMoney ToSqlMoney(sbyte? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlMoney.Null);
        }

        public static SqlMoney ToSqlMoney(float? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlMoney.Null);
        }

        [CLSCompliant(false)]
        public static SqlMoney ToSqlMoney(ushort? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlMoney.Null);
        }

        [CLSCompliant(false)]
        public static SqlMoney ToSqlMoney(uint? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlMoney.Null);
        }

        [CLSCompliant(false)]
        public static SqlMoney ToSqlMoney(ulong? value)
        {
            return (value.HasValue ? ToDecimal(value.Value) : SqlMoney.Null);
        }

        public static SqlMoney ToSqlMoney(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlMoney.Null;
            }
            if (value is SqlMoney)
            {
                return (SqlMoney) value;
            }
            if (value is decimal)
            {
                return ToSqlMoney((decimal) value);
            }
            if (value is string)
            {
                return ToSqlMoney((string) value);
            }
            if (value is char)
            {
                return ToSqlMoney((char) value);
            }
            if (value is bool)
            {
                return ToSqlMoney((bool) value);
            }
            return ToDecimal(value);
        }

        [CLSCompliant(false)]
        public static SqlMoney ToSqlMoney(sbyte value)
        {
            return (SqlMoney) value;
        }

        public static SqlMoney ToSqlMoney(float value)
        {
            return (decimal) value;
        }

        public static SqlMoney ToSqlMoney(string value)
        {
            return ((value == null) ? SqlMoney.Null : SqlMoney.Parse(value));
        }

        [CLSCompliant(false)]
        public static SqlMoney ToSqlMoney(ushort value)
        {
            return (SqlMoney) value;
        }

        [CLSCompliant(false)]
        public static SqlMoney ToSqlMoney(uint value)
        {
            return (SqlMoney) value;
        }

        [CLSCompliant(false)]
        public static SqlMoney ToSqlMoney(ulong value)
        {
            return (SqlMoney) value;
        }

        public static SqlSingle ToSqlSingle(bool value)
        {
            return (value ? 1f : 0f);
        }

        public static SqlSingle ToSqlSingle(byte value)
        {
            return (float) value;
        }

        public static SqlSingle ToSqlSingle(char value)
        {
            return (float) value;
        }

        public static SqlSingle ToSqlSingle(SqlBoolean value)
        {
            return value.ToSqlSingle();
        }

        public static SqlSingle ToSqlSingle(SqlByte value)
        {
            return value.ToSqlSingle();
        }

        public static SqlSingle ToSqlSingle(SqlDecimal value)
        {
            return value.ToSqlSingle();
        }

        public static SqlSingle ToSqlSingle(SqlDouble value)
        {
            return value.ToSqlSingle();
        }

        public static SqlSingle ToSqlSingle(SqlInt16 value)
        {
            return value.ToSqlSingle();
        }

        public static SqlSingle ToSqlSingle(SqlInt32 value)
        {
            return value.ToSqlSingle();
        }

        public static SqlSingle ToSqlSingle(SqlInt64 value)
        {
            return value.ToSqlSingle();
        }

        public static SqlSingle ToSqlSingle(SqlMoney value)
        {
            return value.ToSqlSingle();
        }

        public static SqlSingle ToSqlSingle(SqlString value)
        {
            return value.ToSqlSingle();
        }

        public static SqlSingle ToSqlSingle(decimal value)
        {
            return (float) value;
        }

        public static SqlSingle ToSqlSingle(double value)
        {
            return (float) value;
        }

        public static SqlSingle ToSqlSingle(short value)
        {
            return (float) value;
        }

        public static SqlSingle ToSqlSingle(int value)
        {
            return (float) value;
        }

        public static SqlSingle ToSqlSingle(long value)
        {
            return (float) value;
        }

        public static SqlSingle ToSqlSingle(bool? value)
        {
            return (value.HasValue ? ToSingle(value.Value) : SqlSingle.Null);
        }

        public static SqlSingle ToSqlSingle(byte? value)
        {
            return (value.HasValue ? ToSingle(value.Value) : SqlSingle.Null);
        }

        public static SqlSingle ToSqlSingle(decimal? value)
        {
            return (value.HasValue ? ToSingle(value.Value) : SqlSingle.Null);
        }

        public static SqlSingle ToSqlSingle(double? value)
        {
            return (value.HasValue ? ToSingle(value.Value) : SqlSingle.Null);
        }

        public static SqlSingle ToSqlSingle(short? value)
        {
            return (value.HasValue ? ToSingle(value.Value) : SqlSingle.Null);
        }

        public static SqlSingle ToSqlSingle(int? value)
        {
            return (value.HasValue ? ToSingle(value.Value) : SqlSingle.Null);
        }

        public static SqlSingle ToSqlSingle(long? value)
        {
            return (value.HasValue ? ToSingle(value.Value) : SqlSingle.Null);
        }

        [CLSCompliant(false)]
        public static SqlSingle ToSqlSingle(sbyte? value)
        {
            return (value.HasValue ? ToSingle(value.Value) : SqlSingle.Null);
        }

        public static SqlSingle ToSqlSingle(float? value)
        {
            return (value.HasValue ? ((SqlSingle) value.Value) : SqlSingle.Null);
        }

        [CLSCompliant(false)]
        public static SqlSingle ToSqlSingle(ushort? value)
        {
            return (value.HasValue ? ToSingle(value.Value) : SqlSingle.Null);
        }

        [CLSCompliant(false)]
        public static SqlSingle ToSqlSingle(uint? value)
        {
            return (value.HasValue ? ToSingle(value.Value) : SqlSingle.Null);
        }

        [CLSCompliant(false)]
        public static SqlSingle ToSqlSingle(ulong? value)
        {
            return (value.HasValue ? ToSingle(value.Value) : SqlSingle.Null);
        }

        public static SqlSingle ToSqlSingle(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlSingle.Null;
            }
            if (value is SqlSingle)
            {
                return (SqlSingle) value;
            }
            if (value is float)
            {
                return ToSqlSingle((float) value);
            }
            if (value is string)
            {
                return ToSqlSingle((string) value);
            }
            if (value is char)
            {
                return ToSqlSingle((char) value);
            }
            if (value is bool)
            {
                return ToSqlSingle((bool) value);
            }
            return ToSingle(value);
        }

        [CLSCompliant(false)]
        public static SqlSingle ToSqlSingle(sbyte value)
        {
            return (float) value;
        }

        public static SqlSingle ToSqlSingle(float value)
        {
            return value;
        }

        public static SqlSingle ToSqlSingle(string value)
        {
            return ((value == null) ? SqlSingle.Null : SqlSingle.Parse(value));
        }

        [CLSCompliant(false)]
        public static SqlSingle ToSqlSingle(ushort value)
        {
            return (float) value;
        }

        [CLSCompliant(false)]
        public static SqlSingle ToSqlSingle(uint value)
        {
            return (float) value;
        }

        [CLSCompliant(false)]
        public static SqlSingle ToSqlSingle(ulong value)
        {
            return (float) value;
        }

        public static SqlString ToSqlString(bool value)
        {
            return value.ToString();
        }

        public static SqlString ToSqlString(byte value)
        {
            return value.ToString();
        }

        public static SqlString ToSqlString(char value)
        {
            return value.ToString();
        }

        public static SqlString ToSqlString(SqlBinary value)
        {
            return (value.IsNull ? SqlString.Null : value.ToString());
        }

        public static SqlString ToSqlString(SqlBoolean value)
        {
            return value.ToSqlString();
        }

        public static SqlString ToSqlString(SqlByte value)
        {
            return value.ToSqlString();
        }

        public static SqlString ToSqlString(SqlChars value)
        {
            return value.ToSqlString();
        }

        public static SqlString ToSqlString(SqlDateTime value)
        {
            return value.ToSqlString();
        }

        public static SqlString ToSqlString(SqlDecimal value)
        {
            return value.ToSqlString();
        }

        public static SqlString ToSqlString(SqlDouble value)
        {
            return value.ToSqlString();
        }

        public static SqlString ToSqlString(SqlGuid value)
        {
            return value.ToSqlString();
        }

        public static SqlString ToSqlString(SqlInt16 value)
        {
            return value.ToSqlString();
        }

        public static SqlString ToSqlString(SqlInt32 value)
        {
            return value.ToSqlString();
        }

        public static SqlString ToSqlString(SqlInt64 value)
        {
            return value.ToSqlString();
        }

        public static SqlString ToSqlString(SqlMoney value)
        {
            return value.ToSqlString();
        }

        public static SqlString ToSqlString(SqlSingle value)
        {
            return value.ToSqlString();
        }

        public static SqlString ToSqlString(SqlXml value)
        {
            return (value.IsNull ? SqlString.Null : value.Value);
        }

        public static SqlString ToSqlString(DateTime value)
        {
            return value.ToString();
        }

        public static SqlString ToSqlString(decimal value)
        {
            return value.ToString();
        }

        public static SqlString ToSqlString(double value)
        {
            return value.ToString();
        }

        public static SqlString ToSqlString(Guid value)
        {
            return value.ToString();
        }

        public static SqlString ToSqlString(short value)
        {
            return value.ToString();
        }

        public static SqlString ToSqlString(int value)
        {
            return value.ToString();
        }

        public static SqlString ToSqlString(long value)
        {
            return value.ToString();
        }

        public static SqlString ToSqlString(bool? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        public static SqlString ToSqlString(byte? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        public static SqlString ToSqlString(char? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        public static SqlString ToSqlString(DateTime? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        public static SqlString ToSqlString(decimal? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        public static SqlString ToSqlString(double? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        public static SqlString ToSqlString(Guid? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        public static SqlString ToSqlString(short? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        public static SqlString ToSqlString(int? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        public static SqlString ToSqlString(long? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        [CLSCompliant(false)]
        public static SqlString ToSqlString(sbyte? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        public static SqlString ToSqlString(float? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        public static SqlString ToSqlString(TimeSpan? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        [CLSCompliant(false)]
        public static SqlString ToSqlString(ushort? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        [CLSCompliant(false)]
        public static SqlString ToSqlString(uint? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        [CLSCompliant(false)]
        public static SqlString ToSqlString(ulong? value)
        {
            return (value.HasValue ? value.ToString() : SqlString.Null);
        }

        public static SqlString ToSqlString(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlString.Null;
            }
            if (value is SqlString)
            {
                return (SqlString) value;
            }
            if (value is string)
            {
                return ToSqlString((string) value);
            }
            if (value is char)
            {
                return ToSqlString((char) value);
            }
            if (value is TimeSpan)
            {
                return ToSqlString((TimeSpan) value);
            }
            if (value is DateTime)
            {
                return ToSqlString((DateTime) value);
            }
            if (value is Guid)
            {
                return ToSqlString((Guid) value);
            }
            if (value is char[])
            {
                return ToSqlString((char[]) value);
            }
            if (value is SqlChars)
            {
                return ToSqlString((SqlChars) value);
            }
            if (value is SqlXml)
            {
                return ToSqlString((SqlXml) value);
            }
            if (value is SqlGuid)
            {
                return ToSqlString((SqlGuid) value);
            }
            if (value is SqlDateTime)
            {
                return ToSqlString((SqlDateTime) value);
            }
            if (value is SqlBinary)
            {
                return ToSqlString((SqlBinary) value);
            }
            if (value is Type)
            {
                return ToSqlString((Type) value);
            }
            if (value is XmlDocument)
            {
                return ToSqlString((XmlDocument) value);
            }
            return ToString(value);
        }

        [CLSCompliant(false)]
        public static SqlString ToSqlString(sbyte value)
        {
            return value.ToString();
        }

        public static SqlString ToSqlString(float value)
        {
            return value.ToString();
        }

        public static SqlString ToSqlString(string value)
        {
            return ((value == null) ? SqlString.Null : value);
        }

        public static SqlString ToSqlString(TimeSpan value)
        {
            return value.ToString();
        }

        public static SqlString ToSqlString(Type value)
        {
            return ((value == null) ? SqlString.Null : value.FullName);
        }

        [CLSCompliant(false)]
        public static SqlString ToSqlString(ushort value)
        {
            return value.ToString();
        }

        [CLSCompliant(false)]
        public static SqlString ToSqlString(uint value)
        {
            return value.ToString();
        }

        [CLSCompliant(false)]
        public static SqlString ToSqlString(ulong value)
        {
            return value.ToString();
        }

        public static SqlString ToSqlString(char[] value)
        {
            return new string(value);
        }

        public static SqlString ToSqlString(XmlDocument value)
        {
            return ((value == null) ? SqlString.Null : value.InnerXml);
        }

        public static SqlXml ToSqlXml(SqlBinary value)
        {
            return (value.IsNull ? SqlXml.Null : new SqlXml(new MemoryStream(value.Value)));
        }

        public static SqlXml ToSqlXml(SqlBytes value)
        {
            return (value.IsNull ? SqlXml.Null : new SqlXml(value.Stream));
        }

        public static SqlXml ToSqlXml(SqlChars value)
        {
            return (value.IsNull ? SqlXml.Null : new SqlXml(new XmlTextReader(new StringReader(value.ToSqlString().Value))));
        }

        public static SqlXml ToSqlXml(SqlString value)
        {
            return (value.IsNull ? SqlXml.Null : new SqlXml(new XmlTextReader(new StringReader(value.Value))));
        }

        public static SqlXml ToSqlXml(Stream value)
        {
            return ((value == null) ? SqlXml.Null : new SqlXml(value));
        }

        public static SqlXml ToSqlXml(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return SqlXml.Null;
            }
            if (value is SqlXml)
            {
                return (SqlXml) value;
            }
            if (value is string)
            {
                return ToSqlXml((string) value);
            }
            if (value is Stream)
            {
                return ToSqlXml((Stream) value);
            }
            if (value is XmlReader)
            {
                return ToSqlXml((XmlReader) value);
            }
            if (value is XmlDocument)
            {
                return ToSqlXml((XmlDocument) value);
            }
            if (value is char[])
            {
                return ToSqlXml((char[]) value);
            }
            if (value is byte[])
            {
                return ToSqlXml((byte[]) value);
            }
            if (value is SqlString)
            {
                return ToSqlXml((SqlString) value);
            }
            if (value is SqlChars)
            {
                return ToSqlXml((SqlChars) value);
            }
            if (value is SqlBinary)
            {
                return ToSqlXml((SqlBinary) value);
            }
            if (!(value is SqlBytes))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(SqlXml));
            }
            return ToSqlXml((SqlBytes) value);
        }

        public static SqlXml ToSqlXml(string value)
        {
            return ((value == null) ? SqlXml.Null : new SqlXml(new XmlTextReader(new StringReader(value))));
        }

        public static SqlXml ToSqlXml(XmlDocument value)
        {
            return ((value == null) ? SqlXml.Null : new SqlXml(new XmlTextReader(new StringReader(value.InnerXml))));
        }

        public static SqlXml ToSqlXml(XmlReader value)
        {
            return ((value == null) ? SqlXml.Null : new SqlXml(value));
        }

        public static SqlXml ToSqlXml(byte[] value)
        {
            return ((value == null) ? SqlXml.Null : new SqlXml(new MemoryStream(value)));
        }

        public static SqlXml ToSqlXml(char[] value)
        {
            return ((value == null) ? SqlXml.Null : new SqlXml(new XmlTextReader(new StringReader(new string(value)))));
        }

        public static Stream ToStream(SqlBinary p)
        {
            return (p.IsNull ? Stream.Null : new MemoryStream(p.Value));
        }

        public static Stream ToStream(SqlBytes p)
        {
            return (p.IsNull ? Stream.Null : p.Stream);
        }

        public static Stream ToStream(SqlGuid p)
        {
            return (p.IsNull ? Stream.Null : new MemoryStream(p.Value.ToByteArray()));
        }

        public static Stream ToStream(Guid p)
        {
            return ((p == Guid.Empty) ? Stream.Null : new MemoryStream(p.ToByteArray()));
        }

        public static Stream ToStream(Guid? p)
        {
            return (p.HasValue ? new MemoryStream(p.Value.ToByteArray()) : Stream.Null);
        }

        public static Stream ToStream(byte[] p)
        {
            return ((p == null) ? Stream.Null : new MemoryStream(p));
        }

        public static Stream ToStream(object p)
        {
            if ((p == null) || (p is DBNull))
            {
                return Stream.Null;
            }
            if (p is Stream)
            {
                return (Stream) p;
            }
            if (p is Guid)
            {
                return ToStream((Guid) p);
            }
            if (p is byte[])
            {
                return ToStream((byte[]) p);
            }
            if (p is SqlBytes)
            {
                return ToStream((SqlBytes) p);
            }
            if (p is SqlBinary)
            {
                return ToStream((SqlBinary) p);
            }
            if (!(p is SqlGuid))
            {
                throw CreateInvalidCastException(p.GetType(), typeof(Stream));
            }
            return ToStream((SqlGuid) p);
        }

        public static string ToString(bool value)
        {
            return value.ToString();
        }

        public static string ToString(byte value)
        {
            return value.ToString();
        }

        public static string ToString(char value)
        {
            return value.ToString();
        }

        public static string ToString(SqlBoolean value)
        {
            return value.ToString();
        }

        public static string ToString(SqlByte value)
        {
            return value.ToString();
        }

        public static string ToString(SqlChars value)
        {
            return (value.IsNull ? null : value.ToSqlString().Value);
        }

        public static string ToString(SqlDecimal value)
        {
            return value.ToString();
        }

        public static string ToString(SqlDouble value)
        {
            return value.ToString();
        }

        public static string ToString(SqlGuid value)
        {
            return value.ToString();
        }

        public static string ToString(SqlInt16 value)
        {
            return value.ToString();
        }

        public static string ToString(SqlInt32 value)
        {
            return value.ToString();
        }

        public static string ToString(SqlInt64 value)
        {
            return value.ToString();
        }

        public static string ToString(SqlMoney value)
        {
            return value.ToString();
        }

        public static string ToString(SqlSingle value)
        {
            return value.ToString();
        }

        public static string ToString(SqlString value)
        {
            return value.ToString();
        }

        public static string ToString(SqlXml value)
        {
            return (value.IsNull ? null : value.Value);
        }

        public static string ToString(DateTime value)
        {
            return value.ToString();
        }

        public static string ToString(decimal value)
        {
            return value.ToString();
        }

        public static string ToString(double value)
        {
            return value.ToString();
        }

        public static string ToString(Guid value)
        {
            return value.ToString();
        }

        public static string ToString(short value)
        {
            return value.ToString();
        }

        public static string ToString(int value)
        {
            return value.ToString();
        }

        public static string ToString(long value)
        {
            return value.ToString();
        }

        public static string ToString(bool? value)
        {
            return value.ToString();
        }

        public static string ToString(byte? value)
        {
            return value.ToString();
        }

        public static string ToString(char? value)
        {
            return value.ToString();
        }

        public static string ToString(DateTime? value)
        {
            return value.ToString();
        }

        public static string ToString(decimal? value)
        {
            return value.ToString();
        }

        public static string ToString(double? value)
        {
            return value.ToString();
        }

        public static string ToString(Guid? value)
        {
            return value.ToString();
        }

        public static string ToString(short? value)
        {
            return value.ToString();
        }

        public static string ToString(int? value)
        {
            return value.ToString();
        }

        public static string ToString(long? value)
        {
            return value.ToString();
        }

        [CLSCompliant(false)]
        public static string ToString(sbyte? value)
        {
            return value.ToString();
        }

        public static string ToString(float? value)
        {
            return value.ToString();
        }

        public static string ToString(TimeSpan? value)
        {
            return value.ToString();
        }

        [CLSCompliant(false)]
        public static string ToString(ushort? value)
        {
            return value.ToString();
        }

        [CLSCompliant(false)]
        public static string ToString(uint? value)
        {
            return value.ToString();
        }

        [CLSCompliant(false)]
        public static string ToString(ulong? value)
        {
            return value.ToString();
        }

        public static string ToString(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return string.Empty;
            }
            if (value is string)
            {
                return (string) value;
            }
            if (value is char)
            {
                return ToString((char) value);
            }
            if (value is TimeSpan)
            {
                return ToString((TimeSpan) value);
            }
            if (value is DateTime)
            {
                return ToString((DateTime) value);
            }
            if (value is Guid)
            {
                return ToString((Guid) value);
            }
            if (value is SqlGuid)
            {
                return ToString((SqlGuid) value);
            }
            if (value is SqlChars)
            {
                return ToString((SqlChars) value);
            }
            if (value is SqlXml)
            {
                return ToString((SqlXml) value);
            }
            if (value is XmlDocument)
            {
                return ToString((XmlDocument) value);
            }
            if (value is Type)
            {
                return ToString((Type) value);
            }
            if (value is IConvertible)
            {
                return ((IConvertible) value).ToString(null);
            }
            if (value is IFormattable)
            {
                return ((IFormattable) value).ToString(null, null);
            }
            return value.ToString();
        }

        [CLSCompliant(false)]
        public static string ToString(sbyte value)
        {
            return value.ToString();
        }

        public static string ToString(float value)
        {
            return value.ToString();
        }

        public static string ToString(TimeSpan value)
        {
            return value.ToString();
        }

        public static string ToString(Type value)
        {
            return ((value == null) ? null : value.FullName);
        }

        [CLSCompliant(false)]
        public static string ToString(ushort value)
        {
            return value.ToString();
        }

        [CLSCompliant(false)]
        public static string ToString(uint value)
        {
            return value.ToString();
        }

        [CLSCompliant(false)]
        public static string ToString(ulong value)
        {
            return value.ToString();
        }

        public static string ToString(XmlDocument value)
        {
            return ((value == null) ? null : value.InnerXml);
        }

        public static TimeSpan ToTimeSpan(SqlDateTime value)
        {
            return (value.IsNull ? TimeSpan.MinValue : ((TimeSpan) (value.Value - DateTime.MinValue)));
        }

        public static TimeSpan ToTimeSpan(SqlDouble value)
        {
            return (value.IsNull ? TimeSpan.MinValue : TimeSpan.FromDays(value.Value));
        }

        public static TimeSpan ToTimeSpan(SqlInt64 value)
        {
            return (value.IsNull ? TimeSpan.MinValue : TimeSpan.FromTicks(value.Value));
        }

        public static TimeSpan ToTimeSpan(SqlString value)
        {
            return (value.IsNull ? TimeSpan.MinValue : TimeSpan.Parse(value.Value));
        }

        public static TimeSpan ToTimeSpan(DateTime value)
        {
            return (TimeSpan) (value - DateTime.MinValue);
        }

        public static TimeSpan ToTimeSpan(double value)
        {
            return TimeSpan.FromDays(value);
        }

        public static TimeSpan ToTimeSpan(long value)
        {
            return TimeSpan.FromTicks(value);
        }

        public static TimeSpan ToTimeSpan(DateTime? value)
        {
            return (value.HasValue ? ((TimeSpan) (value.Value - DateTime.MinValue)) : TimeSpan.MinValue);
        }

        public static TimeSpan ToTimeSpan(double? value)
        {
            return (value.HasValue ? TimeSpan.FromDays(value.Value) : TimeSpan.MinValue);
        }

        public static TimeSpan ToTimeSpan(long? value)
        {
            return (value.HasValue ? TimeSpan.FromTicks(value.Value) : TimeSpan.MinValue);
        }

        public static TimeSpan ToTimeSpan(TimeSpan? value)
        {
            return (value.HasValue ? value.Value : TimeSpan.MinValue);
        }

        public static TimeSpan ToTimeSpan(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return TimeSpan.MinValue;
            }
            if (value is TimeSpan)
            {
                return (TimeSpan) value;
            }
            if (value is string)
            {
                return ToTimeSpan((string) value);
            }
            if (value is DateTime)
            {
                return ToTimeSpan((DateTime) value);
            }
            if (value is long)
            {
                return ToTimeSpan((long) value);
            }
            if (value is double)
            {
                return ToTimeSpan((double) value);
            }
            if (value is SqlString)
            {
                return ToTimeSpan((SqlString) value);
            }
            if (value is SqlDateTime)
            {
                return ToTimeSpan((SqlDateTime) value);
            }
            if (value is SqlInt64)
            {
                return ToTimeSpan((SqlInt64) value);
            }
            if (!(value is SqlDouble))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(TimeSpan));
            }
            return ToTimeSpan((SqlDouble) value);
        }

        public static TimeSpan ToTimeSpan(string value)
        {
            return ((value == null) ? TimeSpan.MinValue : TimeSpan.Parse(value));
        }

        public static Type ToType(char[] p)
        {
            return ((p == null) ? null : Type.GetType(new string(p)));
        }

        public static Type ToType(SqlChars p)
        {
            return (p.IsNull ? null : Type.GetType(new string(p.Value)));
        }

        public static Type ToType(SqlGuid p)
        {
            return (p.IsNull ? null : Type.GetTypeFromCLSID(p.Value));
        }

        public static Type ToType(SqlString p)
        {
            return (p.IsNull ? null : Type.GetType(p.Value));
        }

        public static Type ToType(Guid p)
        {
            return ((p == Guid.Empty) ? null : Type.GetTypeFromCLSID(p));
        }

        public static Type ToType(Guid? p)
        {
            return (p.HasValue ? Type.GetTypeFromCLSID(p.Value) : null);
        }

        public static Type ToType(object p)
        {
            if ((p == null) || (p is DBNull))
            {
                return null;
            }
            if (p is Type)
            {
                return (Type) p;
            }
            if (p is string)
            {
                return ToType((string) p);
            }
            if (p is char[])
            {
                return ToType((char[]) p);
            }
            if (p is Guid)
            {
                return ToType((Guid) p);
            }
            if (p is SqlString)
            {
                return ToType((SqlString) p);
            }
            if (p is SqlChars)
            {
                return ToType((SqlChars) p);
            }
            if (!(p is SqlGuid))
            {
                throw CreateInvalidCastException(p.GetType(), typeof(Type));
            }
            return ToType((SqlGuid) p);
        }

        public static Type ToType(string p)
        {
            return ((p == null) ? null : Type.GetType(p));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(bool value)
        {
            return (value ? ((ushort) 1) : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(byte value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(char value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(SqlBoolean value)
        {
            return (value.IsNull ? ((ushort) 0) : ToUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(SqlByte value)
        {
            return (value.IsNull ? ((ushort) 0) : ToUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(SqlDecimal value)
        {
            return (value.IsNull ? ((ushort) 0) : ToUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(SqlDouble value)
        {
            return (value.IsNull ? ((ushort) 0) : ToUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(SqlInt16 value)
        {
            return (value.IsNull ? ((ushort) 0) : ToUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(SqlInt32 value)
        {
            return (value.IsNull ? ((ushort) 0) : ToUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(SqlInt64 value)
        {
            return (value.IsNull ? ((ushort) 0) : ToUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(SqlMoney value)
        {
            return (value.IsNull ? ((ushort) 0) : ToUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(SqlSingle value)
        {
            return (value.IsNull ? ((ushort) 0) : ToUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(SqlString value)
        {
            return (value.IsNull ? ((ushort) 0) : ToUInt16(value.Value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(decimal value)
        {
            return (ushort) value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(double value)
        {
            return (ushort) value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(short value)
        {
            return (ushort) value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(int value)
        {
            return (ushort) value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(long value)
        {
            return (ushort) value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(bool? value)
        {
            return ((value.HasValue && value.Value) ? ((ushort) 1) : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(byte? value)
        {
            return (value.HasValue ? ((ushort) value.Value) : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(char? value)
        {
            return (value.HasValue ? ((ushort) value.Value) : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(decimal? value)
        {
            return (value.HasValue ? ((ushort) value.Value) : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(double? value)
        {
            return (value.HasValue ? ((ushort) value.Value) : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(short? value)
        {
            return (value.HasValue ? ((ushort) value.Value) : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(int? value)
        {
            return (value.HasValue ? ((ushort) value.Value) : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(long? value)
        {
            return (value.HasValue ? ((ushort) value.Value) : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(sbyte? value)
        {
            return (value.HasValue ? ((ushort) value.Value) : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(float? value)
        {
            return (value.HasValue ? ((ushort) value.Value) : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(ushort? value)
        {
            return (value.HasValue ? value.Value : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(uint? value)
        {
            return (value.HasValue ? ((ushort) value.Value) : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(ulong? value)
        {
            return (value.HasValue ? ((ushort) value.Value) : ((ushort) 0));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return 0;
            }
            if (value is ushort)
            {
                return (ushort) value;
            }
            if (value is string)
            {
                return ToUInt16((string) value);
            }
            if (value is bool)
            {
                return ToUInt16((bool) value);
            }
            if (value is char)
            {
                return ToUInt16((char) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(ushort));
            }
            return ((IConvertible) value).ToUInt16(null);
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(sbyte value)
        {
            return (ushort) value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(float value)
        {
            return (ushort) value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(string value)
        {
            return ((value == null) ? ((ushort) 0) : ushort.Parse(value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(uint value)
        {
            return (ushort) value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(ulong value)
        {
            return (ushort) value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(bool value)
        {
            return (value ? 1 : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(byte value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(char value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(SqlBoolean value)
        {
            return (value.IsNull ? 0 : ToUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(SqlByte value)
        {
            return (value.IsNull ? 0 : ToUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(SqlDecimal value)
        {
            return (value.IsNull ? 0 : ToUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(SqlDouble value)
        {
            return (value.IsNull ? 0 : ToUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(SqlInt16 value)
        {
            return (value.IsNull ? 0 : ToUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(SqlInt32 value)
        {
            return (value.IsNull ? 0 : ToUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(SqlInt64 value)
        {
            return (value.IsNull ? 0 : ToUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(SqlMoney value)
        {
            return (value.IsNull ? 0 : ToUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(SqlSingle value)
        {
            return (value.IsNull ? 0 : ToUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(SqlString value)
        {
            return (value.IsNull ? 0 : ToUInt32(value.Value));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(decimal value)
        {
            return (uint) value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(double value)
        {
            return (uint) value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(short value)
        {
            return (uint) value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(int value)
        {
            return (uint) value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(long value)
        {
            return (uint) value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(bool? value)
        {
            return ((value.HasValue && value.Value) ? 1 : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(byte? value)
        {
            return (value.HasValue ? ((uint) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(char? value)
        {
            return (value.HasValue ? ((uint) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(decimal? value)
        {
            return (value.HasValue ? ((uint) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(double? value)
        {
            return (value.HasValue ? ((uint) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(short? value)
        {
            return (value.HasValue ? ((uint) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(int? value)
        {
            return (value.HasValue ? ((uint) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(long? value)
        {
            return (value.HasValue ? ((uint) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(sbyte? value)
        {
            return (value.HasValue ? ((uint) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(float? value)
        {
            return (value.HasValue ? ((uint) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(ushort? value)
        {
            return (value.HasValue ? ((uint) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(uint? value)
        {
            return (value.HasValue ? value.Value : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(ulong? value)
        {
            return (value.HasValue ? ((uint) value.Value) : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return 0;
            }
            if (value is uint)
            {
                return (uint) value;
            }
            if (value is string)
            {
                return ToUInt32((string) value);
            }
            if (value is bool)
            {
                return ToUInt32((bool) value);
            }
            if (value is char)
            {
                return ToUInt32((char) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(uint));
            }
            return ((IConvertible) value).ToUInt32(null);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(sbyte value)
        {
            return (uint) value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(float value)
        {
            return (uint) value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(string value)
        {
            return ((value == null) ? 0 : uint.Parse(value));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(ushort value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(ulong value)
        {
            return (uint) value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(bool value)
        {
            return (value ? ((ulong) 1) : ((ulong) 0));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(byte value)
        {
            return (ulong) value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(char value)
        {
            return (ulong) value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(SqlBoolean value)
        {
            return (value.IsNull ? ((ulong) 0L) : ToUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(SqlByte value)
        {
            return (value.IsNull ? ((ulong) 0L) : ToUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(SqlDecimal value)
        {
            return (value.IsNull ? ((ulong) 0L) : ToUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(SqlDouble value)
        {
            return (value.IsNull ? ((ulong) 0L) : ToUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(SqlInt16 value)
        {
            return (value.IsNull ? ((ulong) 0L) : ToUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(SqlInt32 value)
        {
            return (value.IsNull ? ((ulong) 0L) : ToUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(SqlInt64 value)
        {
            return (value.IsNull ? ((ulong) 0L) : ToUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(SqlMoney value)
        {
            return (value.IsNull ? ((ulong) 0L) : ToUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(SqlSingle value)
        {
            return (value.IsNull ? ((ulong) 0L) : ToUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(SqlString value)
        {
            return (value.IsNull ? ((ulong) 0L) : ToUInt64(value.Value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(decimal value)
        {
            return (ulong) value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(double value)
        {
            return (ulong) value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(short value)
        {
            return (ulong) value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(int value)
        {
            return (ulong) value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(long value)
        {
            return (ulong) value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(bool? value)
        {
            return ((value.HasValue && value.Value) ? ((ulong) 1L) : ((ulong) 0L));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(byte? value)
        {
            return (value.HasValue ? ((ulong) value.Value) : ((ulong) 0L));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(char? value)
        {
            return (value.HasValue ? ((ulong) value.Value) : ((ulong) 0L));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(decimal? value)
        {
            return (value.HasValue ? ((ulong) value.Value) : ((ulong) 0L));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(double? value)
        {
            return (value.HasValue ? ((ulong) value.Value) : ((ulong) 0L));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(short? value)
        {
            return (value.HasValue ? ((ulong) value.Value) : ((ulong) 0L));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(int? value)
        {
            return (value.HasValue ? ((ulong) value.Value) : ((ulong) 0L));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(long? value)
        {
            return (value.HasValue ? ((ulong) value.Value) : ((ulong) 0L));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(sbyte? value)
        {
            return (value.HasValue ? ((ulong) value.Value) : ((ulong) 0L));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(float? value)
        {
            return (value.HasValue ? ((ulong) value.Value) : ((ulong) 0L));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(ushort? value)
        {
            return (value.HasValue ? ((ulong) value.Value) : ((ulong) 0L));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(uint? value)
        {
            return (value.HasValue ? ((ulong) value.Value) : ((ulong) 0L));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(ulong? value)
        {
            return (value.HasValue ? value.Value : ((ulong) 0L));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(object value)
        {
            if ((value == null) || (value is DBNull))
            {
                return 0L;
            }
            if (value is ulong)
            {
                return (ulong) value;
            }
            if (value is string)
            {
                return ToUInt64((string) value);
            }
            if (value is bool)
            {
                return ToUInt64((bool) value);
            }
            if (value is char)
            {
                return ToUInt64((char) value);
            }
            if (!(value is IConvertible))
            {
                throw CreateInvalidCastException(value.GetType(), typeof(ulong));
            }
            return ((IConvertible) value).ToUInt64(null);
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(sbyte value)
        {
            return (ulong) value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(float value)
        {
            return (ulong) value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(string value)
        {
            return ((value == null) ? ((ulong) 0L) : ulong.Parse(value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(ushort value)
        {
            return (ulong) value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(uint value)
        {
            return (ulong) value;
        }

        public static XDocument ToXDocument(object p)
        {
            if ((p == null) || (p is DBNull))
            {
                return null;
            }
            if (p is XDocument)
            {
                return (XDocument) p;
            }
            if (p is string)
            {
                return ToXDocument((string) p);
            }
            if (!(p is XmlDocument))
            {
                throw CreateInvalidCastException(p.GetType(), typeof(XDocument));
            }
            return ToXDocument((XmlDocument) p);
        }

        public static XDocument ToXDocument(string p)
        {
            if (p == null)
            {
                return null;
            }
            return XDocument.Parse(p);
        }

        public static XDocument ToXDocument(XmlDocument p)
        {
            if (p == null)
            {
                return null;
            }
            return XDocument.Parse(p.OuterXml);
        }

        public static XElement ToXElement(object p)
        {
            if ((p == null) || (p is DBNull))
            {
                return null;
            }
            if (p is XElement)
            {
                return (XElement) p;
            }
            if (p is string)
            {
                return ToXElement((string) p);
            }
            if (!(p is XmlDocument))
            {
                throw CreateInvalidCastException(p.GetType(), typeof(XElement));
            }
            return ToXElement((XmlDocument) p);
        }

        public static XElement ToXElement(string p)
        {
            if (p == null)
            {
                return null;
            }
            return XElement.Parse(p);
        }

        public static XElement ToXElement(XmlDocument p)
        {
            if (p == null)
            {
                return null;
            }
            return XElement.Parse(p.OuterXml);
        }

        public static XmlDocument ToXmlDocument(SqlBinary p)
        {
            return (p.IsNull ? null : ToXmlDocument((Stream) new MemoryStream(p.Value)));
        }

        public static XmlDocument ToXmlDocument(SqlChars p)
        {
            return (p.IsNull ? null : ToXmlDocument(p.ToSqlString().Value));
        }

        public static XmlDocument ToXmlDocument(SqlString p)
        {
            return (p.IsNull ? null : ToXmlDocument(p.Value));
        }

        public static XmlDocument ToXmlDocument(SqlXml p)
        {
            return (p.IsNull ? null : ToXmlDocument(p.Value));
        }

        public static XmlDocument ToXmlDocument(Stream p)
        {
            if (p == null)
            {
                return null;
            }
            XmlDocument document = new XmlDocument();
            document.Load(p);
            return document;
        }

        public static XmlDocument ToXmlDocument(TextReader p)
        {
            if (p == null)
            {
                return null;
            }
            XmlDocument document = new XmlDocument();
            document.Load(p);
            return document;
        }

        public static XmlDocument ToXmlDocument(object p)
        {
            if ((p == null) || (p is DBNull))
            {
                return null;
            }
            if (p is XmlDocument)
            {
                return (XmlDocument) p;
            }
            if (p is string)
            {
                return ToXmlDocument((string) p);
            }
            if (p is SqlString)
            {
                return ToXmlDocument((SqlString) p);
            }
            if (p is SqlXml)
            {
                return ToXmlDocument((SqlXml) p);
            }
            if (p is SqlChars)
            {
                return ToXmlDocument((SqlChars) p);
            }
            if (p is SqlBinary)
            {
                return ToXmlDocument((SqlBinary) p);
            }
            if (p is Stream)
            {
                return ToXmlDocument((Stream) p);
            }
            if (p is TextReader)
            {
                return ToXmlDocument((TextReader) p);
            }
            if (p is XmlReader)
            {
                return ToXmlDocument((XmlReader) p);
            }
            if (p is char[])
            {
                return ToXmlDocument((char[]) p);
            }
            if (p is byte[])
            {
                return ToXmlDocument((byte[]) p);
            }
            if (p is XDocument)
            {
                return ToXmlDocument((XDocument) p);
            }
            if (!(p is XElement))
            {
                throw CreateInvalidCastException(p.GetType(), typeof(XmlDocument));
            }
            return ToXmlDocument((XElement) p);
        }

        public static XmlDocument ToXmlDocument(string p)
        {
            if (p == null)
            {
                return null;
            }
            XmlDocument document = new XmlDocument();
            document.LoadXml(p);
            return document;
        }

        public static XmlDocument ToXmlDocument(XDocument p)
        {
            if (p == null)
            {
                return null;
            }
            XmlDocument document = new XmlDocument();
            document.Load(p.ToString());
            return document;
        }

        public static XmlDocument ToXmlDocument(XElement p)
        {
            if (p == null)
            {
                return null;
            }
            XmlDocument document = new XmlDocument();
            document.Load(p.ToString());
            return document;
        }

        public static XmlDocument ToXmlDocument(byte[] p)
        {
            return ((p == null) ? null : ToXmlDocument((Stream) new MemoryStream(p)));
        }

        public static XmlDocument ToXmlDocument(char[] p)
        {
            return ((p == null) ? null : ToXmlDocument(new string(p)));
        }

        public static XmlDocument ToXmlDocument(XmlReader p)
        {
            if (p == null)
            {
                return null;
            }
            XmlDocument document = new XmlDocument();
            document.Load(p);
            return document;
        }

        public static XmlReader ToXmlReader(SqlBinary p)
        {
            return (p.IsNull ? null : new XmlTextReader(new MemoryStream(p.Value)));
        }

        public static XmlReader ToXmlReader(SqlChars p)
        {
            return (p.IsNull ? null : new XmlTextReader(new StringReader(p.ToSqlString().Value)));
        }

        public static XmlReader ToXmlReader(SqlString p)
        {
            return (p.IsNull ? null : new XmlTextReader(new StringReader(p.Value)));
        }

        public static XmlReader ToXmlReader(SqlXml p)
        {
            return (p.IsNull ? null : p.CreateReader());
        }

        public static XmlReader ToXmlReader(Stream p)
        {
            return ((p == null) ? null : new XmlTextReader(p));
        }

        public static XmlReader ToXmlReader(TextReader p)
        {
            return ((p == null) ? null : new XmlTextReader(p));
        }

        public static XmlReader ToXmlReader(object p)
        {
            if ((p == null) || (p is DBNull))
            {
                return null;
            }
            if (p is XmlReader)
            {
                return (XmlReader) p;
            }
            if (p is string)
            {
                return ToXmlReader((string) p);
            }
            if (p is SqlString)
            {
                return ToXmlReader((SqlString) p);
            }
            if (p is SqlXml)
            {
                return ToXmlReader((SqlXml) p);
            }
            if (p is SqlChars)
            {
                return ToXmlReader((SqlChars) p);
            }
            if (p is SqlBinary)
            {
                return ToXmlReader((SqlBinary) p);
            }
            if (p is Stream)
            {
                return ToXmlReader((Stream) p);
            }
            if (p is TextReader)
            {
                return ToXmlReader((TextReader) p);
            }
            if (p is XmlDocument)
            {
                return ToXmlReader((XmlDocument) p);
            }
            if (p is char[])
            {
                return ToXmlReader((char[]) p);
            }
            if (!(p is byte[]))
            {
                throw CreateInvalidCastException(p.GetType(), typeof(XmlReader));
            }
            return ToXmlReader((byte[]) p);
        }

        public static XmlReader ToXmlReader(string p)
        {
            return ((p == null) ? null : new XmlTextReader(new StringReader(p)));
        }

        public static XmlReader ToXmlReader(XmlDocument p)
        {
            return ((p == null) ? null : new XmlTextReader(new StringReader(p.InnerXml)));
        }

        public static XmlReader ToXmlReader(byte[] p)
        {
            return ((p == null) ? null : new XmlTextReader(new MemoryStream(p)));
        }

        public static XmlReader ToXmlReader(char[] p)
        {
            return ((p == null) ? null : new XmlTextReader(new StringReader(new string(p))));
        }
    }
}

