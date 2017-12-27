namespace FluorineFx.Messaging.Rtmp.IO
{
    using FluorineFx.IO;
    using System;
    using System.IO;

    [CLSCompliant(false)]
    public interface IStreamableFileService
    {
        bool CanHandle(FileInfo file);
        IStreamableFile GetStreamableFile(FileInfo file);
        string PrepareFilename(string name);

        string Extension { get; }

        string Prefix { get; }
    }
}

