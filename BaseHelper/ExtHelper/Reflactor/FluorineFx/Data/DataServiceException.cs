namespace FluorineFx.Data
{
    using FluorineFx.Messaging;
    using System;

    public class DataServiceException : MessageException
    {
        public DataServiceException()
        {
        }

        public DataServiceException(string message) : base(message)
        {
        }
    }
}

