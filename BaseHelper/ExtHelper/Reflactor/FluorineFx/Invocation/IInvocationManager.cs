namespace FluorineFx.Invocation
{
    using System;
    using System.Collections.Generic;

    public interface IInvocationManager
    {
        Stack<object> Context { get; }

        Dictionary<object, object> Properties { get; }

        object Result { get; set; }
    }
}

