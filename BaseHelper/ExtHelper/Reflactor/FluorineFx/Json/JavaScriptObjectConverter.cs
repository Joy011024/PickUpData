namespace FluorineFx.Json
{
    using FluorineFx;
    using FluorineFx.Util;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;

    public class JavaScriptObjectConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if (destinationType.IsArray)
            {
                return false;
            }
            if (destinationType.IsEnum)
            {
                return false;
            }
            if (destinationType.IsPrimitive)
            {
                return false;
            }
            if (destinationType.IsValueType)
            {
                return false;
            }
            return true;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            JavaScriptObject d = value as JavaScriptObject;
            if ((destinationType == typeof(object)) || (destinationType == typeof(JavaScriptObject)))
            {
                return value;
            }
            if (destinationType == typeof(Hashtable))
            {
                return new Hashtable(d);
            }
            if (destinationType == typeof(ASObject))
            {
                return new ASObject(d);
            }
            if (ReflectionUtils.ImplementsInterface(destinationType, "System.Collections.Generic.IDictionary`2"))
            {
                return TypeHelper.ChangeType(d, destinationType);
            }
            if (ReflectionUtils.ImplementsInterface(destinationType, "System.Collections.IDictionary"))
            {
                return TypeHelper.ChangeType(d, destinationType);
            }
            object target = Activator.CreateInstance(destinationType);
            foreach (KeyValuePair<string, object> pair in value as JavaScriptObject)
            {
                string key = pair.Key;
                MemberInfo member = ReflectionUtils.GetMember(destinationType, key, MemberTypes.Property | MemberTypes.Field);
                if (member != null)
                {
                    object obj4 = TypeHelper.ChangeType(pair.Value, ReflectionUtils.GetMemberUnderlyingType(member));
                    ReflectionUtils.SetMemberValue(member, target, obj4);
                }
            }
            return target;
        }
    }
}

