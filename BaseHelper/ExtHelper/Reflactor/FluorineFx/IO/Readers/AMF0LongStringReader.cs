namespace FluorineFx.IO.Readers
{
    using FluorineFx.IO;
    using System;

    internal class AMF0LongStringReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            int length = reader.ReadInt32();
            return reader.ReadUTF(length);
        }
    }
}

