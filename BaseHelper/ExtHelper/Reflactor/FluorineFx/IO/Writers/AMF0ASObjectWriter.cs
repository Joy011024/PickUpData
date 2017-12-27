namespace FluorineFx.IO.Writers
{
    using FluorineFx;
    using FluorineFx.IO;
    using System;

    internal class AMF0ASObjectWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteASO(ObjectEncoding.AMF0, data as ASObject);
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

