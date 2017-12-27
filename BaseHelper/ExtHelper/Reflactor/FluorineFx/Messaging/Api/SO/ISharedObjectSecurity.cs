namespace FluorineFx.Messaging.Api.SO
{
    using FluorineFx.Messaging.Api;
    using System;
    using System.Collections;

    public interface ISharedObjectSecurity
    {
        bool IsConnectionAllowed(ISharedObject so);
        bool IsCreationAllowed(IScope scope, string name, bool persistent);
        bool IsDeleteAllowed(ISharedObject so, string key);
        bool IsSendAllowed(ISharedObject so, string message, IList arguments);
        bool IsWriteAllowed(ISharedObject so, string key, object value);
    }
}

