namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF0DBNullWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteNull();
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

