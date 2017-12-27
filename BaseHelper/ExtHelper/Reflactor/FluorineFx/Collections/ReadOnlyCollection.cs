namespace FluorineFx.Collections
{
    using System;
    using System.Collections;
    using System.Threading;

    public class ReadOnlyCollection : ICollection, IEnumerable
    {
        private ICollection _collection;
        private object _syncRoot;

        public ReadOnlyCollection(ICollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("list");
            }
            this._collection = collection;
        }

        public void CopyTo(Array array, int index)
        {
            this._collection.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this._collection.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return this._collection.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public object SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    if (this._collection != null)
                    {
                        this._syncRoot = this._collection.SyncRoot;
                    }
                    else
                    {
                        Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
                    }
                }
                return this._syncRoot;
            }
        }
    }
}

