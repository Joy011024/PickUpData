namespace FluorineFx.Json.Rpc
{
    using FluorineFx.Json;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Util;
    using System;
    using System.Diagnostics;

    internal class JsonRpcError
    {
        private JsonRpcError()
        {
            throw new NotSupportedException();
        }

        public static JavaScriptObject FromException(Exception e)
        {
            return FromException(e, false);
        }

        public static JavaScriptObject FromException(ErrorMessage message, bool includeStackTrace)
        {
            ValidationUtils.ArgumentNotNull(message, "message");
            JavaScriptObject obj2 = new JavaScriptObject();
            obj2.Add("name", "JSONRPCError");
            obj2.Add("message", message.faultString);
            obj2.Add("code", message.faultCode);
            if (includeStackTrace)
            {
                obj2.Add("stackTrace", message.faultDetail);
            }
            obj2.Add("errors", null);
            return obj2;
        }

        public static JavaScriptObject FromException(Exception e, bool includeStackTrace)
        {
            ValidationUtils.ArgumentNotNull(e, "e");
            JavaScriptObject obj2 = new JavaScriptObject();
            obj2.Add("name", "JSONRPCError");
            obj2.Add("message", e.GetBaseException().Message);
            if (includeStackTrace)
            {
                obj2.Add("stackTrace", e.StackTrace);
            }
            JavaScriptArray array = new JavaScriptArray();
            do
            {
                array.Add(ToLocalError(e));
                e = e.InnerException;
            }
            while (e != null);
            obj2.Add("errors", array);
            return obj2;
        }

        private static JavaScriptObject ToLocalError(Exception e)
        {
            Debug.Assert(e != null);
            JavaScriptObject obj2 = new JavaScriptObject();
            obj2.Add("name", e.GetType().Name);
            obj2.Add("message", e.Message);
            return obj2;
        }
    }
}

