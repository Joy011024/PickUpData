namespace FluorineFx.Messaging.Config
{
    using System;

    public sealed class SecurityConstraintRef
    {
        private string _reference;

        internal SecurityConstraintRef(string reference)
        {
            this._reference = reference;
        }

        public string Reference
        {
            get
            {
                return this._reference;
            }
        }
    }
}

