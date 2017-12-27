namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx.IO;
    using System;
    using System.IO;

    internal class RtmpWriter : AMFWriter
    {
        public RtmpWriter(Stream stream) : base(stream)
        {
        }
    }
}

