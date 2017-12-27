namespace FluorineFx
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true), Obsolete("Activation mode is obsolete. Please use the factory mechanism instead.", true)]
    public sealed class ActivationAttribute : Attribute
    {
        private string _activationMode;

        public ActivationAttribute(string activationMode)
        {
            this._activationMode = activationMode;
        }

        public string ActivationMode
        {
            get
            {
                return this._activationMode;
            }
        }
    }
}

