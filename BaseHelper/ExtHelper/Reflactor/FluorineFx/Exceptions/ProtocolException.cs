namespace FluorineFx.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class ProtocolException : FluorineException
    {
        public ProtocolException()
        {
        }

        public ProtocolException(string message) : base(message)
        {
        }

        public ProtocolException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ProtocolException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

