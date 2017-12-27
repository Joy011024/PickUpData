namespace FluorineFx.Invocation
{
    using FluorineFx;
    using log4net;
    using System;
    using System.Reflection;

    internal class InvocationHandler
    {
        private MethodInfo _methodInfo;
        private static readonly ILog log = LogManager.GetLogger(typeof(InvocationHandler));

        public InvocationHandler(MethodInfo methodInfo)
        {
            this._methodInfo = methodInfo;
        }

        public object Invoke(object obj, object[] arguments)
        {
            if (log.get_IsDebugEnabled())
            {
                log.Debug(__Res.GetString("Invoke_Method", new object[] { this._methodInfo.DeclaringType.FullName + "." + this._methodInfo.Name }));
            }
            object result = this._methodInfo.Invoke(obj, arguments);
            object[] customAttributes = this._methodInfo.GetCustomAttributes(false);
            if ((customAttributes != null) && (customAttributes.Length > 0))
            {
                int num;
                Attribute attribute;
                InvocationManager invocationManager = new InvocationManager {
                    Result = result
                };
                for (num = 0; num < customAttributes.Length; num++)
                {
                    attribute = customAttributes[num] as Attribute;
                    if (attribute is IInvocationCallback)
                    {
                        (attribute as IInvocationCallback).OnInvoked(invocationManager, this._methodInfo, obj, arguments, result);
                    }
                }
                for (num = 0; num < customAttributes.Length; num++)
                {
                    attribute = customAttributes[num] as Attribute;
                    if (attribute is IInvocationResultHandler)
                    {
                        (attribute as IInvocationResultHandler).HandleResult(invocationManager, this._methodInfo, obj, arguments, result);
                    }
                }
                return invocationManager.Result;
            }
            return result;
        }
    }
}

