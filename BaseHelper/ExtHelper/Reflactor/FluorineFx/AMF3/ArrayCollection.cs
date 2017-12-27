namespace FluorineFx.AMF3
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    [TypeConverter(typeof(ArrayCollectionConverter)), CLSCompliant(false)]
    public class ArrayCollection : IExternalizable, IList, ICollection, IEnumerable
    {
        private IList _list;

        public ArrayCollection()
        {
            this._list = new List<object>();
        }

        public ArrayCollection(IList list)
        {
            this._list = list;
        }

        public int Add(object value)
        {
            return this._list.Add(value);
        }

        public void Clear()
        {
            this._list.Clear();
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
            this._list.Insert(index, value);
        }

        public void ReadExternal(IDataInput input)
        {
            this._list = input.ReadObject() as IList;
        }

        public void Remove(object value)
        {
            this._list.Remove(value);
        }

        public void RemoveAt(int index)
        {
            this._list.RemoveAt(index);
        }

        public object[] ToArray()
        {
            if (this._list != null)
            {
                if (this._list is ArrayList)
                {
                    return ((ArrayList) this._list).ToArray();
                }
                if (this._list is List<object>)
                {
                    return ((List<object>) this._list).ToArray();
                }
                object[] objArray = new object[this._list.Count];
                for (int i = 0; i < this._list.Count; i++)
                {
                    objArray[i] = this._list[i];
                }
                return objArray;
            }
            return null;
        }

        public void WriteExternal(IDataOutput output)
        {
            output.WriteObject(this.ToArray());
        }

        public int Count
        {
            get
            {
                return ((this._list == null) ? 0 : this._list.Count);
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return this._list.IsFixedSize;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this._list.IsReadOnly;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return this._list.IsSynchronized;
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
                this._list[index] = value;
            }
        }

        public IList List
        {
            get
            {
                return this._list;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this._list.SyncRoot;
            }
        }
    }
}

