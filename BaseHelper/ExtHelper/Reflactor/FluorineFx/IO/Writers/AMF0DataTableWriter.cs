namespace FluorineFx.IO.Writers
{
    using FluorineFx;
    using FluorineFx.IO;
    using System;
    using System.Data;

    internal class AMF0DataTableWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteASO(ObjectEncoding.AMF0, TypeHelper.ConvertDataTableToASO(data as DataTable, true));
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

