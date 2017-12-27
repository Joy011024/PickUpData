namespace FluorineFx.Exceptions
{
    using System;

    public class ClientRejectedException : FluorineException
    {
        private object _reason;

        public ClientRejectedException(object reason)
        {
            this._reason = reason;
        }

        public object Reason
        {
            get
            {
                return this._reason;
            }
        }
    }
}

