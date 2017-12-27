namespace FluorineFx.IO.Writers
{
    using FluorineFx;
    using FluorineFx.IO;
    using System;

    internal class AMF0ArrayWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteArray(ObjectEncoding.AMF0, data as Array);
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

