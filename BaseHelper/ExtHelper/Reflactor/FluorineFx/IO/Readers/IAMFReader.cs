namespace FluorineFx.IO.Readers
{
    using FluorineFx.IO;
    using System;

    internal interface IAMFReader
    {
        object ReadData(AMFReader reader);
    }
}

