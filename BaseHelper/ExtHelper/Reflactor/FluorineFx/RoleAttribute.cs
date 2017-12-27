namespace FluorineFx
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class RoleAttribute : Attribute
    {
        private string _roles;

        [Obsolete("It is recommended to define security constraints in the security section of the services configuration file.")]
        public RoleAttribute(string roles)
        {
            this._roles = roles;
        }

        public string Roles
        {
            get
            {
                return this._roles;
            }
        }
    }
}

