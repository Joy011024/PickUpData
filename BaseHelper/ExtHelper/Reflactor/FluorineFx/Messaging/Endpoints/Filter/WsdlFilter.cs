namespace FluorineFx.Messaging.Endpoints.Filter
{
    using FluorineFx;
    using FluorineFx.IO;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.Reflection;

    internal class WsdlFilter : AbstractFilter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WsdlFilter));

        private Type GetTypeForWebService(string webService)
        {
            if ((log != null) && log.get_IsInfoEnabled())
            {
                log.Info(__Res.GetString("Wsdl_ProxyGen", new object[] { webService }));
            }
            Assembly assemblyFromAsmx = WsdlHelper.GetAssemblyFromAsmx(webService);
            if (assemblyFromAsmx != null)
            {
                Type[] types = assemblyFromAsmx.GetTypes();
                if (types.Length > 0)
                {
                    return types[0];
                }
            }
            return null;
        }

        public override void Invoke(AMFContext context)
        {
            for (int i = 0; i < context.AMFMessage.BodyCount; i++)
            {
                Type typeForWebService;
                Exception exception;
                ErrorResponseBody body2;
                AMFBody bodyAt = context.AMFMessage.GetBodyAt(i);
                if (!bodyAt.IsEmptyTarget)
                {
                    if (bodyAt.IsWebService)
                    {
                        try
                        {
                            typeForWebService = this.GetTypeForWebService(bodyAt.TypeName);
                            if (typeForWebService != null)
                            {
                                bodyAt.Target = typeForWebService.FullName + "." + bodyAt.Method;
                            }
                            else
                            {
                                exception = new TypeInitializationException(bodyAt.TypeName, null);
                                if ((log != null) && log.get_IsErrorEnabled())
                                {
                                    log.Error(__Res.GetString("Type_InitError", new object[] { bodyAt.Target }), exception);
                                }
                                body2 = new ErrorResponseBody(bodyAt, exception);
                                context.MessageOutput.AddBody(body2);
                            }
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            if ((log != null) && log.get_IsErrorEnabled())
                            {
                                log.Error(__Res.GetString("Wsdl_ProxyGen", new object[] { bodyAt.Target }), exception);
                            }
                            body2 = new ErrorResponseBody(bodyAt, exception);
                            context.MessageOutput.AddBody(body2);
                        }
                    }
                }
                else
                {
                    object content = bodyAt.Content;
                    if (content is IList)
                    {
                        content = (content as IList)[0];
                    }
                    IMessage message = content as IMessage;
                    if ((message != null) && (message is RemotingMessage))
                    {
                        RemotingMessage message2 = message as RemotingMessage;
                        string source = message2.source;
                        if ((source != null) && source.ToLower().EndsWith(".asmx"))
                        {
                            try
                            {
                                typeForWebService = this.GetTypeForWebService(source);
                                if (typeForWebService != null)
                                {
                                    message2.source = typeForWebService.FullName;
                                }
                                else
                                {
                                    exception = new TypeInitializationException(source, null);
                                    if ((log != null) && log.get_IsErrorEnabled())
                                    {
                                        log.Error(__Res.GetString("Type_InitError", new object[] { source }), exception);
                                    }
                                    body2 = new ErrorResponseBody(bodyAt, message, exception);
                                    context.MessageOutput.AddBody(body2);
                                }
                            }
                            catch (Exception exception2)
                            {
                                exception = exception2;
                                if ((log != null) && log.get_IsErrorEnabled())
                                {
                                    log.Error(__Res.GetString("Wsdl_ProxyGen", new object[] { source }), exception);
                                }
                                body2 = new ErrorResponseBody(bodyAt, message, exception);
                                context.MessageOutput.AddBody(body2);
                            }
                        }
                    }
                }
            }
        }
    }
}

