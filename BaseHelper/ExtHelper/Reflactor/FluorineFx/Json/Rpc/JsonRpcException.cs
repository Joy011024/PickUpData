namespace FluorineFx.Json.Rpc
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class JsonRpcException : ApplicationException
    {
        private const string _defaultMessage = "The JSON-RPC request could not be completed due to an error.";

        public JsonRpcException() : this((string) null)
        {
        }

        public JsonRpcException(Exception innerException) : base("The JSON-RPC request could not be completed due to an error.", innerException)
        {
        }

        public JsonRpcException(string message) : base(StringUtils.MaskNullString(message, "The JSON-RPC request could not be completed due to an error."))
        {
        }

        protected JsonRpcException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public JsonRpcException(string message, Exception innerException) : base(StringUtils.MaskNullString(message, "The JSON-RPC request could not be completed due to an error."), innerException)
        {
        }
    }
}

