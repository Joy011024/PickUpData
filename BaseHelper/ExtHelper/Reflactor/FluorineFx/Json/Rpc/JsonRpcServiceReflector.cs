namespace FluorineFx.Json.Rpc
{
    using FluorineFx;
    using FluorineFx.Collections;
    using FluorineFx.Json.Services;
    using FluorineFx.Util;
    using log4net;
    using System;

    internal sealed class JsonRpcServiceReflector
    {
        private static readonly CopyOnWriteDictionary _classByTypeCache = new CopyOnWriteDictionary();
        private static readonly ILog log = LogManager.GetLogger(typeof(JsonRpcServiceReflector));

        private JsonRpcServiceReflector()
        {
            throw new NotSupportedException();
        }

        private static ServiceClass BuildFromType(Type type)
        {
            if (TypeHelper.GetTypeIsAccessible(type))
            {
                return new ServiceClass(type);
            }
            string message = __Res.GetString("Type_InitError", new object[] { type.FullName });
            if (log.get_IsErrorEnabled())
            {
                log.Error(message);
            }
            throw new TypeLoadException(message);
        }

        public static ServiceClass FromType(Type type)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            ServiceClass class2 = (ServiceClass) _classByTypeCache[type];
            if (class2 == null)
            {
                class2 = BuildFromType(type);
                _classByTypeCache[type] = class2;
            }
            return class2;
        }
    }
}

