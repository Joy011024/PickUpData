namespace FluorineFx
{
    using System;

    internal sealed class ExceptionASO : ASObject
    {
        public ExceptionASO(Exception exception)
        {
            base.Add("code", "SERVER.PROCESSING");
            base.Add("level", "error");
            base.Add("description", exception.Message);
            base.Add("details", exception.StackTrace);
            base.Add("type", exception.GetType().FullName);
            if (exception.InnerException != null)
            {
                base.Add("rootcause", new ExceptionASO(exception.InnerException));
            }
        }
    }
}

