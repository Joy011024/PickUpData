namespace FluorineFx.Messaging.Api.Persistence
{
    using System;
    using System.Collections;

    public interface IPersistenceStore
    {
        ICollection GetObjectNames();
        ICollection GetObjects();
        bool Load(IPersistable obj);
        IPersistable Load(string name);
        bool Remove(IPersistable obj);
        bool Remove(string name);
        bool Save(IPersistable obj);
    }
}

