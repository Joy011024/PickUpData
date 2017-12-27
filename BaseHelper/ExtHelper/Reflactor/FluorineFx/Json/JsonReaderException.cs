namespace FluorineFx.Json
{
    using System;

    public class JsonReaderException : Exception
    {
        public JsonReaderException()
        {
        }

        public JsonReaderException(string message) : base(message)
        {
        }

        public JsonReaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

