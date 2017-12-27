namespace FluorineFx.Json.Rpc
{
    using FluorineFx.Messaging;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.Web;

    internal class JsonRpcHandler
    {
        private HttpContext _context;
        private static Hashtable Features = new Hashtable();
        private static readonly ILog log = LogManager.GetLogger(typeof(JsonRpcHandler));

        public JsonRpcHandler(HttpContext context)
        {
            this._context = context;
        }

        private object GetFeature(string feature)
        {
            if (Features.Contains(feature))
            {
                return Features[feature];
            }
            lock (Features.SyncRoot)
            {
                if (!Features.Contains(feature))
                {
                    IHttpHandler handler;
                    MessageBroker messageBroker = MessageBroker.GetMessageBroker(MessageBroker.DefaultMessageBrokerId);
                    if (feature == "proxy")
                    {
                        handler = new JsonRpcProxyGenerator(messageBroker);
                        Features[feature] = handler;
                        return handler;
                    }
                    if (feature == "rpc")
                    {
                        handler = new JsonRpcExecutive(messageBroker);
                        Features[feature] = handler;
                        return handler;
                    }
                }
                else
                {
                    return Features[feature];
                }
            }
            throw new NotImplementedException(string.Format("The requested feature {0} is not implemented ", feature));
        }

        public void ProcessRequest()
        {
            HttpRequest request = this._context.Request;
            string requestType = request.RequestType;
            string feature = null;
            if (StringUtils.CaselessEquals(requestType, "GET") || StringUtils.CaselessEquals(requestType, "HEAD"))
            {
                feature = request.QueryString[null];
            }
            else if (StringUtils.CaselessEquals(requestType, "POST"))
            {
                feature = "rpc";
            }
            (this.GetFeature(feature) as IHttpHandler).ProcessRequest(this._context);
        }
    }
}

