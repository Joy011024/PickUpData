namespace FluorineFx.Browser
{
    using System;
    using System.Web;

    public interface IServiceBrowserRenderer
    {
        bool CanRender(HttpRequest httpRequest);
        void Render(HttpApplication httpApplication);
    }
}

