namespace FluorineFx.IO.Readers
{
    using FluorineFx.IO;
    using System;

    internal class AMF0ReferenceReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadReference();
        }
    }
}

