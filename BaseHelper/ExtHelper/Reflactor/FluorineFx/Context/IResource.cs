namespace FluorineFx.Context
{
    using System;
    using System.IO;

    public interface IResource
    {
        string Description { get; }

        bool Exists { get; }

        FileInfo File { get; }
    }
}

