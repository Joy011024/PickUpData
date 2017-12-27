namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF0CharWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(2);
            writer.WriteUTF(new string((char) data, 1));
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

