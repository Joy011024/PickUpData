namespace FluorineFx.Messaging.Api.Service
{
    using System;

    public interface IServiceCall
    {
        object[] Arguments { get; }

        System.Exception Exception { get; set; }

        bool IsSuccess { get; }

        string ServiceMethodName { get; }

        string ServiceName { get; }

        byte Status { get; set; }
    }
}

