namespace FluorineFx.Messaging.Config
{
    using System;

    public sealed class SecurityConstraint
    {
        private string _authMethod;
        private string _id;
        private string[] _roles;

        internal SecurityConstraint(string id, string authMethod, string[] roles)
        {
            this._id = id;
            this._authMethod = authMethod;
            this._roles = roles;
        }

        public string AuthMethod
        {
            get
            {
                return this._authMethod;
            }
        }

        public string Id
        {
            get
            {
                return this._id;
            }
        }

        public string[] Roles
        {
            get
            {
                return this._roles;
            }
        }
    }
}

