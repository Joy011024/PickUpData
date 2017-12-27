namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx.Messaging.Api;
    using System;

    internal class ScopeResolver : IScopeResolver
    {
        protected IGlobalScope _globalScope;

        public ScopeResolver(IGlobalScope globalScope)
        {
            this._globalScope = globalScope;
        }

        public IScope ResolveScope(string path)
        {
            return this.ResolveScope(this._globalScope, path);
        }

        public IScope ResolveScope(IScope root, string path)
        {
            IScope scope = root;
            if (path != null)
            {
                string[] strArray = path.Split(new char[] { '/' });
                foreach (string str in strArray)
                {
                    string name = str;
                    if (name != string.Empty)
                    {
                        if (scope.HasChildScope(name))
                        {
                            scope = scope.GetScope(name);
                        }
                        else
                        {
                            if (scope.Equals(root))
                            {
                                throw new ScopeNotFoundException(scope, name);
                            }
                            lock (scope.SyncRoot)
                            {
                                if (!scope.HasChildScope(name))
                                {
                                    if (!scope.CreateChildScope(name))
                                    {
                                        throw new ScopeNotFoundException(scope, name);
                                    }
                                    scope = scope.GetScope(name);
                                }
                                else
                                {
                                    scope = scope.GetScope(name);
                                }
                            }
                        }
                        if ((scope is WebScope) && ((WebScope) scope).IsShuttingDown)
                        {
                            throw new ScopeShuttingDownException(scope);
                        }
                    }
                }
            }
            return scope;
        }

        public IGlobalScope GlobalScope
        {
            get
            {
                return this._globalScope;
            }
        }
    }
}

