namespace FluorineFx.Messaging.Endpoints
{
    using FluorineFx.Context;
    using FluorineFx.IO;
    using System;
    using System.IO;
    using System.Security;
    using System.Security.Permissions;
    using System.Web;

    internal class AMFContext
    {
        private FluorineFx.IO.AMFMessage _amfMessage;
        private Stream _inputStream;
        private FluorineFx.IO.MessageOutput _messageOutput;
        private Stream _outputStream;
        public const string FluorineAMFContextKey = "__@fluorineamfcontext";

        public AMFContext(Stream inputStream, Stream outputStream)
        {
            this._inputStream = inputStream;
            this._outputStream = outputStream;
        }

        public FluorineFx.IO.AMFMessage AMFMessage
        {
            get
            {
                return this._amfMessage;
            }
            set
            {
                this._amfMessage = value;
            }
        }

        public static AMFContext Current
        {
            get
            {
                AMFContext data = null;
                HttpContext current = HttpContext.Current;
                if (current != null)
                {
                    return (current.Items["__@fluorineamfcontext"] as AMFContext);
                }
                try
                {
                    new SecurityPermission(PermissionState.Unrestricted).Demand();
                    data = WebSafeCallContext.GetData("__@fluorineamfcontext") as AMFContext;
                }
                catch (SecurityException)
                {
                }
                return data;
            }
            set
            {
                HttpContext current = HttpContext.Current;
                if (current != null)
                {
                    current.Items["__@fluorineamfcontext"] = value;
                }
                try
                {
                    new SecurityPermission(PermissionState.Unrestricted).Demand();
                    WebSafeCallContext.SetData("__@fluorineamfcontext", value);
                }
                catch (SecurityException)
                {
                }
            }
        }

        public Stream InputStream
        {
            get
            {
                return this._inputStream;
            }
        }

        public FluorineFx.IO.MessageOutput MessageOutput
        {
            get
            {
                return this._messageOutput;
            }
            set
            {
                this._messageOutput = value;
            }
        }

        public Stream OutputStream
        {
            get
            {
                return this._outputStream;
            }
        }
    }
}

