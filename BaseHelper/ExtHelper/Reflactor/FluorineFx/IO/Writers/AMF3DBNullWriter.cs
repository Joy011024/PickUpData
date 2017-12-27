namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3DBNullWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteAMF3Null();
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

