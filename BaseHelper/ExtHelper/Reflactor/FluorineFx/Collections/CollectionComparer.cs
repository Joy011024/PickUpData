namespace FluorineFx.Collections
{
    using System;
    using System.Collections;
    using System.Diagnostics;

    public class CollectionComparer : IComparer
    {
        private IComparer _comparer;

        public CollectionComparer() : this(Comparer.Default)
        {
        }

        public CollectionComparer(IComparer comparer)
        {
            this._comparer = comparer;
        }

        public int Compare(object x, object y)
        {
            IEnumerable enumerable = x as IEnumerable;
            IEnumerable enumerable2 = y as IEnumerable;
            bool flag = enumerable != null;
            bool flag2 = enumerable2 != null;
            int num = this.CompareBool(flag, flag2);
            if (num != 0)
            {
                return num;
            }
            Debug.Assert(flag == flag2);
            if (!flag)
            {
                return this._comparer.Compare(x, y);
            }
            Debug.Assert(flag);
            Debug.Assert(flag2);
            IEnumerator enumerator = enumerable.GetEnumerator();
            IEnumerator enumerator2 = enumerable2.GetEnumerator();
            bool flag3 = enumerator.MoveNext();
            bool flag4 = enumerator2.MoveNext();
            while (flag3 && flag4)
            {
                num = this._comparer.Compare(enumerator.Current, enumerator2.Current);
                if (num != 0)
                {
                    return num;
                }
                flag3 = enumerator.MoveNext();
                flag4 = enumerator2.MoveNext();
            }
            return this.CompareBool(flag3, flag4);
        }

        private int CompareBool(bool bool1, bool bool2)
        {
            if (bool1)
            {
                return (bool2 ? 0 : 1);
            }
            return (bool2 ? -1 : 0);
        }

        public static CollectionComparer Default
        {
            get
            {
                return new CollectionComparer();
            }
        }
    }
}

