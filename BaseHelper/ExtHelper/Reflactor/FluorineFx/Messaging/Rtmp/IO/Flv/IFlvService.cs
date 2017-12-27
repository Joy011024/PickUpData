namespace FluorineFx.Messaging.Rtmp.IO.Flv
{
    using FluorineFx.IO;
    using FluorineFx.Messaging.Rtmp.IO;
    using System;

    [CLSCompliant(false)]
    public interface IFlvService : IStreamableFileService
    {
        AMFWriter Serializer { get; set; }
    }
}

