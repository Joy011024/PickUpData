namespace FluorineFx.SWX.Writers
{
    using FluorineFx.SWX;
    using FluorineFx.Util;
    using System;

    internal class SWXDoubleWriter : ISWXWriter
    {
        public void WriteData(SwxAssembler assembler, object data)
        {
            double num = Convert.ToDouble(data);
            assembler.PushDouble(num);
        }

        public bool IsPrimitive
        {
            get
            {
                return true;
            }
        }
    }
}

