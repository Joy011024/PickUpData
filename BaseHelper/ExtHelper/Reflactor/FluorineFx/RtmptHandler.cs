namespace FluorineFx
{
    using FluorineFx.Context;
    using FluorineFx.Messaging;
    using log4net;
    using System;
    using System.Web;

    public class RtmptHandler : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RtmptHandler));

        private MessageServer GetMessageServer(HttpContext context)
        {
            return (context.Application["__@fluorinemessageserver"] as MessageServer);
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpApplication applicationInstance = context.ApplicationInstance;
            if (applicationInstance.Request.ContentType == "application/x-fcs")
            {
                applicationInstance.Response.Clear();
                applicationInstance.Response.ContentType = "application/x-fcs";
                GlobalContext.get_Properties().set_Item("ClientIP", HttpContext.Current.Request.UserHostAddress);
                if (log.get_IsDebugEnabled())
                {
                    log.Debug(__Res.GetString("Amf_Begin"));
                }
                try
                {
                    FluorineWebContext.Initialize();
                    MessageServer messageServer = this.GetMessageServer(context);
                    if (messageServer != null)
                    {
                        messageServer.ServiceRtmpt();
                    }
                    else
                    {
                        log.Fatal(__Res.GetString("MessageServer_AccessFail"));
                    }
                    if (log.get_IsDebugEnabled())
                    {
                        log.Debug(__Res.GetString("Amf_End"));
                    }
                    applicationInstance.CompleteRequest();
                }
                catch (Exception exception)
                {
                    log.Fatal(__Res.GetString("Amf_Fatal"), exception);
                    applicationInstance.Response.Clear();
                    applicationInstance.Response.ClearHeaders();
                    applicationInstance.Response.Status = __Res.GetString("Amf_Fatal404") + " " + exception.Message;
                    applicationInstance.CompleteRequest();
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}

