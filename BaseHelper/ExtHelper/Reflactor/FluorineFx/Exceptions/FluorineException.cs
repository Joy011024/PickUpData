namespace FluorineFx.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class FluorineException : ApplicationException
    {
        public FluorineException()
        {
        }

        public FluorineException(string message) : base(message)
        {
        }

        public FluorineException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public FluorineException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

