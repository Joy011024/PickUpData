namespace FluorineFx.Json.Rpc
{
    using FluorineFx.Json;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Util;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;
    using System.Web;

    internal sealed class JsonRpcExecutive : JsonRpcServiceFeature
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(JsonRpcExecutive));

        public JsonRpcExecutive(MessageBroker messageBroker) : base(messageBroker)
        {
        }

        private static IDictionary CreateResponse(object id, object result, object error)
        {
            JavaScriptObject obj2 = new JavaScriptObject();
            obj2["id"] = id;
            if (error != null)
            {
                obj2["error"] = error;
                return obj2;
            }
            obj2["result"] = result;
            return obj2;
        }

        private object FromException(ErrorMessage message)
        {
            return JsonRpcError.FromException(message, false);
        }

        private object FromException(Exception ex)
        {
            return JsonRpcError.FromException(ex, false);
        }

        private TextReader GetRequestReader()
        {
            if (StringUtils.CaselessEquals(base.Request.ContentType, "application/x-www-form-urlencoded"))
            {
                return new StringReader((base.Request.Form.Count == 1) ? base.Request.Form[0] : base.Request.Form["JSON-RPC"]);
            }
            return new StreamReader(base.Request.InputStream, base.Request.ContentEncoding);
        }

        private IDictionary Invoke(IDictionary request)
        {
            Exception exception;
            ValidationUtils.ArgumentNotNull(request, "request");
            object error = null;
            object result = null;
            object id = request["id"];
            string s = request["credentials"] as string;
            if (!StringUtils.IsNullOrEmpty(s))
            {
                try
                {
                    CommandMessage message = new CommandMessage(8) {
                        body = s
                    };
                    IMessage message2 = this.MessageBroker.RouteMessage(message);
                    if (message2 is ErrorMessage)
                    {
                        error = this.FromException(message2 as ErrorMessage);
                        return CreateResponse(id, result, error);
                    }
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    error = this.FromException(exception);
                    return CreateResponse(id, result, error);
                }
            }
            if (JavaScriptConvert.IsNull(id))
            {
                throw new NotSupportedException("Notification are not yet supported.");
            }
            log.Debug(string.Format("Received request with the ID {0}.", id.ToString()));
            string str2 = StringUtils.MaskNullString((string) request["method"]);
            if (str2.Length == 0)
            {
                throw new JsonRpcException("No method name supplied for this request.");
            }
            try
            {
                RemotingMessage message3 = new RemotingMessage {
                    destination = base.Request.QueryString["destination"],
                    source = base.Request.QueryString["source"],
                    operation = str2
                };
                object obj5 = request["params"];
                object[] objArray = (obj5 as JavaScriptArray).ToArray();
                message3.body = objArray;
                IMessage message4 = this.MessageBroker.RouteMessage(message3);
                if (message4 is ErrorMessage)
                {
                    error = this.FromException(message4 as ErrorMessage);
                }
                else
                {
                    result = message4.body;
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                log.Error(exception.Message, exception);
                throw;
            }
            return CreateResponse(id, result, error);
        }

        protected override void ProcessRequest()
        {
            if (!StringUtils.CaselessEquals(base.Request.RequestType, "POST"))
            {
                throw new JsonRpcException(string.Format("HTTP {0} is not supported for RPC execution. Use HTTP POST only.", base.Request.RequestType));
            }
            base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            base.Response.ContentType = "text/plain";
            using (TextReader reader = this.GetRequestReader())
            {
                this.ProcessRequest(reader, base.Response.Output);
            }
        }

        private void ProcessRequest(TextReader input, TextWriter output)
        {
            IDictionary dictionary;
            ValidationUtils.ArgumentNotNull(input, "input");
            ValidationUtils.ArgumentNotNull(output, "output");
            try
            {
                IDictionary request = JavaScriptConvert.DeserializeObject(input) as IDictionary;
                dictionary = this.Invoke(request);
            }
            catch (MissingMethodException exception)
            {
                dictionary = CreateResponse(null, null, this.FromException(exception));
            }
            catch (JsonReaderException exception2)
            {
                dictionary = CreateResponse(null, null, this.FromException(exception2));
            }
            catch (JsonWriterException exception3)
            {
                dictionary = CreateResponse(null, null, this.FromException(exception3));
            }
            string str = JavaScriptConvert.SerializeObject(dictionary);
            output.Write(str);
        }
    }
}

