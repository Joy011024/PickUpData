namespace FluorineFx.IO.Readers
{
    using FluorineFx.IO;
    using System;

    internal class AMF0AssociativeArrayReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAssociativeArray();
        }
    }
}

