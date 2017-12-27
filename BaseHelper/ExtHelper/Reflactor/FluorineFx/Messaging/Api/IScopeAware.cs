namespace FluorineFx.Messaging.Api
{
    using System;

    public interface IScopeAware
    {
        void SetScope(IScope scope);
    }
}

