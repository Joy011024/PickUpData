namespace FluorineFx.Security
{
    using FluorineFx.Context;
    using System;
    using System.Collections;
    using System.Security.Principal;

    public class GenericLoginCommand : ILoginCommand
    {
        public static string FluorineTicket = "fluorineauthticket";

        public virtual IPrincipal DoAuthentication(string username, Hashtable credentials)
        {
            return new GenericPrincipal(new GenericIdentity(username), new string[0]);
        }

        public virtual bool DoAuthorization(IPrincipal principal, IList roles)
        {
            foreach (string str in roles)
            {
                if (principal.IsInRole(str))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool Logout(IPrincipal principal)
        {
            if (FluorineContext.Current != null)
            {
                FluorineContext.Current.ClearPrincipal();
            }
            return true;
        }

        public virtual void Start()
        {
        }

        public virtual void Stop()
        {
        }
    }
}

