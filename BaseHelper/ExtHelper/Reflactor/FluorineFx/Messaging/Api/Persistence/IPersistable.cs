namespace FluorineFx.Messaging.Api.Persistence
{
    using FluorineFx.IO;
    using System;

    public interface IPersistable
    {
        void Deserialize(AMFReader reader);
        void Serialize(AMFWriter writer);

        bool IsPersistent { get; set; }

        long LastModified { get; }

        string Name { get; set; }

        string Path { get; set; }

        IPersistenceStore Store { get; set; }
    }
}

