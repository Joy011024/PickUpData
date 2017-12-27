namespace FluorineFx.Json.Rpc
{
    using FluorineFx.Messaging;
    using System;
    using System.Security.Principal;
    using System.Web;
    using System.Web.SessionState;

    internal abstract class JsonRpcServiceFeature : IHttpHandler
    {
        private HttpContext _context;
        private readonly FluorineFx.Messaging.MessageBroker _messageBroker;

        protected JsonRpcServiceFeature(FluorineFx.Messaging.MessageBroker messageBroker)
        {
            this._messageBroker = messageBroker;
        }

        protected abstract void ProcessRequest();
        public virtual void ProcessRequest(HttpContext context)
        {
            this._context = context;
            this.ProcessRequest();
        }

        public HttpApplicationState Application
        {
            get
            {
                return this.Context.Application;
            }
        }

        public HttpApplication ApplicationInstance
        {
            get
            {
                return this.Context.ApplicationInstance;
            }
        }

        public HttpContext Context
        {
            get
            {
                return this._context;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public virtual FluorineFx.Messaging.MessageBroker MessageBroker
        {
            get
            {
                return this._messageBroker;
            }
        }

        public HttpRequest Request
        {
            get
            {
                return this.Context.Request;
            }
        }

        public HttpResponse Response
        {
            get
            {
                return this.Context.Response;
            }
        }

        public HttpServerUtility Server
        {
            get
            {
                return this.Context.Server;
            }
        }

        public HttpSessionState Session
        {
            get
            {
                return this.Context.Session;
            }
        }

        public IPrincipal User
        {
            get
            {
                return this.Context.User;
            }
        }
    }
}

