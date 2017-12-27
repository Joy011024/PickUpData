namespace FluorineFx.IO.Writers
{
    using FluorineFx;
    using FluorineFx.IO;
    using System;

    internal class AMF0CacheableObjectWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteData(ObjectEncoding.AMF0, (data as CacheableObject).Object);
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

