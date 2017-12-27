namespace FluorineFx.Messaging.Endpoints.Filter
{
    using FluorineFx.Messaging.Endpoints;
    using System;

    internal class FilterChain
    {
        private IFilter _filter;

        public FilterChain(IFilter filter)
        {
            this._filter = filter;
        }

        public void InvokeFilters(AMFContext context)
        {
            for (IFilter filter = this._filter; filter != null; filter = filter.Next)
            {
                filter.Invoke(context);
            }
        }
    }
}

