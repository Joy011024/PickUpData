namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3DoubleWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            double num = Convert.ToDouble(data);
            writer.WriteAMF3Double(num);
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

