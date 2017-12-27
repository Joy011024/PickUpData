namespace FluorineFx.Context
{
    using System;
    using System.Reflection;

    public interface IApplicationState
    {
        void Add(string name, object value);
        void Remove(string key);
        void RemoveAt(int index);

        object this[int index] { get; }

        object this[string name] { get; set; }
    }
}

