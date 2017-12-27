namespace FluorineFx
{
    using FluorineFx.Exceptions;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public sealed class MethodHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MethodHandler));

        private MethodHandler()
        {
        }

        public static MethodInfo GetMethod(Type type, string methodName, IList arguments)
        {
            return GetMethod(type, methodName, arguments, false);
        }

        public static MethodInfo GetMethod(Type type, string methodName, IList arguments, bool exactMatch)
        {
            return GetMethod(type, methodName, arguments, exactMatch, true);
        }

        public static MethodInfo GetMethod(Type type, string methodName, IList arguments, bool exactMatch, bool throwException)
        {
            return GetMethod(type, methodName, arguments, exactMatch, throwException, true);
        }

        public static MethodInfo GetMethod(Type type, string methodName, IList arguments, bool exactMatch, bool throwException, bool traceError)
        {
            int num;
            MethodInfo info;
            int num2;
            string str;
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            List<MethodInfo> list = new List<MethodInfo>();
            for (num = 0; num < methods.Length; num++)
            {
                info = methods[num];
                if ((info.Name == methodName) && (((info.GetParameters().Length == 0) && (arguments == null)) || ((arguments != null) && (info.GetParameters().Length == arguments.Count))))
                {
                    list.Add(info);
                }
            }
            if (list.Count > 0)
            {
                for (num = list.Count - 1; num >= 0; num--)
                {
                    info = list[num];
                    ParameterInfo[] parameters = info.GetParameters();
                    bool flag = true;
                    num2 = 0;
                    while (num2 < parameters.Length)
                    {
                        ParameterInfo info2 = parameters[num2];
                        if (!exactMatch)
                        {
                            if (!TypeHelper.IsAssignable(arguments[num2], info2.ParameterType))
                            {
                                flag = false;
                                break;
                            }
                        }
                        else if ((arguments[num2] == null) || (arguments[num2].GetType() != info2.ParameterType))
                        {
                            flag = false;
                            break;
                        }
                        num2++;
                    }
                    if (!flag)
                    {
                        list.Remove(info);
                    }
                }
            }
            if (list.Count == 0)
            {
                str = __Res.GetString("Invocation_NoSuitableMethod", new object[] { methodName });
                if (traceError && log.get_IsErrorEnabled())
                {
                    log.Error(str);
                    for (num2 = 0; (arguments != null) && (num2 < arguments.Count); num2++)
                    {
                        string str2;
                        object obj2 = arguments[num2];
                        if (obj2 != null)
                        {
                            str2 = __Res.GetString("Invocation_ParameterType", new object[] { num2, obj2.GetType().FullName });
                        }
                        else
                        {
                            str2 = __Res.GetString("Invocation_ParameterType", new object[] { num2, "null" });
                        }
                        log.Error(str2);
                    }
                }
                if (throwException)
                {
                    throw new FluorineException(str);
                }
            }
            if (list.Count > 1)
            {
                str = __Res.GetString("Invocation_Ambiguity", new object[] { methodName });
                if (throwException)
                {
                    throw new FluorineException(str);
                }
            }
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }
    }
}

