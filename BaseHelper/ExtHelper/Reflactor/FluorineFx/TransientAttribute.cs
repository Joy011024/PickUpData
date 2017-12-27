namespace FluorineFx
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple=false)]
    public class TransientAttribute : Attribute
    {
    }
}

