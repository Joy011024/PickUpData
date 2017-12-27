namespace FluorineFx.Json
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    [TypeConverter(typeof(JavaScriptArrayConverter))]
    public class JavaScriptArray : ArrayList
    {
        public JavaScriptArray()
        {
        }

        public JavaScriptArray(ICollection collection) : base(collection)
        {
        }

        public JavaScriptArray(int capacity) : base(capacity)
        {
        }
    }
}

