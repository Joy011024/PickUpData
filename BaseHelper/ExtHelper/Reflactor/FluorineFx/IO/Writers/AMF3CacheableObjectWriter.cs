namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3CacheableObjectWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteAMF3Data((data as CacheableObject).Object);
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

