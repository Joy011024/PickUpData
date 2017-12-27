namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;
    using System.Xml;

    internal class AMF0XmlDocumentWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteXmlDocument(data as XmlDocument);
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

