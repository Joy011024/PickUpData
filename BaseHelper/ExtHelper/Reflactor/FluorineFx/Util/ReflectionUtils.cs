namespace FluorineFx.Util
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal abstract class ReflectionUtils
    {
        private static Type GenericICollectionType = Type.GetType("System.Collections.Generic.ICollection`1");
        private static Type GenericIDictionaryType = Type.GetType("System.Collections.Generic.IDictionary`2");
        private static Type GenericIListType = Type.GetType("System.Collections.Generic.IList`1");

        protected ReflectionUtils()
        {
        }

        public static bool CanReadMemberValue(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return true;

                case MemberTypes.Property:
                    return ((PropertyInfo) member).CanRead;
            }
            return false;
        }

        public static bool CanSetMemberValue(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return true;

                case MemberTypes.Property:
                    return ((PropertyInfo) member).CanWrite;
            }
            return false;
        }

        public static object CreateGeneric(Type genericTypeDefinition, IList innerTypes, params object[] args)
        {
            ValidationUtils.ArgumentNotNull(genericTypeDefinition, "genericTypeDefinition");
            ValidationUtils.ArgumentNotNullOrEmpty(innerTypes, "innerTypes");
            return Activator.CreateInstance(MakeGenericType(genericTypeDefinition, CollectionUtils.CreateArray(typeof(Type), innerTypes) as Type[]), args);
        }

        public static object CreateGeneric(Type genericTypeDefinition, Type innerType, params object[] args)
        {
            return CreateGeneric(genericTypeDefinition, new Type[] { innerType }, args);
        }

        public static object CreateUnitializedValue(Type type)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            if (IsGenericTypeDefinition(type))
            {
                throw new ArgumentException(string.Format("Type {0} is a generic type definition and cannot be instantiated.", type), "type");
            }
            if ((type.IsClass || type.IsInterface) || (type == typeof(void)))
            {
                return null;
            }
            if (!type.IsValueType)
            {
                throw new ArgumentException(string.Format("Type {0} cannot be instantiated.", type), "type");
            }
            return Activator.CreateInstance(type);
        }

        public static MemberInfo[] FindMembers(Type targetType, MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria)
        {
            ValidationUtils.ArgumentNotNull(targetType, "targetType");
            List<MemberInfo> list = new List<MemberInfo>(targetType.FindMembers(memberType, bindingAttr, filter, filterCriteria));
            if (((memberType & MemberTypes.Field) != 0) && ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default))
            {
                BindingFlags flags = bindingAttr ^ BindingFlags.Public;
                while ((targetType = targetType.BaseType) != null)
                {
                    list.AddRange(targetType.FindMembers(MemberTypes.Field, flags, filter, filterCriteria));
                }
            }
            return list.ToArray();
        }

        public static Attribute GetAttribute(Type type, ICustomAttributeProvider attributeProvider)
        {
            return GetAttribute(type, attributeProvider, true);
        }

        public static Attribute GetAttribute(Type type, ICustomAttributeProvider attributeProvider, bool inherit)
        {
            Attribute[] attributeArray = GetAttributes(type, attributeProvider, inherit);
            return (((attributeArray != null) && (attributeArray.Length > 0)) ? attributeArray[0] : null);
        }

        public static Attribute[] GetAttributes(Type type, ICustomAttributeProvider attributeProvider, bool inherit)
        {
            ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
            return (attributeProvider.GetCustomAttributes(type, inherit) as Attribute[]);
        }

        public static Type GetDictionaryValueType(Type type)
        {
            Type type2;
            ValidationUtils.ArgumentNotNull(type, "type");
            if (IsSubClass(type, GenericIDictionaryType, out type2))
            {
                if (IsGenericTypeDefinition(type2))
                {
                    throw new Exception(string.Format("Type {0} is not a dictionary.", type));
                }
                return GetGenericArguments(type2)[1];
            }
            if (!typeof(IDictionary).IsAssignableFrom(type))
            {
                throw new Exception(string.Format("Type {0} is not a dictionary.", type));
            }
            return null;
        }

        public static MemberInfo[] GetFieldsAndProperties(Type type, BindingFlags bindingAttr)
        {
            List<MemberInfo> list = new List<MemberInfo>();
            list.AddRange(type.GetFields(bindingAttr));
            list.AddRange(type.GetProperties(bindingAttr));
            return list.ToArray();
        }

        public static Type[] GetGenericArguments(Type type)
        {
            return type.GetGenericArguments();
        }

        public static Type GetGenericTypeDefinition(Type type)
        {
            return type.GetGenericTypeDefinition();
        }

        public static Type GetListItemType(Type type)
        {
            Type type2;
            ValidationUtils.ArgumentNotNull(type, "type");
            if (type.IsArray)
            {
                return type.GetElementType();
            }
            if (IsSubClass(type, GenericIListType, out type2))
            {
                if (IsGenericTypeDefinition(type2))
                {
                    throw new Exception(string.Format("Type {0} is not a list.", type));
                }
                return GetGenericArguments(type2)[0];
            }
            if (!typeof(IList).IsAssignableFrom(type))
            {
                throw new Exception(string.Format("Type {0} is not a list.", type));
            }
            return null;
        }

        public static MemberInfo GetMember(Type type, string name, MemberTypes memberTypes)
        {
            return GetMember(type, name, memberTypes, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        }

        public static MemberInfo GetMember(Type type, string name, MemberTypes memberTypes, BindingFlags bindingAttr)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            ValidationUtils.ArgumentNotNull(name, "name");
            return (CollectionUtils.GetSingleItem(type.GetMember(name, memberTypes, bindingAttr)) as MemberInfo);
        }

        public static Type GetMemberUnderlyingType(MemberInfo member)
        {
            ValidationUtils.ArgumentNotNull(member, "member");
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo) member).EventHandlerType;

                case MemberTypes.Field:
                    return ((FieldInfo) member).FieldType;

                case MemberTypes.Property:
                    return ((PropertyInfo) member).PropertyType;
            }
            throw new ArgumentException("MemberInfo must be if type FieldInfo, PropertyInfo or EventInfo", "member");
        }

        public static object GetMemberValue(MemberInfo member, object target)
        {
            ValidationUtils.ArgumentNotNull(member, "member");
            ValidationUtils.ArgumentNotNull(target, "target");
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo) member).GetValue(target);

                case MemberTypes.Property:
                    try
                    {
                        return ((PropertyInfo) member).GetValue(target, null);
                    }
                    catch (TargetParameterCountException exception)
                    {
                        throw new ArgumentException(string.Format("MemberInfo '{0}' has index parameters", member.Name), exception);
                    }
                    break;
            }
            throw new ArgumentException(string.Format("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo", member.Name), "member");
        }

        public static string GetNameAndAssemblyName(Type t)
        {
            ValidationUtils.ArgumentNotNull(t, "t");
            return (t.FullName + ", " + t.Assembly.GetName().Name);
        }

        public static Type GetObjectType(object v)
        {
            return ((v != null) ? v.GetType() : null);
        }

        public static TypeConverter GetTypeConverter(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return TypeDescriptor.GetConverter(obj);
        }

        public static bool HasDefaultConstructor(Type t)
        {
            ValidationUtils.ArgumentNotNull(t, "t");
            return (t.IsValueType || (t.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null) != null));
        }

        public static bool ImplementsInterface(Type type, string interfaceName)
        {
            return (type.GetInterface(interfaceName, true) != null);
        }

        public static bool IsGenericType(Type type)
        {
            return type.IsGenericType;
        }

        public static bool IsGenericTypeDefinition(Type type)
        {
            return type.IsGenericTypeDefinition;
        }

        public static bool IsIndexedProperty(MemberInfo member)
        {
            ValidationUtils.ArgumentNotNull(member, "member");
            PropertyInfo property = member as PropertyInfo;
            return ((property != null) && IsIndexedProperty(property));
        }

        public static bool IsIndexedProperty(PropertyInfo property)
        {
            ValidationUtils.ArgumentNotNull(property, "property");
            return (property.GetIndexParameters().Length > 0);
        }

        public static bool IsInstantiatableType(Type t)
        {
            ValidationUtils.ArgumentNotNull(t, "t");
            if (((t.IsAbstract || t.IsInterface) || (t.IsArray || IsGenericTypeDefinition(t))) || (t == typeof(void)))
            {
                return false;
            }
            if (!HasDefaultConstructor(t))
            {
                return false;
            }
            return true;
        }

        public static bool IsNullable(Type type)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            return (type.IsValueType && (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>))));
        }

        public static bool IsPropertyIndexed(PropertyInfo property)
        {
            ValidationUtils.ArgumentNotNull(property, "property");
            return !CollectionUtils.IsNullOrEmpty<ParameterInfo>(property.GetIndexParameters());
        }

        public static bool IsSubClass(Type type, Type check)
        {
            Type type2;
            return IsSubClass(type, check, out type2);
        }

        public static bool IsSubClass(Type type, Type check, out Type implementingType)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            ValidationUtils.ArgumentNotNull(check, "check");
            return IsSubClassInternal(type, type, check, out implementingType);
        }

        private static bool IsSubClassInternal(Type initialType, Type currentType, Type check, out Type implementingType)
        {
            if (currentType == check)
            {
                implementingType = currentType;
                return true;
            }
            if (check.IsInterface && (initialType.IsInterface || (currentType == initialType)))
            {
                foreach (Type type in currentType.GetInterfaces())
                {
                    if (IsSubClassInternal(initialType, type, check, out implementingType))
                    {
                        if (check == implementingType)
                        {
                            implementingType = currentType;
                        }
                        return true;
                    }
                }
            }
            if ((IsGenericType(currentType) && !IsGenericTypeDefinition(currentType)) && IsSubClassInternal(initialType, GetGenericTypeDefinition(currentType), check, out implementingType))
            {
                implementingType = currentType;
                return true;
            }
            if (currentType.BaseType == null)
            {
                implementingType = null;
                return false;
            }
            return IsSubClassInternal(initialType, currentType.BaseType, check, out implementingType);
        }

        public static bool IsUnitializedValue(object value)
        {
            if (value == null)
            {
                return true;
            }
            object obj2 = CreateUnitializedValue(value.GetType());
            return value.Equals(obj2);
        }

        public static bool ItemsUnitializedValue<T>(IList<T> list)
        {
            int num;
            ValidationUtils.ArgumentNotNull(list, "list");
            Type listItemType = GetListItemType(list.GetType());
            if (listItemType.IsValueType)
            {
                object obj2 = CreateUnitializedValue(listItemType);
                for (num = 0; num < list.Count; num++)
                {
                    T local = list[num];
                    if (!local.Equals(obj2))
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (!listItemType.IsClass)
                {
                    throw new Exception(string.Format("Type {0} is neither a ValueType or a Class.", listItemType));
                }
                for (num = 0; num < list.Count; num++)
                {
                    object obj3 = list[num];
                    if (obj3 != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        internal static Type MakeGenericType(Type genericTypeDefinition, params Type[] typeArguments)
        {
            ValidationUtils.ArgumentNotNull(genericTypeDefinition, "genericTypeDefinition");
            ValidationUtils.ArgumentNotNullOrEmpty<Type>(typeArguments, "typeArguments");
            ValidationUtils.ArgumentConditionTrue(IsGenericTypeDefinition(genericTypeDefinition), "genericTypeDefinition", string.Format("Type {0} is not a generic type definition.", genericTypeDefinition));
            return genericTypeDefinition.MakeGenericType(typeArguments);
        }

        public static void SetMemberValue(MemberInfo member, object target, object value)
        {
            ValidationUtils.ArgumentNotNull(member, "member");
            ValidationUtils.ArgumentNotNull(target, "target");
            MemberTypes memberType = member.MemberType;
            if (memberType != MemberTypes.Field)
            {
                if (memberType != MemberTypes.Property)
                {
                    throw new ArgumentException(string.Format("MemberInfo '{0}' must be of type FieldInfo or PropertyInfo", member.Name), "member");
                }
            }
            else
            {
                ((FieldInfo) member).SetValue(target, value);
                return;
            }
            ((PropertyInfo) member).SetValue(target, value, null);
        }
    }
}

