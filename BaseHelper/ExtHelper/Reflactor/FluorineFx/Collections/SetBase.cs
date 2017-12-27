namespace FluorineFx.Collections
{
    using System;
    using System.Collections;

    public abstract class SetBase : ISet, IModifiableCollection, ICollection, IReversible, IEnumerable, IComparable
    {
        private bool _allowDuplicates;
        private IComparer _comparer;
        private int _count = 0;
        private RbTree _tree;

        public SetBase(IComparer comparer, bool allowDuplicates)
        {
            this._comparer = comparer;
            this._allowDuplicates = allowDuplicates;
            this._tree = new RbTree(this._comparer);
        }

        public bool Add(object key)
        {
            if (key == null)
            {
                return false;
            }
            RbTree.InsertResult result = this._tree.Insert(key, this._allowDuplicates, true);
            if (result.NewNode)
            {
                this._count++;
            }
            return result.NewNode;
        }

        public bool AddIfNotContains(object key)
        {
            if (key == null)
            {
                return false;
            }
            RbTree.InsertResult result = this._tree.Insert(key, false, false);
            if (result.NewNode)
            {
                this._count++;
            }
            return result.NewNode;
        }

        protected void CheckComparer(ISet other)
        {
            if (this.Comparer.GetType() != other.Comparer.GetType())
            {
                throw new ArgumentException("Sets have incompatible comparer objects", "other");
            }
        }

        public void Clear()
        {
            this._tree = new RbTree(this._comparer);
            this._count = 0;
        }

        public int CompareTo(object obj)
        {
            return CollectionComparer.Default.Compare(this, obj);
        }

        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (array.Rank != 1)
            {
                throw new ArgumentException("Cannot copy to multidimensional array", "array");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", index, "index cannot be negative");
            }
            if (index >= array.Length)
            {
                throw new ArgumentOutOfRangeException("index", index, string.Format("Passed array of length {0}, index cannot exceed {1}", array.Length, array.Length - 1));
            }
            int count = this.Count;
            if ((array.Length - index) < count)
            {
                throw new ArgumentOutOfRangeException("index", index, string.Format("Not enough room in the array to copy the collection. Array length {0}, start index {1}, items in collection {2}", array.Length, index, count));
            }
            int num2 = index;
            foreach (object obj2 in this)
            {
                array.SetValue(obj2, num2);
                num2++;
            }
        }

        public override bool Equals(object obj)
        {
            return (CollectionComparer.Default.Compare(this, obj) == 0);
        }

        public object Find(object key)
        {
            if (key != null)
            {
                RbTreeNode node = this._tree.LowerBound(key);
                if (node.IsNull)
                {
                    return null;
                }
                if (this.Comparer.Compare(node.Value, key) == 0)
                {
                    return node.Value;
                }
            }
            return null;
        }

        public ICollection FindAll(object key)
        {
            ArrayList list = new ArrayList();
            if (key != null)
            {
                RbTreeNode node = this._tree.LowerBound(key);
                RbTreeNode node2 = this._tree.UpperBound(key);
                if (node == node2)
                {
                    return list;
                }
                for (RbTreeNode node3 = node; node3 != node2; node3 = this._tree.Next(node3))
                {
                    list.Add(node3.Value);
                }
            }
            return list;
        }

        public IEnumerator GetEnumerator()
        {
            return new SetEnumerator(this._tree);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int Remove(object key)
        {
            if (key == null)
            {
                return 0;
            }
            int num = this._tree.Erase(key);
            this._count -= num;
            return num;
        }

        public IComparer Comparer
        {
            get
            {
                return this._comparer;
            }
        }

        public int Count
        {
            get
            {
                return this._count;
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

        public IEnumerable Reversed
        {
            get
            {
                return new ReversedTree(this._tree);
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }
    }
}

