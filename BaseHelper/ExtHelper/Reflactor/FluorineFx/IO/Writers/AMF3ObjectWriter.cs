namespace FluorineFx.IO.Writers
{
    using FluorineFx;
    using FluorineFx.AMF3;
    using FluorineFx.IO;
    using System;
    using System.Collections;

    internal class AMF3ObjectWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            if (data is IList)
            {
                if (writer.UseLegacyCollection)
                {
                    writer.WriteByte(9);
                    writer.WriteAMF3Array(data as IList);
                }
                else
                {
                    writer.WriteByte(10);
                    object obj2 = new ArrayCollection(data as IList);
                    writer.WriteAMF3Object(obj2);
                }
            }
            else if (data is IDictionary)
            {
                writer.WriteByte(10);
                writer.WriteAMF3Object(data);
            }
            else if (data is Exception)
            {
                writer.WriteByte(10);
                writer.WriteAMF3Object(new ExceptionASO(data as Exception));
            }
            else if (data is IExternalizable)
            {
                writer.WriteByte(10);
                writer.WriteAMF3Object(data);
            }
            else
            {
                writer.WriteByte(10);
                writer.WriteAMF3Object(data);
            }
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

