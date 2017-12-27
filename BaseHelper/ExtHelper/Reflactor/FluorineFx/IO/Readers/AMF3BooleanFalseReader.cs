namespace FluorineFx.IO.Readers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3BooleanFalseReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return false;
        }
    }
}

