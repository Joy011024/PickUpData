namespace FluorineFx.IO
{
    using FluorineFx.Messaging.Messages;
    using System;
    using System.Collections;

    internal class ErrorResponseBody : ResponseBody
    {
        private ErrorResponseBody()
        {
        }

        public ErrorResponseBody(AMFBody requestBody, Exception exception) : base(requestBody)
        {
            base.Content = exception;
            if (requestBody.IsEmptyTarget)
            {
                object content = requestBody.Content;
                if (content is IList)
                {
                    content = (content as IList)[0];
                }
                IMessage message = content as IMessage;
                if (message != null)
                {
                    ErrorMessage errorMessage = ErrorMessage.GetErrorMessage(message, exception);
                    base.Content = errorMessage;
                }
            }
            base.IgnoreResults = requestBody.IgnoreResults;
            base.Target = requestBody.Response + "/onStatus";
            base.Response = null;
        }

        public ErrorResponseBody(AMFBody requestBody, string error) : base(requestBody)
        {
            base.IgnoreResults = requestBody.IgnoreResults;
            base.Target = requestBody.Response + "/onStatus";
            base.Response = null;
            base.Content = error;
        }

        public ErrorResponseBody(AMFBody requestBody, IMessage message, ErrorMessage errorMessage) : base(requestBody)
        {
            base.Content = errorMessage;
            base.Target = requestBody.Response + "/onStatus";
            base.IgnoreResults = requestBody.IgnoreResults;
            base.Response = "";
        }

        public ErrorResponseBody(AMFBody requestBody, IMessage message, Exception exception) : base(requestBody)
        {
            ErrorMessage errorMessage = ErrorMessage.GetErrorMessage(message, exception);
            base.Content = errorMessage;
            base.Target = requestBody.Response + "/onStatus";
            base.IgnoreResults = requestBody.IgnoreResults;
            base.Response = "";
        }
    }
}

