namespace FluorineFx.Json
{
    using System;

    public enum WriteState
    {
        Error,
        Closed,
        Object,
        Array,
        Property,
        Start
    }
}

