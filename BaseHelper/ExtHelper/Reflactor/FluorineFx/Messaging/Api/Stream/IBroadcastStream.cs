namespace FluorineFx.Messaging.Api.Stream
{
    using FluorineFx.Messaging.Api.Messaging;
    using System;

    [CLSCompliant(false)]
    public interface IBroadcastStream : IStream
    {
        void SaveAs(string filePath, bool isAppend);

        IProvider Provider { get; }

        string PublishedName { get; set; }

        string SaveFilename { get; }
    }
}

