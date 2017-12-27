namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF0StringWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteString(data as string);
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

