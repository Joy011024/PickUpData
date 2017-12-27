namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx.IO;
    using System;
    using System.IO;

    internal class RtmpReader : AMFReader
    {
        public RtmpReader(Stream stream) : base(stream)
        {
        }
    }
}

