namespace FluorineFx.Messaging
{
    using FluorineFx;
    using FluorineFx.Exceptions;
    using FluorineFx.Messaging.Messages;
    using System;

    public class MessageException : FluorineException
    {
        private ASObject _extendedData;
        private string _faultCode;
        private object _rootCause;

        public MessageException()
        {
            this._faultCode = "Server.Processing";
            this._extendedData = new ASObject();
        }

        public MessageException(ASObject extendedData)
        {
            this._faultCode = "Server.Processing";
            this._extendedData = extendedData;
        }

        public MessageException(Exception inner) : base(inner.Message, inner)
        {
            this._faultCode = "Server.Processing";
            this._extendedData = new ASObject();
            this._rootCause = inner;
        }

        public MessageException(string message) : base(message)
        {
            this._faultCode = "Server.Processing";
            this._extendedData = new ASObject();
        }

        public MessageException(ASObject extendedData, string message) : base(message)
        {
            this._faultCode = "Server.Processing";
            this._extendedData = extendedData;
        }

        public MessageException(Exception inner, ASObject extendedData) : base(inner.Message, inner)
        {
            this._faultCode = "Server.Processing";
            this._extendedData = extendedData;
            this._rootCause = inner;
        }

        public MessageException(string message, Exception inner) : base(message, inner)
        {
            this._faultCode = "Server.Processing";
            this._extendedData = new ASObject();
        }

        public MessageException(ASObject extendedData, string message, Exception inner) : base(message, inner)
        {
            this._faultCode = "Server.Processing";
            this._extendedData = extendedData;
            this._rootCause = inner;
        }

        internal virtual ErrorMessage GetErrorMessage()
        {
            ErrorMessage message = new ErrorMessage {
                faultCode = this.FaultCode,
                faultString = this.Message
            };
            if (base.InnerException != null)
            {
                message.faultDetail = base.InnerException.StackTrace;
                if (this.ExtendedData != null)
                {
                    this.ExtendedData["FluorineStackTrace"] = this.StackTrace;
                }
            }
            else
            {
                message.faultDetail = this.StackTrace;
            }
            if (this.RootCause != null)
            {
                message.rootCause = this.RootCause;
            }
            message.extendedData = this.ExtendedData;
            return message;
        }

        public ASObject ExtendedData
        {
            get
            {
                return this._extendedData;
            }
        }

        public string FaultCode
        {
            get
            {
                return this._faultCode;
            }
            set
            {
                this._faultCode = value;
            }
        }

        public object RootCause
        {
            get
            {
                return this._rootCause;
            }
            set
            {
                this._rootCause = value;
            }
        }
    }
}

