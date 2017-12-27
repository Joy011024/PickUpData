namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3BooleanWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteAMF3Bool((bool) data);
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

