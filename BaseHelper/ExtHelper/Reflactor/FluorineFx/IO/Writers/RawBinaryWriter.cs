namespace FluorineFx.IO.Writers
{
    using FluorineFx.IO;
    using System;

    internal class RawBinaryWriter : IAMFWriter
    {
        public void WriteData(AMFWriter writer, object data)
        {
            writer.WriteBytes((data as RawBinary).Buffer);
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

