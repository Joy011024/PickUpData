namespace FluorineFx.IO.Writers
{
    using FluorineFx.AMF3;
    using FluorineFx.IO;
    using System;

    internal class AMF3ByteArrayWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            if (data is byte[])
            {
                data = new ByteArray(data as byte[]);
            }
            if (data is ByteArray)
            {
                writer.WriteByteArray(data as ByteArray);
            }
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

