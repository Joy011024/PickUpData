namespace FluorineFx.Exceptions
{
    using System;

    public class ServiceNotFoundException : FluorineException
    {
        public ServiceNotFoundException(string serviceName) : base(__Res.GetString("Service_NotFound", new object[] { serviceName }))
        {
        }
    }
}

