namespace FluorineFx.Remoting
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Invocation;
    using FluorineFx.IO;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Services;
    using log4net;
    using System;
    using System.Collections;
    using System.Reflection;

    internal class RemotingAdapter : ServiceAdapter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RemotingAdapter));

        public override object Invoke(IMessage message)
        {
            object obj2 = null;
            string str3;
            MessageException innerException;
            RemotingMessage message2 = message as RemotingMessage;
            string operation = message2.operation;
            string fullTypeName = base.DestinationSettings.Properties["source"] as string;
            if ((message2.source != null) && (message2.source != string.Empty))
            {
                if (fullTypeName == "*")
                {
                    fullTypeName = message2.source;
                }
                if (fullTypeName != message2.source)
                {
                    str3 = __Res.GetString("Type_MismatchMissingSource", new object[] { message2.source, base.DestinationSettings.Properties["source"] as string });
                    throw new MessageException(str3, new TypeLoadException(str3));
                }
            }
            if (fullTypeName == null)
            {
                throw new TypeInitializationException("null", null);
            }
            string source = fullTypeName + "." + operation;
            IList body = message2.body as IList;
            string cacheKey = CacheMap.GenerateCacheKey(source, body);
            if (FluorineConfiguration.Instance.CacheMap.ContainsValue(cacheKey))
            {
                obj2 = FluorineConfiguration.Instance.CacheMap.Get(cacheKey);
                if (obj2 != null)
                {
                    if ((log != null) && log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("Cache_HitKey", new object[] { operation, cacheKey }));
                    }
                    return obj2;
                }
            }
            FactoryInstance factoryInstance = base.Destination.GetFactoryInstance();
            factoryInstance.Source = fullTypeName;
            object obj3 = factoryInstance.Lookup();
            if (obj3 == null)
            {
                throw new MessageException(new TypeInitializationException(fullTypeName, null));
            }
            Type type = obj3.GetType();
            if (!TypeHelper.GetTypeIsAccessible(type))
            {
                str3 = __Res.GetString("Type_InitError", new object[] { type.FullName });
                throw new MessageException(str3, new TypeLoadException(str3));
            }
            try
            {
                MethodInfo methodInfo = MethodHandler.GetMethod(type, operation, body);
                if (methodInfo == null)
                {
                    throw new MessageException(new MissingMethodException(fullTypeName, operation));
                }
                object[] customAttributes = methodInfo.GetCustomAttributes(typeof(RoleAttribute), true);
                if ((customAttributes != null) && (customAttributes.Length == 1))
                {
                    RoleAttribute attribute = customAttributes[0] as RoleAttribute;
                    string[] roles = attribute.Roles.Split(new char[] { ',' });
                    if (!base.Destination.Service.DoAuthorization(roles))
                    {
                        throw new UnauthorizedAccessException(__Res.GetString("Security_AccessNotAllowed"));
                    }
                }
                ParameterInfo[] parameters = methodInfo.GetParameters();
                object[] array = new object[parameters.Length];
                body.CopyTo(array, 0);
                TypeHelper.NarrowValues(array, parameters);
                obj2 = new InvocationHandler(methodInfo).Invoke(obj3, array);
            }
            catch (TargetInvocationException exception)
            {
                innerException = null;
                if (exception.InnerException is MessageException)
                {
                    innerException = exception.InnerException as MessageException;
                }
                else
                {
                    innerException = new MessageException(exception.InnerException);
                }
                throw innerException;
            }
            catch (Exception exception3)
            {
                innerException = new MessageException(exception3);
                throw innerException;
            }
            if ((FluorineConfiguration.Instance.CacheMap != null) && FluorineConfiguration.Instance.CacheMap.ContainsCacheDescriptor(source))
            {
                CacheableObject obj4 = new CacheableObject(source, cacheKey, obj2);
                FluorineConfiguration.Instance.CacheMap.Add(obj4.Source, obj4.CacheKey, obj4);
                obj2 = obj4;
            }
            return obj2;
        }
    }
}

