namespace FluorineFx.Collections.Generic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class CopyOnWriteArraySet<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        private CopyOnWriteArray<T> _array;

        public CopyOnWriteArraySet()
        {
            this._array = new CopyOnWriteArray<T>();
        }

        public CopyOnWriteArraySet(ICollection collection)
        {
            this._array = new CopyOnWriteArray<T>();
            foreach (object obj2 in collection)
            {
                this._array.Add((T) obj2);
            }
        }

        public void Add(T item)
        {
            if (!this._array.Contains(item))
            {
                this._array.Add(item);
            }
        }

        public void Clear()
        {
            this._array.Clear();
        }

        public bool Contains(T item)
        {
            return this._array.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this._array.CopyTo(array, arrayIndex);
        }

        public void CopyTo(Array array, int index)
        {
            this._array.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this._array.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return this._array.Remove(item);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this._array.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return this._array.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
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
                return this._array.SyncRoot;
            }
        }
    }
}

