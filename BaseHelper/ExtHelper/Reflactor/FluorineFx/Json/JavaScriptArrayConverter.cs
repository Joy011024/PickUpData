namespace FluorineFx.Json
{
    using FluorineFx;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;

    public class JavaScriptArrayConverter : ArrayConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            return (destinationType.IsArray || ((destinationType == typeof(ArrayList)) || ((destinationType == typeof(IList)) || ((destinationType.GetInterface("System.Collections.IList") != null) || ((destinationType.GetInterface("System.Collections.Generic.ICollection`1") != null) || base.CanConvertTo(context, destinationType))))));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object obj2;
            IList list;
            int num;
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if (destinationType.IsArray)
            {
                return (value as JavaScriptArray).ToArray();
            }
            if (destinationType == typeof(ArrayList))
            {
                return (value as ArrayList);
            }
            if (destinationType == typeof(IList))
            {
                return (value as IList);
            }
            if (destinationType.GetInterface("System.Collections.Generic.ICollection`1") != null)
            {
                obj2 = TypeHelper.CreateInstance(destinationType);
                MethodInfo method = destinationType.GetMethod("Add");
                if (method != null)
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if ((parameters != null) && (parameters.Length == 1))
                    {
                        Type parameterType = parameters[0].ParameterType;
                        list = (IList) value;
                        for (num = 0; num < list.Count; num++)
                        {
                            method.Invoke(obj2, new object[] { TypeHelper.ChangeType(list[num], parameterType) });
                        }
                        return obj2;
                    }
                }
            }
            if (destinationType.GetInterface("System.Collections.IList") != null)
            {
                obj2 = TypeHelper.CreateInstance(destinationType);
                list = obj2 as IList;
                for (num = 0; num < (value as IList).Count; num++)
                {
                    list.Add((value as IList)[num]);
                }
                return obj2;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

