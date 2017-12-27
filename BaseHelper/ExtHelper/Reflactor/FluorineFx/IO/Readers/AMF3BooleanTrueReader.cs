namespace FluorineFx.IO.Readers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3BooleanTrueReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return true;
        }
    }
}

