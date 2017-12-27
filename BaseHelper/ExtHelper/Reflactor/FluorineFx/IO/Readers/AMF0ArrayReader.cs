namespace FluorineFx.IO.Readers
{
    using FluorineFx.IO;
    using System;

    internal class AMF0ArrayReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadArray();
        }
    }
}

