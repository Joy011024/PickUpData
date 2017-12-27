namespace FluorineFx.Context
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Security;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;
    using System.Security;
    using System.Security.Permissions;
    using System.Security.Principal;
    using System.Web;

    public abstract class FluorineContext
    {
        public const string FlexClientIdHeader = "DSId";
        public const string FluorineClientKey = "__@fluorineclient";
        public const string FluorineConnectionKey = "__@fluorineconnection";
        public const string FluorineContextKey = "__@fluorinecontext";
        public const string FluorineDataServiceTransaction = "__@fluorinedataservicetransaction";
        public const string FluorinePrincipalAttribute = "__@fluorineprincipal";
        public const string FluorineSessionAttribute = "__@fluorinesession";
        public const string FluorineStreamIdKey = "__@fluorinestreamid";
        public const string FluorineTicket = "fluorineauthticket";
        private static readonly ILog log = LogManager.GetLogger(typeof(FluorineContext));

        internal FluorineContext()
        {
        }

        public abstract void ClearPrincipal();
        internal abstract string EncryptCredentials(IEndpoint endpoint, IPrincipal principal, string userId, string password);
        public virtual string GetFullPath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            string localPath = "";
            try
            {
                string applicationBaseDirectory = this.ApplicationBaseDirectory;
                if (applicationBaseDirectory != null)
                {
                    Uri uri = new Uri(applicationBaseDirectory);
                    if (uri.IsFile)
                    {
                        localPath = uri.LocalPath;
                    }
                }
            }
            catch
            {
            }
            if ((localPath != null) && (localPath.Length > 0))
            {
                return Path.GetFullPath(Path.Combine(localPath, path));
            }
            return Path.GetFullPath(path);
        }

        public virtual IResource GetResource(string location)
        {
            return new FileSystemResource(location);
        }

        public abstract IPrincipal RestorePrincipal(ILoginCommand loginCommand);
        internal abstract IPrincipal RestorePrincipal(ILoginCommand loginCommand, string key);
        internal virtual void SetCurrentClient(IClient client)
        {
        }

        internal abstract void StorePrincipal(IPrincipal principal, string key);
        public abstract void StorePrincipal(IPrincipal principal, string userId, string password);

        public abstract string AbsoluteUri { get; }

        public abstract string ActivationMode { get; }

        public virtual string ApplicationBaseDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public abstract string ApplicationPath { get; }

        public abstract IApplicationState ApplicationState { get; }

        public abstract IClient Client { get; }

        public string ClientId
        {
            get
            {
                if (this.Client != null)
                {
                    return this.Client.Id;
                }
                return null;
            }
        }

        public virtual IConnection Connection
        {
            get
            {
                return null;
            }
        }

        public static FluorineContext Current
        {
            get
            {
                FluorineContext data = null;
                HttpContext current = HttpContext.Current;
                if (current != null)
                {
                    return (current.Items["__@fluorinecontext"] as FluorineContext);
                }
                try
                {
                    new SecurityPermission(PermissionState.Unrestricted).Demand();
                    data = WebSafeCallContext.GetData("__@fluorinecontext") as FluorineContext;
                }
                catch (SecurityException)
                {
                }
                return data;
            }
        }

        public abstract IDictionary Items { get; }

        public abstract string PhysicalApplicationPath { get; }

        public abstract string RequestApplicationPath { get; }

        public abstract string RequestPath { get; }

        public abstract string RootPath { get; }

        public abstract ISessionState Session { get; }

        public abstract IPrincipal User { get; set; }
    }
}

