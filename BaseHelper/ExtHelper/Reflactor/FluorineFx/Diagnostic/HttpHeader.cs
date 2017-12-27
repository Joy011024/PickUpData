namespace FluorineFx.Diagnostic
{
    using System;
    using System.Collections;
    using System.Web;

    internal class HttpHeader : DebugEvent
    {
        public HttpHeader()
        {
            this["EventType"] = "HttpRequestHeaders";
            Hashtable hashtable = new Hashtable();
            if (HttpContext.Current != null)
            {
                foreach (string str in HttpContext.Current.Request.Headers.AllKeys)
                {
                    string str2 = HttpContext.Current.Request.Headers[str];
                    hashtable[str] = str2;
                }
            }
            this["HttpHeaders"] = hashtable;
        }
    }
}

