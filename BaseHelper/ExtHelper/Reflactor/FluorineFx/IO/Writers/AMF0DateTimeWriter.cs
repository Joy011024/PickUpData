namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF0DateTimeWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(11);
            writer.WriteDateTime((DateTime) data);
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

