namespace FluorineFx.Json
{
    using FluorineFx.Json.Services;
    using FluorineFx.Util;
    using System;
    using System.Web;

    public interface IJsonRpcProxyGenerator
    {
        void WriteProxy(ServiceClass service, IndentedTextWriter writer, HttpRequest request);
    }
}

