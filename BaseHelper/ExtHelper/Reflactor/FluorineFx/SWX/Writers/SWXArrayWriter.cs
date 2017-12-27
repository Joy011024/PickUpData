namespace FluorineFx.SWX.Writers
{
    using FluorineFx.SWX;
    using System;

    internal class SWXArrayWriter : ISWXWriter
    {
        public void WriteData(SwxAssembler assembler, object data)
        {
            Array array = data as Array;
            assembler.PushArray(array);
        }

        public bool IsPrimitive
        {
            get
            {
                return false;
            }
        }
    }
}

