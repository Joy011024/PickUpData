﻿namespace FluorineFx.IO.Readers
{
    using FluorineFx.IO;
    using System;

    internal class AMF3NullReader : IAMFReader
    {
        public object ReadData(AMFReader reader)
        {
            return null;
        }
    }
}

