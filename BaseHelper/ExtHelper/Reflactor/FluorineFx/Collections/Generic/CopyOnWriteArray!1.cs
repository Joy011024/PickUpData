namespace FluorineFx.Collections.Generic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class CopyOnWriteArray<T> : IList<T>, IList, ICollection<T>, ICollection, IEnumerable<T>, IEnumerable
    {
        private T[] _array;
        private static object _objLock;

        static CopyOnWriteArray()
        {
            CopyOnWriteArray<T>._objLock = new object();
        }

        public CopyOnWriteArray()
        {
            this._array = new T[0];
        }

        public CopyOnWriteArray(IList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            this._array = new T[list.Count];
            int num = 0;
            foreach (T local in list)
            {
                this._array[num++] = local;
            }
        }

        public int Add(object value)
        {
            lock (this.SyncRoot)
            {
                int length = this._array.Length;
                T[] destinationArray = new T[length + 1];
                Array.Copy(this._array, 0, destinationArray, 0, length);
                destinationArray[length] = (T) value;
                this._array = destinationArray;
                return length;
            }
        }

        public void Add(T item)
        {
            lock (this.SyncRoot)
            {
                int length = this._array.Length;
                T[] destinationArray = new T[length + 1];
                Array.Copy(this._array, 0, destinationArray, 0, length);
                destinationArray[length] = item;
                this._array = destinationArray;
            }
        }

        public void Clear()
        {
            lock (this.SyncRoot)
            {
                this._array = new T[0];
            }
        }

        public bool Contains(T item)
        {
            return (this.IndexOf(item) > -1);
        }

        public bool Contains(object value)
        {
            return (this.IndexOf(value) > -1);
        }

        private void Copy(T[] src, int offset, int count)
        {
            lock (this.SyncRoot)
            {
                this._array = new T[count];
                Array.Copy(src, offset, this._array, 0, count);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(this._array, 0, array, arrayIndex, array.Length - arrayIndex);
        }

        public void CopyTo(Array array, int index)
        {
            Array.Copy(this._array, 0, array, index, array.Length - index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator<T>(this._array);
        }

        public int IndexOf(T item)
        {
            T[] elementData = this._array;
            int length = elementData.Length;
            return CopyOnWriteArray<T>.IndexOf(item, elementData, length);
        }

        public int IndexOf(object value)
        {
            T[] elementData = this._array;
            int length = elementData.Length;
            return CopyOnWriteArray<T>.IndexOf((T) value, elementData, length);
        }

        private static int IndexOf(T elem, T[] elementData, int length)
        {
            int num;
            if (elem == null)
            {
                for (num = 0; num < length; num++)
                {
                    if (elementData[num] == null)
                    {
                        return num;
                    }
                }
            }
            else
            {
                for (num = 0; num < length; num++)
                {
                    if (elem.Equals(elementData[num]))
                    {
                        return num;
                    }
                }
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            lock (this.SyncRoot)
            {
                int length = this._array.Length;
                T[] destinationArray = new T[length + 1];
                Array.Copy(this._array, 0, destinationArray, 0, index);
                destinationArray[index] = item;
                Array.Copy(this._array, index, destinationArray, index + 1, length - index);
                this._array = destinationArray;
            }
        }

        public void Insert(int index, object value)
        {
            lock (this.SyncRoot)
            {
                int length = this._array.Length;
                T[] destinationArray = new T[length + 1];
                Array.Copy(this._array, 0, destinationArray, 0, index);
                destinationArray[index] = (T) value;
                Array.Copy(this._array, index, destinationArray, index + 1, length - index);
                this._array = destinationArray;
            }
        }

        public bool Remove(T item)
        {
            lock (this.SyncRoot)
            {
                int length = this._array.Length;
                if (length == 0)
                {
                    return false;
                }
                int index = length - 1;
                T[] localArray = new T[index];
                for (int i = 0; i < index; i++)
                {
                    if (item.Equals(this._array[i]))
                    {
                        for (int j = i + 1; j < length; j++)
                        {
                            localArray[j - 1] = this._array[j];
                        }
                        this._array = localArray;
                        return true;
                    }
                    localArray[i] = this._array[i];
                }
                if (item.Equals(this._array[index]))
                {
                    this._array = localArray;
                }
                return true;
            }
        }

        public void Remove(object value)
        {
            lock (this.SyncRoot)
            {
                int length = this._array.Length;
                if (length != 0)
                {
                    int index = length - 1;
                    T[] localArray = new T[index];
                    for (int i = 0; i < index; i++)
                    {
                        if (value.Equals(this._array[i]))
                        {
                            for (int j = i + 1; j < length; j++)
                            {
                                localArray[j - 1] = this._array[j];
                            }
                            this._array = localArray;
                            goto Label_00ED;
                        }
                        localArray[i] = this._array[i];
                    }
                    if (value.Equals(this._array[index]))
                    {
                        this._array = localArray;
                    }
                }
            Label_00ED:;
            }
        }

        public void RemoveAt(int index)
        {
            lock (this.SyncRoot)
            {
                int length = this._array.Length;
                T local = this._array[index];
                T[] destinationArray = new T[length - 1];
                Array.Copy(this._array, 0, destinationArray, 0, index);
                int num2 = (length - index) - 1;
                if (num2 > 0)
                {
                    Array.Copy(this._array, index + 1, destinationArray, index, num2);
                }
                this._array = destinationArray;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._array.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return this._array.Length;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
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
                return true;
            }
        }

        public T this[int index]
        {
            get
            {
                return this._array[index];
            }
            set
            {
                lock (this.SyncRoot)
                {
                    T local = this._array[index];
                    if (!((value != null) && value.Equals(local)))
                    {
                        T[] destinationArray = new T[this._array.Length];
                        Array.Copy(this._array, 0, destinationArray, 0, this._array.Length);
                        destinationArray[index] = value;
                        this._array = destinationArray;
                    }
                }
            }
        }

        public object SyncRoot
        {
            get
            {
                return CopyOnWriteArray<T>._objLock;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this._array[index];
            }
            set
            {
                lock (this.SyncRoot)
                {
                    T local = this._array[index];
                    if (!value.Equals(local))
                    {
                        T[] destinationArray = new T[this._array.Length];
                        Array.Copy(this._array, 0, destinationArray, 0, this._array.Length);
                        destinationArray[index] = (T) value;
                        this._array = destinationArray;
                    }
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            private T[] list;
            private int index;
            private T current;
            internal Enumerator(T[] list)
            {
                this.list = list;
                this.index = 0;
                this.current = default(T);
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (this.index < this.list.Length)
                {
                    this.current = this.list[this.index];
                    this.index++;
                    return true;
                }
                this.index = this.list.Length + 1;
                this.current = default(T);
                return false;
            }

            public T Current
            {
                get
                {
                    return this.current;
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    if ((this.index == 0) || (this.index == (this.list.Length + 1)))
                    {
                        throw new InvalidOperationException();
                    }
                    return this.Current;
                }
            }
            void IEnumerator.Reset()
            {
                this.index = 0;
                this.current = default(T);
            }
        }
    }
}

