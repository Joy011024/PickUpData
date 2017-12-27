namespace FluorineFx.IO.Readers
{
    using FluorineFx.IO;
    using System;

    internal class AMF0BooleanReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return reader.ReadBoolean();
        }
    }
}

