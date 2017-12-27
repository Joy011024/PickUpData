namespace FluorineFx.Context
{
    using FluorineFx.Util;
    using System;
    using System.Globalization;
    using System.IO;

    internal class FileSystemResource : IResource
    {
        private FileInfo _fileHandle;
        protected const string DefaultBasePathPlaceHolder = "~";
        public const string ProtocolSeparator = "://";

        public FileSystemResource(string resourceName)
        {
            this._fileHandle = this.ResolveFileHandle(resourceName);
        }

        protected static string GetResourceNameWithoutProtocol(string resourceName)
        {
            int index = resourceName.IndexOf("://");
            if (index == -1)
            {
                return resourceName;
            }
            return resourceName.Substring(index + "://".Length);
        }

        protected virtual string ResolveBasePathPlaceHolder(string resourceName, string basePathPlaceHolder)
        {
            if ((resourceName[0] == '/') && (resourceName[2] == ':'))
            {
                resourceName = resourceName.Substring(1);
            }
            if (StringUtils.HasText(resourceName) && resourceName.TrimStart(new char[0]).StartsWith(basePathPlaceHolder))
            {
                return resourceName.Replace(basePathPlaceHolder, AppDomain.CurrentDomain.BaseDirectory).TrimStart(new char[0]);
            }
            return resourceName;
        }

        protected virtual FileInfo ResolveFileHandle(string resourceName)
        {
            return new FileInfo(this.ResolveResourceNameWithoutProtocol(resourceName));
        }

        protected virtual string ResolveResourceNameWithoutProtocol(string resourceName)
        {
            return this.ResolveBasePathPlaceHolder(GetResourceNameWithoutProtocol(resourceName), "~");
        }

        public string Description
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "file [{0}]", new object[] { this._fileHandle.FullName });
            }
        }

        public bool Exists
        {
            get
            {
                return ((this._fileHandle != null) && this._fileHandle.Exists);
            }
        }

        public FileInfo File
        {
            get
            {
                return this._fileHandle;
            }
        }
    }
}

