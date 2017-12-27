namespace FluorineFx.Messaging.Api
{
    using FluorineFx.Exceptions;
    using System;

    public class ScopeShuttingDownException : FluorineException
    {
        public ScopeShuttingDownException(IScope scope) : base("Scope shutting down: " + scope)
        {
        }
    }
}

