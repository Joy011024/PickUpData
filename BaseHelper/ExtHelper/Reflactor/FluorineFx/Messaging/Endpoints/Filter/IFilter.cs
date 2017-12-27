namespace FluorineFx.Messaging.Endpoints.Filter
{
    using FluorineFx.Messaging.Endpoints;
    using System;

    internal interface IFilter
    {
        void Invoke(AMFContext context);

        IFilter Next { get; set; }
    }
}

