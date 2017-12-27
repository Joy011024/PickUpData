namespace FluorineFx.Json
{
    using FluorineFx.Collections;
    using System;
    using System.Collections;

    public class JavaScriptParameters : ReadOnlyList
    {
        public static readonly JavaScriptParameters Empty = new JavaScriptParameters(new ArrayList());

        public JavaScriptParameters(IList list) : base(list)
        {
        }
    }
}

