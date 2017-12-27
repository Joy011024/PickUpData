namespace FluorineFx.IO.Readers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3ArrayReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadAMF3Array();
        }
    }
}

