namespace FluorineFx.Invocation
{
    using System;
    using System.Reflection;

    public interface IInvocationResultHandler
    {
        void HandleResult(IInvocationManager invocationManager, MethodInfo methodInfo, object obj, object[] arguments, object result);
    }
}

