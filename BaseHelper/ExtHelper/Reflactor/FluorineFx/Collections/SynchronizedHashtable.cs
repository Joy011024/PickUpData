namespace FluorineFx.Collections
{
    using FluorineFx.Util;
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class SynchronizedHashtable : IDictionary, ICollection, IEnumerable, ICloneable
    {
        private readonly Hashtable _table;

        public SynchronizedHashtable()
        {
            this._table = new Hashtable();
        }

        public SynchronizedHashtable(IDictionary dictionary)
        {
            ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
            this._table = new Hashtable(dictionary);
        }

        public void Add(object key, object value)
        {
            lock (this.SyncRoot)
            {
                this._table.Add(key, value);
            }
        }

        public object AddIfAbsent(object key, object value)
        {
            lock (this.SyncRoot)
            {
                if (!this._table.ContainsKey(key))
                {
                    this._table.Add(key, value);
                    return value;
                }
                return this._table[key];
            }
        }

        public void Clear()
        {
            lock (this.SyncRoot)
            {
                this._table.Clear();
            }
        }

        public object Clone()
        {
            lock (this.SyncRoot)
            {
                return new SynchronizedHashtable(this);
            }
        }

        public bool Contains(object key)
        {
            lock (this.SyncRoot)
            {
                return this._table.Contains(key);
            }
        }

        public bool ContainsKey(object key)
        {
            lock (this.SyncRoot)
            {
                return this._table.ContainsKey(key);
            }
        }

        public bool ContainsValue(object value)
        {
            lock (this.SyncRoot)
            {
                return this._table.ContainsValue(value);
            }
        }

        public void CopyTo(Array array, int index)
        {
            lock (this.SyncRoot)
            {
                this._table.CopyTo(array, index);
            }
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            lock (this.SyncRoot)
            {
                return new SynchronizedDictionaryEnumerator(this.SyncRoot, this._table.GetEnumerator());
            }
        }

        public void Remove(object key)
        {
            lock (this.SyncRoot)
            {
                this._table.Remove(key);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (this.SyncRoot)
            {
                return new SynchronizedEnumerator(this.SyncRoot, ((IEnumerable) this._table).GetEnumerator());
            }
        }

        public int Count
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._table.Count;
                }
            }
        }

        public bool IsFixedSize
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._table.IsFixedSize;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._table.IsReadOnly;
                }
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return true;
            }
        }

        public object this[object key]
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._table[key];
                }
            }
            set
            {
                lock (this.SyncRoot)
                {
                    this._table[key] = value;
                }
            }
        }

        public ICollection Keys
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._table.Keys;
                }
            }
        }

        public object SyncRoot
        {
            get
            {
                return this._table.SyncRoot;
            }
        }

        public ICollection Values
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._table.Values;
                }
            }
        }
    }
}

