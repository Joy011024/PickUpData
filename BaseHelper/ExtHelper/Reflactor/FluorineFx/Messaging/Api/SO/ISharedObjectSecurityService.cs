namespace FluorineFx.Messaging.Api.SO
{
    using FluorineFx.Messaging.Api;
    using System;
    using System.Collections;

    public interface ISharedObjectSecurityService : IService
    {
        IEnumerator GetSharedObjectSecurity();
        void RegisterSharedObjectSecurity(ISharedObjectSecurity handler);
        void UnregisterSharedObjectSecurity(ISharedObjectSecurity handler);
    }
}

