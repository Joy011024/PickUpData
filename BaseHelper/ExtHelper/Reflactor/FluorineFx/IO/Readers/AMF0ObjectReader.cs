namespace FluorineFx.IO.Readers
{
    using FluorineFx.IO;
    using System;

    internal class AMF0ObjectReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadObject();
        }
    }
}

