namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3ASObjectWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(10);
            writer.WriteAMF3Object(data);
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

