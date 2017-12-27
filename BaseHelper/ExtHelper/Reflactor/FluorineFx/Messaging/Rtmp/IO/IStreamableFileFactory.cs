namespace FluorineFx.Messaging.Rtmp.IO
{
    using FluorineFx.Messaging.Api;
    using System;
    using System.Collections.Generic;
    using System.IO;

    [CLSCompliant(false)]
    public interface IStreamableFileFactory : IScopeService, IService
    {
        IStreamableFileService GetService(FileInfo file);
        ICollection<IStreamableFileService> GetServices();
    }
}

