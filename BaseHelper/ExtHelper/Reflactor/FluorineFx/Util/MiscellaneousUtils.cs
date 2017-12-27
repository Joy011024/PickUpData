namespace FluorineFx.Util
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;

    internal abstract class MiscellaneousUtils
    {
        protected MiscellaneousUtils()
        {
        }

        public static string GetDescription(object o)
        {
            ValidationUtils.ArgumentNotNull(o, "o");
            ICustomAttributeProvider attributeProvider = o as ICustomAttributeProvider;
            if (attributeProvider == null)
            {
                Type type = o.GetType();
                if (type.IsEnum)
                {
                    attributeProvider = type.GetField(o.ToString());
                }
                else
                {
                    attributeProvider = type;
                }
            }
            DescriptionAttribute attribute = ReflectionUtils.GetAttribute(typeof(DescriptionAttribute), attributeProvider) as DescriptionAttribute;
            if (attribute == null)
            {
                throw new Exception(string.Format("No DescriptionAttribute on '{0}'.", o.GetType()));
            }
            return attribute.Description;
        }

        public static string[] GetDescriptions(IList values)
        {
            ValidationUtils.ArgumentNotNull(values, "values");
            string[] strArray = new string[values.Count];
            for (int i = 0; i < values.Count; i++)
            {
                strArray[i] = GetDescription(values[i]);
            }
            return strArray;
        }

        public static string ToString(object value)
        {
            return ((value != null) ? value.ToString() : "{null}");
        }
    }
}

