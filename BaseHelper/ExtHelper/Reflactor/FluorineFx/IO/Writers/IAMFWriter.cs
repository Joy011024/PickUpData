namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal interface IAMFWriter
    {
        void WriteData(AMFWriter writer, object data);

        bool IsPrimitive { get; }
    }
}

