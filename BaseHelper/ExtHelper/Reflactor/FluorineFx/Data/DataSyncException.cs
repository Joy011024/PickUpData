namespace FluorineFx.Data
{
    using FluorineFx.Data.Messages;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Messages;
    using System;
    using System.Collections;

    public class DataSyncException : MessageException
    {
        private IList _propertyNames;
        private object _serverObject;

        public DataSyncException(object serverVersion, IList propertyNames)
        {
            this._serverObject = serverVersion;
            this._propertyNames = propertyNames;
        }

        internal override ErrorMessage GetErrorMessage()
        {
            DataErrorMessage message = new DataErrorMessage(this) {
                faultCode = base.FaultCode,
                faultString = this.Message
            };
            if (base.InnerException != null)
            {
                message.faultDetail = base.InnerException.StackTrace;
                if (base.ExtendedData != null)
                {
                    base.ExtendedData["FluorineStackTrace"] = this.StackTrace;
                }
            }
            else
            {
                message.faultDetail = this.StackTrace;
            }
            message.extendedData = base.ExtendedData;
            if (base.RootCause != null)
            {
                message.rootCause = base.RootCause;
            }
            message.propertyNames = this._propertyNames;
            message.serverObject = this._serverObject;
            return message;
        }

        public IList PropertyNames
        {
            get
            {
                return this._propertyNames;
            }
        }

        public object ServerObject
        {
            get
            {
                return this._serverObject;
            }
        }
    }
}

