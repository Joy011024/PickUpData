namespace FluorineFx.IO.Writers
{
    using FluorineFx;
    using FluorineFx.IO;
    using System;
    using System.Data;

    internal class AMF0DataSetWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteASO(ObjectEncoding.AMF0, TypeHelper.ConvertDataSetToASO(data as DataSet, true));
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

