namespace FluorineFx.Context
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using System.Web;

    internal sealed class WebSafeCallContext
    {
        private WebSafeCallContext()
        {
        }

        public static void FreeNamedDataSlot(string name)
        {
            HttpContext current = HttpContext.Current;
            if (current == null)
            {
                CallContext.FreeNamedDataSlot(name);
            }
            else
            {
                current.Items.Remove(name);
            }
        }

        public static object GetData(string name)
        {
            HttpContext current = HttpContext.Current;
            if (current == null)
            {
                return CallContext.GetData(name);
            }
            return current.Items[name];
        }

        public static void SetData(string name, object value)
        {
            HttpContext current = HttpContext.Current;
            if (current == null)
            {
                CallContext.SetData(name, value);
            }
            else
            {
                current.Items[name] = value;
            }
        }
    }
}

