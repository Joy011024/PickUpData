namespace FluorineFx.Messaging.Endpoints.Filter
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Context;
    using FluorineFx.Diagnostic;
    using FluorineFx.Exceptions;
    using FluorineFx.Invocation;
    using FluorineFx.IO;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Messaging.Services;
    using log4net;
    using System;
    using System.Collections;
    using System.Data;
    using System.Reflection;
    using System.Text;

    internal class ProcessFilter : AbstractFilter
    {
        private EndpointBase _endpoint;
        private static readonly ILog log = LogManager.GetLogger(typeof(ProcessFilter));

        public ProcessFilter(EndpointBase endpoint)
        {
            this._endpoint = endpoint;
        }

        public override void Invoke(AMFContext context)
        {
            MessageOutput messageOutput = context.MessageOutput;
            for (int i = 0; i < context.AMFMessage.BodyCount; i++)
            {
                AMFBody bodyAt = context.AMFMessage.GetBodyAt(i);
                ResponseBody body = null;
                if (!bodyAt.IsEmptyTarget && !bodyAt.IsDebug)
                {
                    if (bodyAt.IsDescribeService)
                    {
                        body = new ResponseBody {
                            IgnoreResults = bodyAt.IgnoreResults,
                            Target = bodyAt.Response + "/onResult",
                            Response = null
                        };
                        body.Content = new DescribeService(bodyAt).GetDescription();
                        messageOutput.AddBody(body);
                    }
                    else if (messageOutput.GetResponse(bodyAt) == null)
                    {
                        Exception exception2;
                        try
                        {
                            object obj2;
                            RemotingService service = this._endpoint.GetMessageBroker().GetService("remoting-service") as RemotingService;
                            if (service == null)
                            {
                                string message = __Res.GetString("Service_NotFound", new object[] { "remoting-service" });
                                body = new ErrorResponseBody(bodyAt, new FluorineException(message));
                                messageOutput.AddBody(body);
                                if (log.get_IsErrorEnabled())
                                {
                                    log.Error(message);
                                }
                                continue;
                            }
                            Destination destinationWithSource = null;
                            if (destinationWithSource == null)
                            {
                                destinationWithSource = service.GetDestinationWithSource(bodyAt.TypeName);
                            }
                            if (destinationWithSource == null)
                            {
                                destinationWithSource = service.DefaultDestination;
                            }
                            if (destinationWithSource == null)
                            {
                                string str2 = __Res.GetString("Destination_NotFound", new object[] { bodyAt.TypeName });
                                body = new ErrorResponseBody(bodyAt, new FluorineException(str2));
                                messageOutput.AddBody(body);
                                if (log.get_IsErrorEnabled())
                                {
                                    log.Error(str2);
                                }
                                continue;
                            }
                            service.CheckSecurity(destinationWithSource);
                            string source = bodyAt.TypeName + "." + bodyAt.Method;
                            IList parameterList = bodyAt.GetParameterList();
                            string cacheKey = CacheMap.GenerateCacheKey(source, parameterList);
                            if (FluorineConfiguration.Instance.CacheMap.ContainsValue(cacheKey))
                            {
                                obj2 = FluorineConfiguration.Instance.CacheMap.Get(cacheKey);
                                if (obj2 != null)
                                {
                                    if ((log != null) && log.get_IsDebugEnabled())
                                    {
                                        log.Debug(__Res.GetString("Cache_HitKey", new object[] { source, cacheKey }));
                                    }
                                    body = new ResponseBody(bodyAt, obj2);
                                    messageOutput.AddBody(body);
                                    continue;
                                }
                            }
                            FactoryInstance factoryInstance = destinationWithSource.GetFactoryInstance();
                            factoryInstance.Source = bodyAt.TypeName;
                            if (FluorineContext.Current.ActivationMode != null)
                            {
                                factoryInstance.Scope = FluorineContext.Current.ActivationMode;
                            }
                            object obj3 = factoryInstance.Lookup();
                            if (obj3 != null)
                            {
                                string str5;
                                if (!TypeHelper.GetTypeIsAccessible(obj3.GetType()))
                                {
                                    str5 = __Res.GetString("Type_InitError", new object[] { bodyAt.TypeName });
                                    if (log.get_IsErrorEnabled())
                                    {
                                        log.Error(str5);
                                    }
                                    body = new ErrorResponseBody(bodyAt, new FluorineException(str5));
                                    messageOutput.AddBody(body);
                                    continue;
                                }
                                MethodInfo methodInfo = null;
                                if (!bodyAt.IsRecordsetDelivery)
                                {
                                    methodInfo = MethodHandler.GetMethod(obj3.GetType(), bodyAt.Method, bodyAt.GetParameterList());
                                }
                                else
                                {
                                    methodInfo = obj3.GetType().GetMethod(bodyAt.Method);
                                }
                                if (methodInfo != null)
                                {
                                    PagingContext context2;
                                    string str6;
                                    string str7;
                                    string[] strArray2;
                                    int num2;
                                    object[] customAttributes = methodInfo.GetCustomAttributes(typeof(RoleAttribute), true);
                                    if ((customAttributes != null) && (customAttributes.Length == 1))
                                    {
                                        RoleAttribute attribute = customAttributes[0] as RoleAttribute;
                                        string[] roles = attribute.Roles.Split(new char[] { ',' });
                                        if (!service.DoAuthorization(roles))
                                        {
                                            throw new UnauthorizedAccessException(__Res.GetString("Security_AccessNotAllowed"));
                                        }
                                    }
                                    PageSizeAttribute attribute2 = null;
                                    MethodInfo method = null;
                                    object[] objArray2 = methodInfo.GetCustomAttributes(typeof(PageSizeAttribute), true);
                                    if ((objArray2 != null) && (objArray2.Length == 1))
                                    {
                                        attribute2 = objArray2[0] as PageSizeAttribute;
                                        method = obj3.GetType().GetMethod(bodyAt.Method + "Count");
                                        if ((method != null) && (method.ReturnType != typeof(int)))
                                        {
                                            method = null;
                                        }
                                    }
                                    ParameterInfo[] parameters = methodInfo.GetParameters();
                                    object[] array = new object[parameters.Length];
                                    if (!bodyAt.IsRecordsetDelivery)
                                    {
                                        if (array.Length != parameterList.Count)
                                        {
                                            str5 = __Res.GetString("Arg_Mismatch", new object[] { parameterList.Count, methodInfo.Name, array.Length });
                                            if ((log != null) && log.get_IsErrorEnabled())
                                            {
                                                log.Error(str5);
                                            }
                                            body = new ErrorResponseBody(bodyAt, new ArgumentException(str5));
                                            messageOutput.AddBody(body);
                                            continue;
                                        }
                                        parameterList.CopyTo(array, 0);
                                        if (attribute2 != null)
                                        {
                                            context2 = new PagingContext(attribute2.Offset, attribute2.Limit);
                                            PagingContext.SetPagingContext(context2);
                                        }
                                    }
                                    else
                                    {
                                        if (bodyAt.Target.EndsWith(".release"))
                                        {
                                            body = new ResponseBody(bodyAt, null);
                                            messageOutput.AddBody(body);
                                            continue;
                                        }
                                        str6 = parameterList[0] as string;
                                        byte[] bytes = Convert.FromBase64String(bodyAt.GetRecordsetArgs());
                                        str7 = Encoding.UTF8.GetString(bytes);
                                        if ((str7 != null) && (str7 != string.Empty))
                                        {
                                            strArray2 = str7.Split(new char[] { ',' });
                                            num2 = 0;
                                            while (num2 < strArray2.Length)
                                            {
                                                if (strArray2[num2] == string.Empty)
                                                {
                                                    array[num2] = null;
                                                }
                                                else
                                                {
                                                    array[num2] = strArray2[num2];
                                                }
                                                num2++;
                                            }
                                        }
                                        context2 = new PagingContext(Convert.ToInt32(parameterList[1]), Convert.ToInt32(parameterList[2]));
                                        PagingContext.SetPagingContext(context2);
                                    }
                                    TypeHelper.NarrowValues(array, parameters);
                                    try
                                    {
                                        obj2 = new InvocationHandler(methodInfo).Invoke(obj3, array);
                                        if ((FluorineConfiguration.Instance.CacheMap != null) && FluorineConfiguration.Instance.CacheMap.ContainsCacheDescriptor(source))
                                        {
                                            CacheableObject obj4 = new CacheableObject(source, cacheKey, obj2);
                                            FluorineConfiguration.Instance.CacheMap.Add(obj4.Source, obj4.CacheKey, obj4);
                                            obj2 = obj4;
                                        }
                                        body = new ResponseBody(bodyAt, obj2);
                                        if (attribute2 != null)
                                        {
                                            int num3 = 0;
                                            str6 = null;
                                            IList list2 = bodyAt.GetParameterList();
                                            str7 = null;
                                            if (!bodyAt.IsRecordsetDelivery)
                                            {
                                                object[] objArray4 = new object[list2.Count];
                                                list2.CopyTo(objArray4, 0);
                                                str6 = Guid.NewGuid().ToString();
                                                if (method != null)
                                                {
                                                    num3 = (int) method.Invoke(obj3, array);
                                                }
                                                strArray2 = new string[objArray4.Length];
                                                for (num2 = 0; num2 < objArray4.Length; num2++)
                                                {
                                                    if (objArray4[num2] != null)
                                                    {
                                                        strArray2[num2] = objArray4[num2].ToString();
                                                    }
                                                    else
                                                    {
                                                        strArray2[num2] = string.Empty;
                                                    }
                                                }
                                                str7 = string.Join(",", strArray2);
                                                str7 = Convert.ToBase64String(Encoding.UTF8.GetBytes(str7));
                                            }
                                            else
                                            {
                                                str6 = bodyAt.GetParameterList()[0] as string;
                                            }
                                            if (obj2 is DataTable)
                                            {
                                                DataTable table = obj2 as DataTable;
                                                table.ExtendedProperties["TotalCount"] = num3;
                                                table.ExtendedProperties["Service"] = str7 + "/" + bodyAt.Target;
                                                table.ExtendedProperties["RecordsetId"] = str6;
                                                if (bodyAt.IsRecordsetDelivery)
                                                {
                                                    table.ExtendedProperties["Cursor"] = Convert.ToInt32(list2[list2.Count - 2]);
                                                    table.ExtendedProperties["DynamicPage"] = true;
                                                }
                                            }
                                        }
                                    }
                                    catch (UnauthorizedAccessException exception)
                                    {
                                        body = new ErrorResponseBody(bodyAt, exception);
                                    }
                                    catch (Exception exception3)
                                    {
                                        exception2 = exception3;
                                        if ((exception2 is TargetInvocationException) && (exception2.InnerException != null))
                                        {
                                            body = new ErrorResponseBody(bodyAt, exception2.InnerException);
                                        }
                                        else
                                        {
                                            body = new ErrorResponseBody(bodyAt, exception2);
                                        }
                                        if ((log != null) && log.get_IsErrorEnabled())
                                        {
                                            log.Error(exception2.Message, exception2);
                                        }
                                    }
                                }
                                else
                                {
                                    body = new ErrorResponseBody(bodyAt, new MissingMethodException(bodyAt.TypeName, bodyAt.Method));
                                }
                            }
                            else
                            {
                                body = new ErrorResponseBody(bodyAt, new TypeInitializationException(bodyAt.TypeName, null));
                            }
                        }
                        catch (Exception exception4)
                        {
                            exception2 = exception4;
                            if ((log != null) && log.get_IsErrorEnabled())
                            {
                                log.Error(exception2.Message, exception2);
                            }
                            body = new ErrorResponseBody(bodyAt, exception2);
                        }
                        messageOutput.AddBody(body);
                    }
                }
            }
        }
    }
}

