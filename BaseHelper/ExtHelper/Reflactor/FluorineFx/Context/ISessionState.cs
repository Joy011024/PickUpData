namespace FluorineFx.Context
{
    using System;
    using System.Collections;
    using System.Reflection;

    public interface ISessionState : ICollection, IEnumerable
    {
        void Add(string name, object value);
        void Clear();
        void Remove(string name);
        void RemoveAll();
        void RemoveAt(int index);

        object this[string name] { get; set; }

        object this[int index] { get; set; }

        string SessionID { get; }
    }
}

