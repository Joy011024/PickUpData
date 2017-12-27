namespace FluorineFx.Messaging.Endpoints.Filter
{
    using FluorineFx;
    using FluorineFx.Context;
    using FluorineFx.IO;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Security;
    using log4net;
    using System;
    using System.Collections;
    using System.Security.Principal;
    using System.Threading;

    internal class AuthenticationFilter : AbstractFilter
    {
        private EndpointBase _endpoint;
        private static readonly ILog log = LogManager.GetLogger(typeof(AuthenticationFilter));

        public AuthenticationFilter(EndpointBase endpoint)
        {
            this._endpoint = endpoint;
        }

        public override void Invoke(AMFContext context)
        {
            IPrincipal principal = null;
            int num;
            ErrorResponseBody body2;
            MessageBroker messageBroker = this._endpoint.GetMessageBroker();
            try
            {
                string str3;
                AMFHeader header = context.AMFMessage.GetHeader("Credentials");
                if ((header != null) && (header.Content != null))
                {
                    string username = ((ASObject) header.Content)["userid"] as string;
                    string password = ((ASObject) header.Content)["password"] as string;
                    ASObject content = new ASObject();
                    content["name"] = "Credentials";
                    content["mustUnderstand"] = false;
                    content["data"] = null;
                    AMFHeader header2 = new AMFHeader("RequestPersistentHeader", true, content);
                    context.MessageOutput.AddHeader(header2);
                    ILoginCommand loginCommand = this._endpoint.GetMessageBroker().LoginCommand;
                    if (loginCommand == null)
                    {
                        throw new UnauthorizedAccessException(__Res.GetString("Security_LoginMissing"));
                    }
                    Hashtable credentials = new Hashtable(1);
                    credentials["password"] = password;
                    principal = loginCommand.DoAuthentication(username, credentials);
                    if (principal == null)
                    {
                        throw new UnauthorizedAccessException(__Res.GetString("Security_AccessNotAllowed"));
                    }
                    FluorineContext.Current.StorePrincipal(principal, username, password);
                    str3 = FluorineContext.Current.EncryptCredentials(this._endpoint, principal, username, password);
                    FluorineContext.Current.StorePrincipal(principal, str3);
                    ASObject obj3 = new ASObject();
                    obj3["name"] = "CredentialsId";
                    obj3["mustUnderstand"] = false;
                    obj3["data"] = str3;
                    AMFHeader header3 = new AMFHeader("RequestPersistentHeader", true, obj3);
                    context.MessageOutput.AddHeader(header3);
                }
                else
                {
                    header = context.AMFMessage.GetHeader("CredentialsId");
                    if (header != null)
                    {
                        str3 = header.Content as string;
                        if (str3 != null)
                        {
                            FluorineContext.Current.RestorePrincipal(messageBroker.LoginCommand, str3);
                        }
                    }
                    else
                    {
                        principal = FluorineContext.Current.RestorePrincipal(messageBroker.LoginCommand);
                    }
                }
            }
            catch (UnauthorizedAccessException exception)
            {
                for (num = 0; num < context.AMFMessage.BodyCount; num++)
                {
                    body2 = new ErrorResponseBody(context.AMFMessage.GetBodyAt(num), exception);
                    context.MessageOutput.AddBody(body2);
                }
            }
            catch (Exception exception2)
            {
                if ((log != null) && log.get_IsErrorEnabled())
                {
                    log.Error(exception2.Message, exception2);
                }
                for (num = 0; num < context.AMFMessage.BodyCount; num++)
                {
                    body2 = new ErrorResponseBody(context.AMFMessage.GetBodyAt(num), exception2);
                    context.MessageOutput.AddBody(body2);
                }
            }
            FluorineContext.Current.User = principal;
            Thread.CurrentPrincipal = principal;
        }
    }
}

