namespace FluorineFx.Messaging.Rtmp.IO.Mp3
{
    using FluorineFx.IO;
    using FluorineFx.IO.Mp3;
    using FluorineFx.Messaging.Rtmp.IO;
    using System;
    using System.IO;

    internal class Mp3Service : BaseStreamableFileService, IMp3Service, IStreamableFileService
    {
        public override IStreamableFile GetStreamableFile(FileInfo file)
        {
            return new Mp3File(file);
        }

        public override string Extension
        {
            get
            {
                return ".mp3";
            }
        }

        public override string Prefix
        {
            get
            {
                return "mp3";
            }
        }
    }
}

