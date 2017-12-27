namespace FluorineFx.IO.Readers
{
    using FluorineFx.Exceptions;
    using FluorineFx.IO;
    using System;

    internal class AMFUnknownTagReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            throw new UnexpectedAMF();
        }
    }
}

