namespace FluorineFx.Json
{
    using System;

    public enum JsonToken
    {
        None,
        StartObject,
        StartArray,
        PropertyName,
        Comment,
        Integer,
        Float,
        String,
        Boolean,
        Null,
        Undefined,
        EndObject,
        EndArray,
        Constructor,
        Date
    }
}

