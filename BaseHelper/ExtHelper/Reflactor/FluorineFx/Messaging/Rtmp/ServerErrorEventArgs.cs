namespace FluorineFx.Messaging.Rtmp
{
    using System;

    internal class ServerErrorEventArgs : EventArgs
    {
        private System.Exception _exception;

        public ServerErrorEventArgs(System.Exception exception)
        {
            this._exception = exception;
        }

        public System.Exception Exception
        {
            get
            {
                return this._exception;
            }
        }
    }
}

