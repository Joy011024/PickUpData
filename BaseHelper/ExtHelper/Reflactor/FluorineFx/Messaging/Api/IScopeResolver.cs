namespace FluorineFx.Messaging.Api
{
    using System;

    public interface IScopeResolver
    {
        IScope ResolveScope(string path);
        IScope ResolveScope(IScope root, string path);

        IGlobalScope GlobalScope { get; }
    }
}

