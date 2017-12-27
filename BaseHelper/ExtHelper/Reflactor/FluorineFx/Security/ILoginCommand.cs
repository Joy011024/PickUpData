namespace FluorineFx.Security
{
    using System;
    using System.Collections;
    using System.Security.Principal;

    public interface ILoginCommand
    {
        IPrincipal DoAuthentication(string username, Hashtable credentials);
        bool DoAuthorization(IPrincipal principal, IList roles);
        bool Logout(IPrincipal principal);
        void Start();
        void Stop();
    }
}

