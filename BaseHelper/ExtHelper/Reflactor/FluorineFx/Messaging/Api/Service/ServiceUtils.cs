namespace FluorineFx.Messaging.Api.Service
{
    using FluorineFx.Context;
    using FluorineFx.Messaging.Api;
    using System;

    public class ServiceUtils
    {
        public static bool InvokeOnConnection(string method, object[] args)
        {
            return InvokeOnConnection(method, args, null);
        }

        public static bool InvokeOnConnection(IConnection connection, string method, object[] args)
        {
            return InvokeOnConnection(connection, method, args, null);
        }

        public static bool InvokeOnConnection(string method, object[] args, IPendingServiceCallback callback)
        {
            return InvokeOnConnection(FluorineContext.Current.Connection, method, args, callback);
        }

        public static bool InvokeOnConnection(IConnection connection, string method, object[] args, IPendingServiceCallback callback)
        {
            if (connection is IServiceCapableConnection)
            {
                if (callback == null)
                {
                    (connection as IServiceCapableConnection).Invoke(method, args);
                }
                else
                {
                    (connection as IServiceCapableConnection).Invoke(method, args, callback);
                }
                return true;
            }
            return false;
        }
    }
}

