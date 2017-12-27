namespace FluorineFx.Messaging.Rtmp.IO
{
    using FluorineFx.Configuration;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Rtmp.IO.Flv;
    using FluorineFx.Messaging.Rtmp.IO.Mp3;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class StreamableFileFactory : IStreamableFileFactory, IScopeService, IService
    {
        private List<IStreamableFileService> _services = new List<IStreamableFileService>();
        private static ILog log = LogManager.GetLogger(typeof(StreamableFileFactory));

        public StreamableFileFactory()
        {
            this._services.Add(new FlvService());
            this._services.Add(new Mp3Service());
        }

        public IStreamableFileService GetService(FileInfo file)
        {
            log.Info("Get service for file: " + file.Name);
            foreach (IStreamableFileService service in this._services)
            {
                if (service.CanHandle(file))
                {
                    log.Info("Found service");
                    return service;
                }
            }
            return null;
        }

        public ICollection<IStreamableFileService> GetServices()
        {
            return this._services;
        }

        public void Shutdown()
        {
        }

        public void Start(ConfigurationSection configuration)
        {
        }
    }
}

