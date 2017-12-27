namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3IntWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            int num = Convert.ToInt32(data);
            writer.WriteAMF3Int(num);
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

