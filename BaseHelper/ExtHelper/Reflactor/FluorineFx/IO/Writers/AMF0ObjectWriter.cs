namespace FluorineFx.IO.Writers
{
    using FluorineFx;
    using FluorineFx.IO;
    using System;
    using System.Collections;

    internal class AMF0ObjectWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            if (data is IList)
            {
                IList list = data as IList;
                object[] array = new object[list.Count];
                list.CopyTo(array, 0);
                writer.WriteArray(ObjectEncoding.AMF0, array);
            }
            else if (data is IDictionary)
            {
                writer.WriteAssociativeArray(ObjectEncoding.AMF0, data as IDictionary);
            }
            else if (data is Exception)
            {
                writer.WriteASO(ObjectEncoding.AMF0, new ExceptionASO(data as Exception));
            }
            else
            {
                writer.WriteObject(ObjectEncoding.AMF0, data);
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

