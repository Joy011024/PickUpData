namespace FluorineFx.Messaging.Api
{
    using FluorineFx.Exceptions;
    using System;

    public class ScopeNotFoundException : FluorineException
    {
        public ScopeNotFoundException(IScope scope, string childName) : base(__Res.GetString("Scope_ChildNotFound", new object[] { childName, scope.Name }))
        {
        }
    }
}

