namespace FluorineFx.Collections
{
    using System;
    using System.Collections;

    public interface IModifiableCollection : ICollection, IEnumerable
    {
        bool Add(object key);
        bool AddIfNotContains(object key);
        void Clear();
        object Find(object key);
        ICollection FindAll(object key);
        int Remove(object key);

        bool IsReadOnly { get; }
    }
}

