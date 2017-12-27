namespace FluorineFx.Context
{
    using FluorineFx;
    using FluorineFx.IO;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Security;
    using System;
    using System.Collections;
    using System.Security.Principal;
    using System.Threading;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Security;

    internal sealed class FluorineWebContext : FluorineContext
    {
        internal FluorineWebContext()
        {
        }

        public override void ClearPrincipal()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(GetFormsAuthCookieName());
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (((ticket != null) && (ticket.UserData != null)) && ticket.UserData.StartsWith("fluorineauthticket"))
                {
                    HttpRuntime.Cache.Remove(ticket.UserData);
                }
            }
            FormsAuthentication.SignOut();
            if (AMFContext.Current != null)
            {
                AMFContext current = AMFContext.Current;
                if (current.AMFMessage.GetHeader("CredentialsId") != null)
                {
                    current.AMFMessage.RemoveHeader("CredentialsId");
                    ASObject content = new ASObject();
                    content["name"] = "CredentialsId";
                    content["mustUnderstand"] = false;
                    content["data"] = null;
                    AMFHeader header = new AMFHeader("RequestPersistentHeader", true, content);
                    current.MessageOutput.AddHeader(header);
                }
            }
        }

        internal override string EncryptCredentials(IEndpoint endpoint, IPrincipal principal, string userId, string password)
        {
            string id = endpoint.Id;
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(FormsAuthentication.GetAuthCookie("fluorine", false).Value);
            string userData = string.Join("|", new string[] { GenericLoginCommand.FluorineTicket, id, userId, password });
            FormsAuthenticationTicket ticket2 = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, userData, ticket.CookiePath);
            return FormsAuthentication.Encrypt(ticket2);
        }

        public static string GetFormsAuthCookieName()
        {
            return (Environment.UserInteractive ? ".ASPXAUTH" : FormsAuthentication.FormsCookieName);
        }

        public override IResource GetResource(string location)
        {
            return new FileSystemResource(location);
        }

        internal static void Initialize()
        {
            HttpContext.Current.Items["__@fluorinecontext"] = new FluorineWebContext();
        }

        public override IPrincipal RestorePrincipal(ILoginCommand loginCommand)
        {
            IPrincipal currentPrincipal = null;
            if ((HttpContext.Current != null) && HttpContext.Current.Request.IsAuthenticated)
            {
                if (!(HttpContext.Current.User.Identity is FormsIdentity))
                {
                    return HttpContext.Current.User;
                }
                FormsIdentity identity = HttpContext.Current.User.Identity as FormsIdentity;
                if (!((identity.Ticket.UserData != null) && identity.Ticket.UserData.StartsWith("fluorineauthticket")))
                {
                    return HttpContext.Current.User;
                }
            }
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(GetFormsAuthCookieName());
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null)
                {
                    currentPrincipal = HttpContext.Current.Cache[ticket.UserData] as IPrincipal;
                    if ((currentPrincipal == null) && ((ticket.UserData != null) && ticket.UserData.StartsWith("fluorineauthticket")))
                    {
                        string[] strArray = ticket.UserData.Split(new char[] { '|' });
                        string username = strArray[2];
                        string password = strArray[3];
                        if (loginCommand == null)
                        {
                            throw new UnauthorizedAccessException(__Res.GetString("Security_LoginMissing"));
                        }
                        Hashtable credentials = new Hashtable(1);
                        credentials["password"] = password;
                        currentPrincipal = loginCommand.DoAuthentication(username, credentials);
                        if (currentPrincipal == null)
                        {
                            throw new UnauthorizedAccessException(__Res.GetString("Security_AuthenticationFailed"));
                        }
                        this.StorePrincipal(currentPrincipal, username, password);
                    }
                }
                else
                {
                    currentPrincipal = Thread.CurrentPrincipal;
                }
            }
            if (currentPrincipal != null)
            {
                this.User = currentPrincipal;
                Thread.CurrentPrincipal = currentPrincipal;
            }
            return currentPrincipal;
        }

        internal override IPrincipal RestorePrincipal(ILoginCommand loginCommand, string key)
        {
            IPrincipal principal = null;
            if (key != null)
            {
                principal = HttpContext.Current.Cache[key] as IPrincipal;
                if (principal == null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(key);
                    if (ticket == null)
                    {
                        throw new UnauthorizedAccessException(__Res.GetString("Security_AuthenticationFailed"));
                    }
                    string[] strArray = ticket.UserData.Split(new char[] { '|' });
                    string username = strArray[2];
                    string str2 = strArray[3];
                    if (loginCommand == null)
                    {
                        throw new UnauthorizedAccessException(__Res.GetString("Security_LoginMissing"));
                    }
                    Hashtable credentials = new Hashtable(1);
                    credentials["password"] = str2;
                    principal = loginCommand.DoAuthentication(username, credentials);
                    if (principal == null)
                    {
                        throw new UnauthorizedAccessException(__Res.GetString("Security_AuthenticationFailed"));
                    }
                    this.StorePrincipal(principal, key);
                }
            }
            if (principal != null)
            {
                this.User = principal;
                Thread.CurrentPrincipal = principal;
            }
            return principal;
        }

        internal void SetConnection(IConnection connection)
        {
            this.Items["__@fluorineconnection"] = connection;
        }

        internal override void SetCurrentClient(IClient client)
        {
            this.Items["__@fluorineclient"] = client;
        }

        internal override void StorePrincipal(IPrincipal principal, string key)
        {
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(FormsAuthentication.GetAuthCookie("fluorine", false).Value);
            HttpRuntime.Cache.Insert(key, principal, null, Cache.NoAbsoluteExpiration, ticket.Expiration.Subtract(ticket.IssueDate), CacheItemPriority.Normal, null);
        }

        public override void StorePrincipal(IPrincipal principal, string userId, string password)
        {
            string str = Guid.NewGuid().ToString("N");
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(userId, false);
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string userData = string.Join("|", new string[] { GenericLoginCommand.FluorineTicket, str, userId, password });
            FormsAuthenticationTicket ticket2 = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, userData, ticket.CookiePath);
            authCookie.Value = FormsAuthentication.Encrypt(ticket2);
            authCookie.Secure = FormsAuthentication.RequireSSL;
            HttpContext.Current.Response.Cookies.Add(authCookie);
            HttpRuntime.Cache.Insert(userData, principal, null, Cache.NoAbsoluteExpiration, ticket2.Expiration.Subtract(ticket2.IssueDate), CacheItemPriority.Normal, null);
        }

        public override string AbsoluteUri
        {
            get
            {
                return HttpContext.Current.Request.Url.AbsoluteUri;
            }
        }

        public override string ActivationMode
        {
            get
            {
                try
                {
                    if (HttpContext.Current != null)
                    {
                        return HttpContext.Current.Request.QueryString["activate"];
                    }
                }
                catch (HttpException)
                {
                }
                return null;
            }
        }

        public override string ApplicationPath
        {
            get
            {
                string str = "";
                if (HttpContext.Current.Request.Url != null)
                {
                    str = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.ToLower().IndexOf(HttpContext.Current.Request.ApplicationPath.ToLower(), (int) (HttpContext.Current.Request.Url.AbsoluteUri.ToLower().IndexOf(HttpContext.Current.Request.Url.Authority.ToLower()) + HttpContext.Current.Request.Url.Authority.Length)) + HttpContext.Current.Request.ApplicationPath.Length);
                }
                return str;
            }
        }

        public override IApplicationState ApplicationState
        {
            get
            {
                return new HttpApplicationStateWrapper();
            }
        }

        public override IClient Client
        {
            get
            {
                return (this.Items["__@fluorineclient"] as IClient);
            }
        }

        public override IConnection Connection
        {
            get
            {
                return (this.Items["__@fluorineconnection"] as IConnection);
            }
        }

        public override IDictionary Items
        {
            get
            {
                return HttpContext.Current.Items;
            }
        }

        public override string PhysicalApplicationPath
        {
            get
            {
                return HttpContext.Current.Request.PhysicalApplicationPath;
            }
        }

        public override string RequestApplicationPath
        {
            get
            {
                return HttpContext.Current.Request.ApplicationPath;
            }
        }

        public override string RequestPath
        {
            get
            {
                return HttpContext.Current.Request.Path;
            }
        }

        public override string RootPath
        {
            get
            {
                return HttpRuntime.AppDomainAppPath;
            }
        }

        public override ISessionState Session
        {
            get
            {
                return HttpSessionStateWrapper.CreateSessionWrapper(HttpContext.Current.Session);
            }
        }

        public override IPrincipal User
        {
            get
            {
                return HttpContext.Current.User;
            }
            set
            {
                HttpContext.Current.User = value;
            }
        }
    }
}

