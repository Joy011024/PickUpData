namespace FluorineFx.Json.Services
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DuplicateMethodException : ApplicationException
    {
        private const string _defaultMessage = "A method with the same name has been defined elsewhere on the service.";

        public DuplicateMethodException() : this(null)
        {
        }

        public DuplicateMethodException(string message) : base(StringUtils.MaskNullString(message, "A method with the same name has been defined elsewhere on the service."))
        {
        }

        protected DuplicateMethodException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DuplicateMethodException(string message, Exception innerException) : base(StringUtils.MaskNullString(message, "A method with the same name has been defined elsewhere on the service."), innerException)
        {
        }
    }
}

