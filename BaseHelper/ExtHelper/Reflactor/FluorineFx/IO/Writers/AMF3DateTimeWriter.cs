namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3DateTimeWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(8);
            writer.WriteAMF3DateTime((DateTime) data);
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

