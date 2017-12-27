namespace FluorineFx.Data.Messages
{
    using FluorineFx.Data;
    using FluorineFx.Messaging.Messages;
    using System;
    using System.Collections;

    internal class DataErrorMessage : ErrorMessage
    {
        public DataMessage cause;
        public IList propertyNames;
        public object serverObject;

        public DataErrorMessage()
        {
        }

        public DataErrorMessage(DataSyncException dataSyncException)
        {
            this.serverObject = dataSyncException.ServerObject;
            this.propertyNames = dataSyncException.PropertyNames;
        }
    }
}

