namespace FluorineFx.SWX.Writers
{
    using FluorineFx.SWX;
    using FluorineFx.Util;
    using System;

    internal class SWXStringWriter : ISWXWriter
    {
        public void WriteData(SwxAssembler assembler, object data)
        {
            string str = Convert.ToString(data);
            assembler.PushString(str);
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

