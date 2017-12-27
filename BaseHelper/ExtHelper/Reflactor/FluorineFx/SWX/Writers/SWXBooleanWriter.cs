namespace FluorineFx.SWX.Writers
{
    using FluorineFx.SWX;
    using FluorineFx.Util;
    using System;

    internal class SWXBooleanWriter : ISWXWriter
    {
        public void WriteData(SwxAssembler assembler, object data)
        {
            bool flag = Convert.ToBoolean(data);
            assembler.PushBoolean(flag);
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

