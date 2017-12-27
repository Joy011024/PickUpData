namespace FluorineFx.Messaging.Rtmp.IO
{
    using FluorineFx.IO;
    using System;
    using System.IO;

    internal abstract class BaseStreamableFileService : IStreamableFileService
    {
        protected BaseStreamableFileService()
        {
        }

        public bool CanHandle(FileInfo file)
        {
            return file.FullName.ToLower().EndsWith(this.Extension.ToLower());
        }

        public abstract IStreamableFile GetStreamableFile(FileInfo file);
        public string PrepareFilename(string name)
        {
            if (name.StartsWith(this.Prefix + ':'))
            {
                name = name.Substring(this.Prefix.Length + 1);
                if (!name.EndsWith(this.Extension))
                {
                    name = name + this.Extension;
                }
            }
            return name;
        }

        public abstract string Extension { get; }

        public abstract string Prefix { get; }
    }
}

