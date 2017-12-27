namespace FluorineFx.Messaging.Endpoints.Filter
{
    using FluorineFx;
    using FluorineFx.Diagnostic;
    using FluorineFx.IO;
    using FluorineFx.Messaging.Endpoints;
    using System;
    using System.Collections;

    internal class DebugFilter : AbstractFilter
    {
        public override void Invoke(AMFContext context)
        {
            AMFMessage aMFMessage = context.AMFMessage;
            AMFHeader header = aMFMessage.GetHeader("amf_server_debug");
            if (header != null)
            {
                ASObject content = header.Content as ASObject;
                AMFBody bodyAt = aMFMessage.GetBodyAt(aMFMessage.BodyCount - 1);
                ResponseBody body = new ResponseBody {
                    Target = bodyAt.Response + "/onDebugEvents",
                    Response = null,
                    IsDebug = true
                };
                ArrayList list = new ArrayList();
                ArrayList list2 = new ArrayList();
                list.Add(list2);
                if ((bool) content["httpheaders"])
                {
                    list2.Add(new HttpHeader());
                }
                if ((bool) content["amfheaders"])
                {
                    list2.Add(new AMFRequestHeaders(aMFMessage));
                }
                ArrayList traceStack = NetDebug.GetTraceStack();
                if ((((bool) content["trace"]) && (traceStack != null)) && (traceStack.Count > 0))
                {
                    ArrayList list4 = new ArrayList(traceStack);
                    list2.Add(new TraceHeader(list4));
                    NetDebug.Clear();
                }
                body.Content = list;
                context.MessageOutput.AddBody(body);
            }
            NetDebug.Clear();
        }
    }
}

