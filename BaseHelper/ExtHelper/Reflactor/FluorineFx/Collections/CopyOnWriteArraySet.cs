namespace FluorineFx.Collections
{
    using System;
    using System.Collections;

    public class CopyOnWriteArraySet : ICollection, IEnumerable
    {
        private CopyOnWriteArray _array;

        public CopyOnWriteArraySet()
        {
            this._array = new CopyOnWriteArray();
        }

        public CopyOnWriteArraySet(ICollection collection)
        {
            this._array = new CopyOnWriteArray();
            foreach (object obj2 in collection)
            {
                this._array.Add(obj2);
            }
        }

        public bool Add(object value)
        {
            if (!this._array.Contains(value))
            {
                this._array.Add(value);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            this._array.Clear();
        }

        public bool Contains(object value)
        {
            return this._array.Contains(value);
        }

        public void CopyTo(Array array, int index)
        {
            this._array.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this._array.GetEnumerator();
        }

        public void Remove(object obj)
        {
            this._array.Remove(obj);
        }

        public int Count
        {
            get
            {
                return this._array.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (this._array.Count == 0);
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

