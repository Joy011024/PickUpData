namespace FluorineFx.Messaging.Messages
{
    using FluorineFx;
    using FluorineFx.Messaging;
    using System;
    using System.Security;

    [CLSCompliant(false)]
    public class ErrorMessage : AcknowledgeMessage
    {
        private ASObject _extendedData;
        private string _faultCode;
        private string _faultDetail;
        private string _faultString;
        private object _rootCause;
        public const string ClientAuthenticationError = "Client.Authentication";
        public const string ClientAuthorizationError = "Client.Authorization";

        internal static ErrorMessage GetErrorMessage(IMessage message, Exception exception)
        {
            MessageException exception2 = null;
            if (exception is MessageException)
            {
                exception2 = exception as MessageException;
            }
            else
            {
                exception2 = new MessageException(exception);
            }
            ErrorMessage errorMessage = exception2.GetErrorMessage();
            if (message.clientId != null)
            {
                errorMessage.clientId = message.clientId;
            }
            else
            {
                errorMessage.clientId = Guid.NewGuid().ToString("D");
            }
            errorMessage.correlationId = message.messageId;
            errorMessage.destination = message.destination;
            if (exception is SecurityException)
            {
                errorMessage.faultCode = "Client.Authentication";
            }
            if (exception is UnauthorizedAccessException)
            {
                errorMessage.faultCode = "Client.Authorization";
            }
            return errorMessage;
        }

        public ASObject extendedData
        {
            get
            {
                return this._extendedData;
            }
            set
            {
                this._extendedData = value;
            }
        }

        public string faultCode
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

        public string faultDetail
        {
            get
            {
                return this._faultDetail;
            }
            set
            {
                this._faultDetail = value;
            }
        }

        public string faultString
        {
            get
            {
                return this._faultString;
            }
            set
            {
                this._faultString = value;
            }
        }

        public object rootCause
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

