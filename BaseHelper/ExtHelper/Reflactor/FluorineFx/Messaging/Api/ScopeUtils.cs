namespace FluorineFx.Messaging.Api
{
    using System;

    public sealed class ScopeUtils
    {
        public static int Application = 1;
        public static int Global = 0;
        public static int Room = 2;

        public static IScope FindApplication(IScope scope)
        {
            IScope parent = scope;
            while (parent.HasParent && (parent.Depth != Application))
            {
                parent = parent.Parent;
            }
            return parent;
        }

        public static IScope FindRoot(IScope scope)
        {
            IScope parent = scope;
            while (parent.HasParent)
            {
                parent = parent.Parent;
            }
            return parent;
        }

        public static object GetScopeService(IScope scope, Type type)
        {
            return GetScopeService(scope, type, true);
        }

        public static object GetScopeService(IScope scope, Type type, bool checkHandler)
        {
            if ((scope == null) || (type == null))
            {
                return null;
            }
            object service = scope.GetService(type);
            if ((service == null) && checkHandler)
            {
                for (IScope scope2 = scope; scope2 != null; scope2 = scope2.Parent)
                {
                    IScopeHandler o = scope2.Handler;
                    if (type.IsInstanceOfType(o))
                    {
                        return o;
                    }
                    if (!scope2.HasParent)
                    {
                        return service;
                    }
                }
            }
            return service;
        }

        public static bool IsApplication(IBasicScope scope)
        {
            return (scope.Depth == Application);
        }

        public static bool IsRoom(IBasicScope scope)
        {
            return (scope.Depth >= Room);
        }
    }
}

