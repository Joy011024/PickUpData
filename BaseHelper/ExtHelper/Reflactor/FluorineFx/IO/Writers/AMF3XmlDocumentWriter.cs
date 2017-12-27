namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;
    using System.Xml;

    internal class AMF3XmlDocumentWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteAMF3XmlDocument(data as XmlDocument);
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

