namespace FluorineFx.Collections
{
    using System;

    internal class Inserter : IOutputIterator
    {
        private IModifiableCollection _collection;

        public Inserter(IModifiableCollection collection)
        {
            this._collection = collection;
        }

        public void Add(object obj)
        {
            this._collection.Add(obj);
        }
    }
}

