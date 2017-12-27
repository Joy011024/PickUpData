namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api;
    using System;

    public interface IStreamFilenameGenerator : IScopeService, IService
    {
        string GenerateFilename(IScope scope, string name, GenerationType type);
        string GenerateFilename(IScope scope, string name, string extension, GenerationType type);

        bool ResolvesToAbsolutePath { get; }
    }
}

