namespace FluorineFx.Collections
{
    using System;
    using System.Collections;

    [Serializable]
    internal class TreeSet : ArrayList, ISortedSet, IList, ICollection, IEnumerable
    {
        private readonly IComparer _comparer;

        public TreeSet()
        {
            this._comparer = System.Collections.Comparer.Default;
        }

        public TreeSet(ICollection c)
        {
            this._comparer = System.Collections.Comparer.Default;
            this.AddAll(c);
        }

        public TreeSet(IComparer c)
        {
            this._comparer = System.Collections.Comparer.Default;
            this._comparer = c;
        }

        public bool Add(object obj)
        {
            bool flag = this.AddWithoutSorting(obj);
            this.Sort(this._comparer);
            return flag;
        }

        public bool AddAll(ICollection c)
        {
            IEnumerator enumerator = new ArrayList(c).GetEnumerator();
            bool flag = false;
            while (enumerator.MoveNext())
            {
                if (this.AddWithoutSorting(enumerator.Current))
                {
                    flag = true;
                }
            }
            this.Sort(this._comparer);
            return flag;
        }

        private bool AddWithoutSorting(object obj)
        {
            bool flag;
            if (!(flag = this.Contains(obj)))
            {
                base.Add(obj);
            }
            return !flag;
        }

        public override bool Contains(object item)
        {
            IEnumerator enumerator = this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (this._comparer.Compare(enumerator.Current, item) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public object First()
        {
            return this[0];
        }

        public ISortedSet TailSet(object limit)
        {
            ISortedSet set = new TreeSet();
            int num = 0;
            while ((num < this.Count) && (this._comparer.Compare(this[num], limit) < 0))
            {
                num++;
            }
            while (num < this.Count)
            {
                set.Add(this[num]);
                num++;
            }
            return set;
        }

        public static TreeSet UnmodifiableTreeSet(ICollection collection)
        {
            ArrayList list = new ArrayList(collection);
            return new TreeSet(ArrayList.ReadOnly(list));
        }

        public IComparer Comparer
        {
            get
            {
                return this._comparer;
            }
        }
    }
}

