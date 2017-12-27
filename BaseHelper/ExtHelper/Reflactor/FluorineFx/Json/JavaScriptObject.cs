namespace FluorineFx.Json
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [TypeConverter(typeof(JavaScriptObjectConverter))]
    public class JavaScriptObject : Dictionary<string, object>
    {
        public JavaScriptObject() : base(EqualityComparer<string>.Default)
        {
        }

        public JavaScriptObject(JavaScriptObject javaScriptObject) : base(javaScriptObject, EqualityComparer<string>.Default)
        {
        }
    }
}

