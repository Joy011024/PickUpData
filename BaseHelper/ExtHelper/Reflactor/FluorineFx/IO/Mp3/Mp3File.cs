namespace FluorineFx.IO.Mp3
{
    using FluorineFx.IO;
    using System;
    using System.IO;

    internal class Mp3File : IMp3File, IStreamableFile
    {
        private FileInfo _file;

        public Mp3File(FileInfo file)
        {
            this._file = file;
        }

        public ITagWriter GetAppendWriter()
        {
            return null;
        }

        public ITagReader GetReader()
        {
            return new Mp3Reader(this._file);
        }

        public ITagWriter GetWriter()
        {
            return null;
        }
    }
}

