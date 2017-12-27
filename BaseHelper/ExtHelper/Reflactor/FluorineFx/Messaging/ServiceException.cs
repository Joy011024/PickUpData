namespace FluorineFx.Messaging
{
    using System;

    public class ServiceException : MessageException
    {
        public ServiceException()
        {
        }

        public ServiceException(Exception inner) : base(inner.Message, inner)
        {
        }

        public ServiceException(string message) : base(message)
        {
        }
    }
}

