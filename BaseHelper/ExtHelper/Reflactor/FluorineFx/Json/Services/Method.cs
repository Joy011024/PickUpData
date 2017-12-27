namespace FluorineFx.Json.Services
{
    using FluorineFx.Util;
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;

    public sealed class Method
    {
        private string _description;
        private string _name;
        private string[] _parameterNames;
        private Parameter[] _parameters;
        private Type _resultType;
        private Parameter[] _sortedParameters;

        internal Method()
        {
        }

        public static Method FromMethodInfo(MethodInfo methodInfo)
        {
            Method method = new Method {
                _name = methodInfo.Name,
                _resultType = methodInfo.ReturnType,
                _description = null,
                _parameters = new Parameter[methodInfo.GetParameters().Length],
                _parameterNames = new string[methodInfo.GetParameters().Length]
            };
            foreach (ParameterInfo info in methodInfo.GetParameters())
            {
                Parameter parameter = Parameter.FromParameterInfo(info);
                int position = parameter.Position;
                method._parameters[position] = parameter;
                method._parameterNames[position] = parameter.Name;
            }
            method._sortedParameters = (Parameter[]) method._parameters.Clone();
            InvariantStringArray.Sort(method._parameterNames, method._sortedParameters);
            return method;
        }

        public Parameter[] GetParameters()
        {
            return this._parameters;
        }

        private object[] MapArguments(string[] names, object[] args)
        {
            Debug.Assert(names != null);
            Debug.Assert(args != null);
            Debug.Assert(names.Length == args.Length);
            object[] objArray = new object[this._parameters.Length];
            for (int i = 0; i < names.Length; i++)
            {
                string s = names[i];
                if ((s != null) && (s.Length != 0))
                {
                    object obj2 = args[i];
                    if (obj2 != null)
                    {
                        int index = -1;
                        if (s.Length <= 2)
                        {
                            char ch;
                            char ch2;
                            if (s.Length == 2)
                            {
                                ch = s[0];
                                ch2 = s[1];
                            }
                            else
                            {
                                ch = '0';
                                ch2 = s[0];
                            }
                            if ((((ch >= '0') && (ch <= '9')) && (ch2 >= '0')) && (ch2 <= '9'))
                            {
                                index = int.Parse(s, NumberStyles.Number, CultureInfo.InvariantCulture);
                                if (index < this._parameters.Length)
                                {
                                    objArray[index] = obj2;
                                }
                            }
                        }
                        if (index < 0)
                        {
                            int num3 = InvariantStringArray.BinarySearch(this._parameterNames, s);
                            if (num3 >= 0)
                            {
                                index = this._sortedParameters[num3].Position;
                            }
                        }
                        if (index >= 0)
                        {
                            objArray[index] = obj2;
                        }
                    }
                }
            }
            return objArray;
        }

        public object[] TransposeVariableArguments(object[] args)
        {
            if (!this.HasParamArray)
            {
                return args;
            }
            int length = this._parameters.Length;
            object[] destinationArray = null;
            if (args.Length == length)
            {
                object obj2 = args[args.Length - 1];
                if (obj2 != null)
                {
                    destinationArray = obj2 as object[];
                    if (destinationArray == null)
                    {
                        Array sourceArray = obj2 as Array;
                        if ((sourceArray != null) && (sourceArray.GetType().GetArrayRank() == 1))
                        {
                            destinationArray = new object[sourceArray.Length];
                            Array.Copy(sourceArray, destinationArray, destinationArray.Length);
                        }
                    }
                }
            }
            if (destinationArray == null)
            {
                destinationArray = new object[(args.Length - length) + 1];
                Array.Copy(args, length - 1, destinationArray, 0, destinationArray.Length);
            }
            object[] objArray2 = new object[length];
            Array.Copy(args, objArray2, (int) (length - 1));
            objArray2[objArray2.Length - 1] = destinationArray;
            return objArray2;
        }

        private static bool TypesMatch(Type expected, Type actual)
        {
            Debug.Assert(expected != null);
            Debug.Assert(actual != null);
            return (expected.IsSealed ? expected.Equals(actual) : expected.IsAssignableFrom(actual));
        }

        public string Description
        {
            get
            {
                return this._description;
            }
        }

        public bool HasParamArray
        {
            get
            {
                return ((this._parameters.Length > 0) && this._parameters[this._parameters.Length - 1].IsParamArray);
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public Type ResultType
        {
            get
            {
                return this._resultType;
            }
        }
    }
}

