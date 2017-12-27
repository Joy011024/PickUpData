namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;
    using System.Xml.Linq;

    internal class AMF0XElementWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteXElement(data as XElement);
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

