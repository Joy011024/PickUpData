namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF0GuidWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteString(((Guid) data).ToString());
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

