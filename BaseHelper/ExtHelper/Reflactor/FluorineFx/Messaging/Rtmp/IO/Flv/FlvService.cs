namespace FluorineFx.Messaging.Rtmp.IO.Flv
{
    using FluorineFx.IO;
    using FluorineFx.IO.FLV;
    using FluorineFx.Messaging.Rtmp.IO;
    using System;
    using System.IO;

    internal class FlvService : BaseStreamableFileService, IFlvService, IStreamableFileService
    {
        private AMFReader _amfReader;
        private AMFWriter _amfWriter;
        private bool _generateMetadata;

        public override IStreamableFile GetStreamableFile(FileInfo file)
        {
            return new Flv(file, this._generateMetadata);
        }

        public AMFReader Deserializer
        {
            get
            {
                return this._amfReader;
            }
            set
            {
                this._amfReader = value;
            }
        }

        public override string Extension
        {
            get
            {
                return ".flv";
            }
        }

        public bool GenerateMetadata
        {
            get
            {
                return this._generateMetadata;
            }
            set
            {
                this._generateMetadata = value;
            }
        }

        public override string Prefix
        {
            get
            {
                return "flv";
            }
        }

        public AMFWriter Serializer
        {
            get
            {
                return this._amfWriter;
            }
            set
            {
                this._amfWriter = value;
            }
        }
    }
}

