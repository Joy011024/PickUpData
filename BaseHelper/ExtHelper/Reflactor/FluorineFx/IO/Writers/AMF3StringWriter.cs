namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3StringWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteAMF3String(data as string);
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

