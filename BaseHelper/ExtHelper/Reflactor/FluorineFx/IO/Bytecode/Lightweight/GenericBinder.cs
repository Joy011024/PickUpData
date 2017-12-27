namespace FluorineFx.IO.Bytecode.Lightweight
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [Serializable]
    internal class GenericBinder : Binder
    {
        private static GenericBinder _generic;
        private readonly bool _genericMethodDefinition;
        private static GenericBinder _nonGeneric;

        public GenericBinder(bool genericMethodDefinition)
        {
            this._genericMethodDefinition = genericMethodDefinition;
        }

        public override FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, CultureInfo culture)
        {
            throw new NotImplementedException("GenericBinder.BindToField");
        }

        public override MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] match, ref object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] names, out object state)
        {
            throw new NotImplementedException("GenericBinder.BindToMethod");
        }

        public override object ChangeType(object value, Type type, CultureInfo culture)
        {
            throw new NotImplementedException("GenericBinder.ChangeType");
        }

        private static bool CheckGenericTypeConstraints(Type genType, Type parameterType)
        {
            Type[] genericParameterConstraints = genType.GetGenericParameterConstraints();
            for (int i = 0; i < genericParameterConstraints.Length; i++)
            {
                if (!genericParameterConstraints[i].IsAssignableFrom(parameterType))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool CompareGenericTypesRecursive(Type genType, Type specType)
        {
            Type[] genericArguments = genType.GetGenericArguments();
            Type[] typeArray2 = specType.GetGenericArguments();
            bool flag = genericArguments.Length == typeArray2.Length;
            for (int i = 0; flag && (i < genericArguments.Length); i++)
            {
                if (genericArguments[i] != typeArray2[i])
                {
                    if (genericArguments[i].IsGenericParameter)
                    {
                        flag = CheckGenericTypeConstraints(genericArguments[i], typeArray2[i]);
                    }
                    else if (genericArguments[i].IsGenericType && typeArray2[i].IsGenericType)
                    {
                        flag = CompareGenericTypesRecursive(genericArguments[i], typeArray2[i]);
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        public override void ReorderArgumentArray(ref object[] args, object state)
        {
            throw new NotImplementedException("GenericBinder.ReorderArgumentArray");
        }

        public override MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] matchMethods, Type[] parameterTypes, ParameterModifier[] modifiers)
        {
            for (int i = 0; i < matchMethods.Length; i++)
            {
                if (matchMethods[i].IsGenericMethodDefinition == this._genericMethodDefinition)
                {
                    ParameterInfo[] parameters = matchMethods[i].GetParameters();
                    bool flag = parameters.Length == parameterTypes.Length;
                    for (int j = 0; flag && (j < parameters.Length); j++)
                    {
                        if (parameters[j].ParameterType != parameterTypes[j])
                        {
                            if (parameters[j].ParameterType.IsGenericParameter)
                            {
                                flag = CheckGenericTypeConstraints(parameters[j].ParameterType, parameterTypes[j]);
                            }
                            else if (parameters[j].ParameterType.IsGenericType && parameterTypes[j].IsGenericType)
                            {
                                flag = CompareGenericTypesRecursive(parameters[j].ParameterType, parameterTypes[j]);
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }
                    if (flag)
                    {
                        return matchMethods[i];
                    }
                }
            }
            return null;
        }

        public override PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers)
        {
            throw new NotImplementedException("GenericBinder.SelectProperty");
        }

        public static GenericBinder Generic
        {
            get
            {
                return (_generic ?? (_generic = new GenericBinder(true)));
            }
        }

        public static GenericBinder NonGeneric
        {
            get
            {
                return (_nonGeneric ?? (_nonGeneric = new GenericBinder(false)));
            }
        }
    }
}

