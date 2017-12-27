namespace FluorineFx.Net
{
    using FluorineFx;
    using System;

    public class NetStatusEventArgs : EventArgs
    {
        private System.Exception _exception;
        private ASObject _info;

        internal NetStatusEventArgs(ASObject info)
        {
            this._info = info;
        }

        internal NetStatusEventArgs(System.Exception exception)
        {
            this._exception = exception;
            this._info = new ASObject();
            this._info["level"] = "error";
            this._info["code"] = "NetConnection.Call.BadVersion";
            this._info["description"] = exception.Message;
        }

        internal NetStatusEventArgs(string message)
        {
            this._info = new ASObject();
            this._info["level"] = "error";
            this._info["code"] = "NetConnection.Call.BadVersion";
            this._info["description"] = message;
        }

        public System.Exception Exception
        {
            get
            {
                return this._exception;
            }
        }

        public ASObject Info
        {
            get
            {
                return this._info;
            }
        }
    }
}

