namespace FluorineFx.Util.Nullables
{
    using System;

    public interface INullableType
    {
        bool HasValue { get; }

        object Value { get; }
    }
}

