namespace FluorineFx.SWX.Writers
{
    using FluorineFx.SWX;
    using System;

    internal interface ISWXWriter
    {
        void WriteData(SwxAssembler assembler, object data);

        bool IsPrimitive { get; }
    }
}

