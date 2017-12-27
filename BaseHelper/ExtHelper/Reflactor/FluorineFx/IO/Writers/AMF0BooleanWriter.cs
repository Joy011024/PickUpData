namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF0BooleanWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(1);
            writer.WriteBoolean((bool) data);
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

