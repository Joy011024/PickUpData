namespace FluorineFx.Collections.Generic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public class CollectionBase<T> : IList<T>, IList, ICollection<T>, ICollection, IEnumerable<T>, IEnumerable
    {
        private List<T> _innerList;

        public CollectionBase() : this(10)
        {
        }

        public CollectionBase(int initialCapacity)
        {
            this._innerList = new List<T>(initialCapacity);
        }

        public virtual int Add(object value)
        {
            int count = this._innerList.Count;
            if (!this.OnValidate((T) value))
            {
                return -1;
            }
            if (!this.OnInsert(count, (T) value))
            {
                return -1;
            }
            count = this._innerList.Add(value);
            this.OnInsertComplete(count, (T) value);
            return count;
        }

        public virtual void Add(T item)
        {
            if (this.OnValidate(item) && this.OnInsert(this._innerList.Count, item))
            {
                this._innerList.Add(item);
                this.OnInsertComplete(this._innerList.Count - 1, item);
            }
        }

        public virtual void Clear()
        {
            if (this.OnClear())
            {
                this._innerList.Clear();
                this.OnClearComplete();
            }
        }

        public virtual bool Contains(T item)
        {
            return this._innerList.Contains(item);
        }

        public virtual bool Contains(object value)
        {
            return this._innerList.Contains(value);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            this._innerList.CopyTo(array, arrayIndex);
        }

        public virtual void CopyTo(Array array, int index)
        {
            this._innerList.CopyTo(array, index);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return this._innerList.GetEnumerator();
        }

        public virtual int IndexOf(T item)
        {
            return this._innerList.IndexOf(item);
        }

        public virtual int IndexOf(object value)
        {
            return this._innerList.IndexOf(value);
        }

        public virtual void Insert(int index, T item)
        {
            if (this.OnValidate(item) && this.OnInsert(index, item))
            {
                this._innerList.Insert(index, item);
                this.OnInsertComplete(index, item);
            }
        }

        public virtual void Insert(int index, object value)
        {
            if (this.OnValidate((T) value) && this.OnInsert(index, (T) value))
            {
                this._innerList.Insert(index, value);
                this.OnInsertComplete(index, (T) value);
            }
        }

        protected virtual bool OnClear()
        {
            return true;
        }

        protected virtual void OnClearComplete()
        {
        }

        protected virtual bool OnInsert(int index, T value)
        {
            return true;
        }

        protected virtual void OnInsertComplete(int index, T value)
        {
        }

        protected virtual bool OnRemove(int index, T value)
        {
            return true;
        }

        protected virtual void OnRemoveComplete(int index, T value)
        {
        }

        protected virtual bool OnSet(int index, T oldValue, T value)
        {
            return true;
        }

        protected virtual void OnSetComplete(int index, T oldValue, T newValue)
        {
        }

        protected virtual bool OnValidate(T value)
        {
            return true;
        }

        public virtual bool Remove(T item)
        {
            int index = this._innerList.IndexOf(item);
            if (index < 0)
            {
                return false;
            }
            if (!this.OnValidate(item))
            {
                return false;
            }
            if (!this.OnRemove(index, item))
            {
                return false;
            }
            this._innerList.Remove(item);
            this.OnRemoveComplete(index, item);
            return true;
        }

        public virtual void Remove(object value)
        {
            int index = this._innerList.IndexOf((T) value);
            if (((index >= 0) && this.OnValidate((T) value)) && this.OnRemove(index, (T) value))
            {
                this._innerList.Remove(value);
                this.OnRemoveComplete(index, (T) value);
            }
        }

        public virtual void RemoveAt(int index)
        {
            T local = this._innerList[index];
            if (this.OnValidate(local) && this.OnRemove(index, local))
            {
                this._innerList.RemoveAt(index);
                this.OnRemoveComplete(index, local);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._innerList.GetEnumerator();
        }

        public virtual int Count
        {
            get
            {
                return this._innerList.Count;
            }
        }

        public virtual bool IsFixedSize
        {
            get
            {
                return ((IList) this._innerList).IsFixedSize;
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return this._innerList.IsReadOnly;
            }
        }

        public virtual bool IsSynchronized
        {
            get
            {
                return ((ICollection) this._innerList).IsSynchronized;
            }
        }

        public virtual T this[int index]
        {
            get
            {
                return this._innerList[index];
            }
            set
            {
                T oldValue = this._innerList[index];
                if (this.OnValidate(value) && this.OnSet(index, oldValue, value))
                {
                    this._innerList[index] = value;
                    this.OnSetComplete(index, oldValue, value);
                }
            }
        }

        public virtual object SyncRoot
        {
            get
            {
                return ((ICollection) this._innerList).SyncRoot;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this._innerList[index];
            }
            set
            {
                T oldValue = this._innerList[index];
                if (this.OnValidate((T) value) && this.OnSet(index, oldValue, (T) value))
                {
                    this._innerList[index] = (T) value;
                    this.OnSetComplete(index, oldValue, (T) value);
                }
            }
        }
    }
}

