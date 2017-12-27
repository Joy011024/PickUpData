namespace FluorineFx.Collections
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class CopyOnWriteDictionary : IDictionary, ICollection, IEnumerable
    {
        private Hashtable _dictionary;

        public CopyOnWriteDictionary()
        {
            this._dictionary = new Hashtable();
        }

        public CopyOnWriteDictionary(IDictionary d)
        {
            this._dictionary = new Hashtable(d);
        }

        public CopyOnWriteDictionary(int capacity)
        {
            this._dictionary = new Hashtable(capacity);
        }

        public void Add(object key, object value)
        {
            lock (this.SyncRoot)
            {
                Hashtable hashtable = new Hashtable(this._dictionary);
                hashtable.Add(key, value);
                this._dictionary = hashtable;
            }
        }

        public void Clear()
        {
            lock (this.SyncRoot)
            {
                this._dictionary = new Hashtable();
            }
        }

        public bool Contains(object key)
        {
            return this._dictionary.ContainsKey(key);
        }

        public void CopyTo(Array array, int index)
        {
            this._dictionary.CopyTo(array, index);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return this._dictionary.GetEnumerator();
        }

        public void Remove(object key)
        {
            lock (this.SyncRoot)
            {
                if (this.Contains(key))
                {
                    Hashtable hashtable = new Hashtable(this._dictionary);
                    hashtable.Remove(key);
                    this._dictionary = hashtable;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._dictionary.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return this._dictionary.Count;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return this._dictionary.IsFixedSize;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this._dictionary.IsReadOnly;
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
                return this._dictionary[key];
            }
            set
            {
                lock (this.SyncRoot)
                {
                    Hashtable hashtable = new Hashtable(this._dictionary);
                    hashtable[key] = value;
                    this._dictionary = hashtable;
                }
            }
        }

        public ICollection Keys
        {
            get
            {
                return this._dictionary.Keys;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this._dictionary.SyncRoot;
            }
        }

        public ICollection Values
        {
            get
            {
                return this._dictionary.Values;
            }
        }
    }
}

