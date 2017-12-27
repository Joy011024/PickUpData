namespace FluorineFx.Messaging.Endpoints.Filter
{
    using FluorineFx.Messaging.Endpoints;
    using System;

    internal abstract class AbstractFilter : IFilter
    {
        private IFilter _next;

        public virtual void Invoke(AMFContext context)
        {
        }

        public virtual IFilter Next
        {
            get
            {
                return this._next;
            }
            set
            {
                this._next = value;
            }
        }
    }
}

