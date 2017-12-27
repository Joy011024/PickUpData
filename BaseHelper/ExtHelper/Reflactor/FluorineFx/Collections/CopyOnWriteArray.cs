namespace FluorineFx.Collections
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class CopyOnWriteArray : IList, ICollection, IEnumerable
    {
        private object[] _array;
        private static object _objLock = new object();

        public CopyOnWriteArray()
        {
            this._array = new object[0];
        }

        public CopyOnWriteArray(IList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            this._array = new object[list.Count];
            int num = 0;
            foreach (object obj2 in list)
            {
                this._array[num++] = obj2;
            }
        }

        public int Add(object value)
        {
            lock (this.SyncRoot)
            {
                int length = this._array.Length;
                object[] destinationArray = new object[length + 1];
                Array.Copy(this._array, 0, destinationArray, 0, length);
                destinationArray[length] = value;
                this._array = destinationArray;
                return length;
            }
        }

        public void Clear()
        {
            lock (this.SyncRoot)
            {
                this._array = new object[0];
            }
        }

        public bool Contains(object value)
        {
            return (this.IndexOf(value) > -1);
        }

        private void Copy(object[] src, int offset, int count)
        {
            lock (this.SyncRoot)
            {
                this._array = new object[count];
                Array.Copy(src, offset, this._array, 0, count);
            }
        }

        public void CopyTo(Array array, int index)
        {
            Array.Copy(this._array, 0, array, index, array.Length - index);
        }

        public IEnumerator GetEnumerator()
        {
            return this._array.GetEnumerator();
        }

        public int IndexOf(object value)
        {
            object[] elementData = this._array;
            int length = elementData.Length;
            return IndexOf(value, elementData, length);
        }

        private static int IndexOf(object elem, object[] elementData, int length)
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

        public void Insert(int index, object value)
        {
            lock (this.SyncRoot)
            {
                int length = this._array.Length;
                object[] destinationArray = new object[length + 1];
                Array.Copy(this._array, 0, destinationArray, 0, index);
                destinationArray[index] = value;
                Array.Copy(this._array, index, destinationArray, index + 1, length - index);
                this._array = destinationArray;
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
                    object[] objArray = new object[index];
                    for (int i = 0; i < index; i++)
                    {
                        if ((value == this._array[i]) || ((value != null) && value.Equals(this._array[i])))
                        {
                            for (int j = i + 1; j < length; j++)
                            {
                                objArray[j - 1] = this._array[j];
                            }
                            this._array = objArray;
                            goto Label_00F3;
                        }
                        objArray[i] = this._array[i];
                    }
                    if ((value == this._array[index]) || ((value != null) && value.Equals(this._array[index])))
                    {
                        this._array = objArray;
                    }
                }
            Label_00F3:;
            }
        }

        public void RemoveAt(int index)
        {
            lock (this.SyncRoot)
            {
                int length = this._array.Length;
                object obj2 = this._array[index];
                object[] destinationArray = new object[length - 1];
                Array.Copy(this._array, 0, destinationArray, 0, index);
                int num2 = (length - index) - 1;
                if (num2 > 0)
                {
                    Array.Copy(this._array, index + 1, destinationArray, index, num2);
                }
                this._array = destinationArray;
            }
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

        public object this[int index]
        {
            get
            {
                return this._array[index];
            }
            set
            {
                lock (this.SyncRoot)
                {
                    object obj2 = this._array[index];
                    if ((obj2 != value) && ((value == null) || !value.Equals(obj2)))
                    {
                        object[] destinationArray = new object[this._array.Length];
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
                return _objLock;
            }
        }
    }
}

