namespace FluorineFx.Messaging.Api.SO
{
    using FluorineFx.Messaging.Api;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ISharedObjectListener
    {
        void OnSharedObjectClear(ISharedObject so);
        void OnSharedObjectConnect(ISharedObject so);
        void OnSharedObjectDelete(ISharedObject so, string key);
        void OnSharedObjectDisconnect(ISharedObject so);
        void OnSharedObjectSend(ISharedObject so, string method, IList parameters);
        void OnSharedObjectUpdate(ISharedObject so, IAttributeStore values);
        void OnSharedObjectUpdate(ISharedObject so, IDictionary<string, object> values);
        void OnSharedObjectUpdate(ISharedObject so, string key, object value);
    }
}

