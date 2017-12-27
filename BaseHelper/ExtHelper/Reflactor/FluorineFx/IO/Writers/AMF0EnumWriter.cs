namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class AMF0EnumWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteByte(0);
            double num = Convert.ToInt32(data);
            writer.WriteDouble(num);
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

