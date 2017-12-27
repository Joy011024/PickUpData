namespace FluorineFx.Messaging.Services
{
    using FluorineFx;
    using FluorineFx.Context;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Messages;
    using log4net;
    using System;
    using System.Collections;
    using System.Security;
    using System.Security.Principal;
    using System.Text;
    using System.Threading;

    internal class AuthenticationService : ServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AuthenticationService));
        public const string ServiceId = "authentication-service";

        public AuthenticationService(MessageBroker broker, ServiceSettings settings) : base(broker, settings)
        {
        }

        private IPrincipal Authenticate(IMessage message)
        {
            if (message.body is string)
            {
                return this.Authenticate(message.body as string);
            }
            return null;
        }

        internal IPrincipal Authenticate(string credentials)
        {
            IPrincipal principal = null;
            string s = credentials;
            byte[] bytes = Convert.FromBase64String(s);
            StringBuilder builder = new StringBuilder();
            builder.Append(Encoding.UTF8.GetChars(bytes));
            string[] strArray = builder.ToString().Split(new char[] { ':' });
            string username = strArray[0];
            string password = strArray[1];
            if (base._messageBroker.LoginCommand != null)
            {
                Hashtable hashtable = new Hashtable(1);
                hashtable["password"] = password;
                principal = base._messageBroker.LoginCommand.DoAuthentication(username, hashtable);
                if (principal != null)
                {
                    FluorineContext.Current.StorePrincipal(principal, username, password);
                }
                return principal;
            }
            if (log.get_IsErrorEnabled())
            {
                log.Error(__Res.GetString("Security_LoginMissing"));
            }
            return principal;
        }

        public override void CheckSecurity(IMessage message)
        {
        }

        public override object ServiceMessage(IMessage message)
        {
            CommandMessage message3 = message as CommandMessage;
            if (message3 != null)
            {
                switch (message3.operation)
                {
                    case 8:
                    {
                        IPrincipal principal = null;
                        try
                        {
                            principal = this.Authenticate(message);
                            FluorineContext.Current.User = principal;
                            Thread.CurrentPrincipal = principal;
                        }
                        catch (SecurityException)
                        {
                            throw;
                        }
                        catch (Exception exception)
                        {
                            string str = __Res.GetString("Security_AuthenticationFailed");
                            if (log.get_IsErrorEnabled())
                            {
                                log.Error(str, exception);
                            }
                            throw new SecurityException(str, exception);
                        }
                        if (principal == null)
                        {
                            throw new SecurityException(__Res.GetString("Security_AuthenticationFailed"));
                        }
                        return new AcknowledgeMessage { body = "success" };
                    }
                    case 9:
                    {
                        bool flag = base._messageBroker.LoginCommand.Logout(FluorineContext.Current.User);
                        return new AcknowledgeMessage { body = "success" };
                    }
                }
            }
            return null;
        }
    }
}

