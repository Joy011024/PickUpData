namespace FluorineFx.Util.Nullables
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;
    using System.Reflection;

    public class NullableDateTimeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(string)) || ((sourceType == typeof(DateTime)) || ((sourceType == typeof(DBNull)) || base.CanConvertFrom(context, sourceType))));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(InstanceDescriptor)) || ((destinationType == typeof(DateTime)) || base.CanConvertTo(context, destinationType)));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                return NullableDateTime.Default;
            }
            if (value is DateTime)
            {
                return new NullableDateTime((DateTime) value);
            }
            if (value is DBNull)
            {
                return NullableDateTime.Default;
            }
            if (value is string)
            {
                string text = ((string) value).Trim();
                if (text == string.Empty)
                {
                    return NullableDateTime.Default;
                }
                return new NullableDateTime((DateTime) TypeDescriptor.GetConverter(typeof(DateTime)).ConvertFromString(context, culture, text));
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((destinationType == typeof(InstanceDescriptor)) && (value is NullableDateTime))
            {
                NullableDateTime time = (NullableDateTime) value;
                Type[] types = new Type[] { typeof(DateTime) };
                ConstructorInfo constructor = typeof(NullableDateTime).GetConstructor(types);
                if (constructor != null)
                {
                    return new InstanceDescriptor(constructor, new object[] { time.Value });
                }
            }
            else if (destinationType == typeof(DateTime))
            {
                NullableDateTime time2 = (NullableDateTime) value;
                if (time2.HasValue)
                {
                    return time2.Value;
                }
                return DBNull.Value;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return new NullableDateTime((DateTime) propertyValues["Value"]);
        }

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(NullableDateTime), attributes);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}

