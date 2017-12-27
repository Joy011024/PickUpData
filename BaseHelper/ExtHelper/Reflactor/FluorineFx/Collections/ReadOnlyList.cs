namespace FluorineFx.Collections
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Threading;

    public class ReadOnlyList : IList, ICollection, IEnumerable
    {
        private IList _list;
        private object _syncRoot;

        public ReadOnlyList(IList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            this._list = list;
        }

        public int Add(object value)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(object value)
        {
            return this._list.Contains(value);
        }

        public void CopyTo(Array array, int index)
        {
            this._list.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        public int IndexOf(object value)
        {
            return this._list.IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        public void Remove(object value)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public int Count
        {
            get
            {
                return this._list.Count;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return true;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public object this[int index]
        {
            get
            {
                return this._list[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public object SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    ICollection is2 = this._list;
                    if (this._list != null)
                    {
                        this._syncRoot = this._list.SyncRoot;
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

