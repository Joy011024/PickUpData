namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3CharWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteAMF3String(new string((char) data, 1));
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

