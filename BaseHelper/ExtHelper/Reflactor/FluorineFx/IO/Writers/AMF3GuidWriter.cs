namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3GuidWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteAMF3String(((Guid) data).ToString());
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

