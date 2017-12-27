namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Configuration;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Stream;
    using FluorineFx.Messaging.Rtmp.Persistence;
    using System;

    internal class DefaultStreamFilenameGenerator : IStreamFilenameGenerator, IScopeService, IService
    {
        public string GenerateFilename(IScope scope, string name, GenerationType type)
        {
            return this.GenerateFilename(scope, name, null, type);
        }

        public string GenerateFilename(IScope scope, string name, string extension, GenerationType type)
        {
            string str = this.GetStreamDirectory(scope) + name;
            if (!((extension == null) || string.Empty.Equals(extension)))
            {
                str = str + extension;
            }
            return str;
        }

        private string GetStreamDirectory(IScope scope)
        {
            return PersistenceUtils.GetPath(scope, "streams");
        }

        public void Shutdown()
        {
        }

        public void Start(ConfigurationSection configuration)
        {
        }

        public bool ResolvesToAbsolutePath
        {
            get
            {
                return false;
            }
        }
    }
}

