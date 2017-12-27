namespace FluorineFx.Invocation
{
    using System;
    using System.Reflection;

    public interface IInvocationCallback
    {
        void OnInvoked(IInvocationManager invocationManager, MethodInfo methodInfo, object obj, object[] arguments, object result);
    }
}

