namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF0AMF3TagWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(0x11);
            writer.WriteAMF3Data(data);
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

