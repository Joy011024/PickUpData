namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;
    using System.Xml.Linq;

    internal class AMF3XElementWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteAMF3XElement(data as XElement);
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

