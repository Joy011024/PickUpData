namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3ArrayWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(9);
            writer.WriteAMF3Array(data as Array);
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

