namespace FluorineFx.AMF3
{
    using FluorineFx;
    using FluorineFx.Util;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;

    public class ArrayCollectionConverter : ArrayConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            return ((destinationType == typeof(ArrayCollection)) || (destinationType.IsArray || ((destinationType == typeof(ArrayList)) || ((destinationType == typeof(IList)) || ((destinationType.GetInterface("System.Collections.IList", false) != null) || ((destinationType.GetInterface("System.Collections.Generic.ICollection`1", false) != null) || base.CanConvertTo(context, destinationType)))))));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object obj2;
            IList list;
            int num;
            ArrayCollection arrays = value as ArrayCollection;
            ValidationUtils.ArgumentNotNull(arrays, "value");
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if (destinationType == typeof(ArrayCollection))
            {
                return value;
            }
            if (destinationType.IsArray)
            {
                return arrays.ToArray();
            }
            if (destinationType == typeof(ArrayList))
            {
                if (arrays.List is ArrayList)
                {
                    return arrays.List;
                }
                return ArrayList.Adapter(arrays.List);
            }
            if (destinationType == typeof(IList))
            {
                return arrays.List;
            }
            if (destinationType.GetInterface("System.Collections.Generic.ICollection`1", false) != null)
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
            if (destinationType.GetInterface("System.Collections.IList", false) != null)
            {
                obj2 = TypeHelper.CreateInstance(destinationType);
                list = obj2 as IList;
                for (num = 0; num < arrays.List.Count; num++)
                {
                    list.Add(arrays.List[num]);
                }
                return obj2;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

