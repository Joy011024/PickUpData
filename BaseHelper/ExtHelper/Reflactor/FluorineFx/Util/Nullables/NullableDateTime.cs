namespace FluorineFx.Util.Nullables
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof(NullableDateTimeConverter))]
    public struct NullableDateTime : INullableType, IFormattable, IComparable
    {
        public static readonly NullableDateTime Default;
        private DateTime _value;
        private bool hasValue;
        public NullableDateTime(DateTime value)
        {
            this._value = value;
            this.hasValue = true;
        }

        object INullableType.Value
        {
            get
            {
                return this.Value;
            }
        }
        public bool HasValue
        {
            get
            {
                return this.hasValue;
            }
        }
        public DateTime Value
        {
            get
            {
                if (!this.hasValue)
                {
                    throw new InvalidOperationException("Nullable type must have a value.");
                }
                return this._value;
            }
        }
        public static explicit operator DateTime(NullableDateTime nullable)
        {
            if (!nullable.HasValue)
            {
                throw new NullReferenceException();
            }
            return nullable.Value;
        }

        public static implicit operator NullableDateTime(DateTime value)
        {
            return new NullableDateTime(value);
        }

        public static implicit operator NullableDateTime(DBNull value)
        {
            return Default;
        }

        public override string ToString()
        {
            if (this.HasValue)
            {
                return this.Value.ToString();
            }
            return string.Empty;
        }

        public override bool Equals(object obj)
        {
            return (!(!(obj is DBNull) || this.HasValue) || ((obj is NullableDateTime) && this.Equals((NullableDateTime) obj)));
        }

        public bool Equals(NullableDateTime x)
        {
            return Equals(this, x);
        }

        public static bool Equals(NullableDateTime x, NullableDateTime y)
        {
            if (x.HasValue != y.HasValue)
            {
                return false;
            }
            if (x.HasValue)
            {
                return (x.Value == y.Value);
            }
            return true;
        }

        public static bool operator ==(NullableDateTime x, NullableDateTime y)
        {
            return x.Equals(y);
        }

        public static bool operator ==(NullableDateTime x, object y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(NullableDateTime x, NullableDateTime y)
        {
            return !x.Equals(y);
        }

        public static bool operator !=(NullableDateTime x, object y)
        {
            return !x.Equals(y);
        }

        public override int GetHashCode()
        {
            if (this.HasValue)
            {
                return this.Value.GetHashCode();
            }
            return 0;
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            if (this.HasValue)
            {
                return this.Value.ToString(format, formatProvider);
            }
            return string.Empty;
        }

        public int CompareTo(object obj)
        {
            if (obj is NullableDateTime)
            {
                NullableDateTime time = (NullableDateTime) obj;
                if (time.HasValue == this.HasValue)
                {
                    if (this.HasValue)
                    {
                        return this.Value.CompareTo(time.Value);
                    }
                    return 0;
                }
                if (this.HasValue)
                {
                    return 1;
                }
                return -1;
            }
            if (!(obj is DateTime))
            {
                throw new ArgumentException("NullableDateTime can only compare to another NullableDateTime or a System.DateTime");
            }
            DateTime time2 = (DateTime) obj;
            if (this.HasValue)
            {
                return this.Value.CompareTo(time2);
            }
            return -1;
        }

        public static NullableDateTime Parse(string s)
        {
            NullableDateTime time;
            if ((s == null) || (s.Trim().Length == 0))
            {
                return new NullableDateTime();
            }
            try
            {
                time = new NullableDateTime(DateTime.Parse(s));
            }
            catch (Exception exception)
            {
                throw new FormatException("Error parsing '" + s + "' to NullableDateTime.", exception);
            }
            return time;
        }

        static NullableDateTime()
        {
            Default = new NullableDateTime();
        }
    }
}

