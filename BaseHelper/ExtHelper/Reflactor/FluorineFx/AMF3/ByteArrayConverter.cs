namespace FluorineFx.AMF3
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class ByteArrayConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(byte[])) || base.CanConvertTo(context, destinationType));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(byte[]))
            {
                return (value as ByteArray).MemoryStream.ToArray();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

