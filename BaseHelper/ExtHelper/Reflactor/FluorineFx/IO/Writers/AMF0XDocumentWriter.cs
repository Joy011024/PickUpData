namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;
    using System.Xml.Linq;

    internal class AMF0XDocumentWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteXDocument(data as XDocument);
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

