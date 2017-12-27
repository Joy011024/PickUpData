namespace FluorineFx.Collections
{
    using System;
    using System.Collections;

    public sealed class SetOp
    {
        private SetOp()
        {
        }

        public static void Difference(IEnumerable set1, IEnumerable set2, IComparer comparer, IOutputIterator output)
        {
            IEnumerator enumerator = set1.GetEnumerator();
            IEnumerator enumerator2 = set2.GetEnumerator();
            bool flag = enumerator.MoveNext();
            bool flag2 = enumerator2.MoveNext();
            while (flag && flag2)
            {
                int num = comparer.Compare(enumerator.Current, enumerator2.Current);
                if (num < 0)
                {
                    output.Add(enumerator.Current);
                    flag = enumerator.MoveNext();
                }
                else if (num > 0)
                {
                    flag2 = enumerator2.MoveNext();
                }
                else
                {
                    flag = enumerator.MoveNext();
                    flag2 = enumerator2.MoveNext();
                }
            }
            while (flag)
            {
                output.Add(enumerator.Current);
                flag = enumerator.MoveNext();
            }
        }

        public static void Inersection(IEnumerable set1, IEnumerable set2, IComparer comparer, IOutputIterator output)
        {
            IEnumerator enumerator = set1.GetEnumerator();
            IEnumerator enumerator2 = set2.GetEnumerator();
            bool flag = enumerator.MoveNext();
            bool flag2 = enumerator2.MoveNext();
            while (flag && flag2)
            {
                int num = comparer.Compare(enumerator.Current, enumerator2.Current);
                if (num < 0)
                {
                    flag = enumerator.MoveNext();
                }
                else if (num > 0)
                {
                    flag2 = enumerator2.MoveNext();
                }
                else
                {
                    output.Add(enumerator.Current);
                    flag = enumerator.MoveNext();
                    flag2 = enumerator2.MoveNext();
                }
            }
        }

        public static void Merge(IEnumerable set1, IEnumerable set2, IComparer comparer, IOutputIterator output)
        {
            IEnumerator enumerator = set1.GetEnumerator();
            IEnumerator enumerator2 = set2.GetEnumerator();
            bool flag = enumerator.MoveNext();
            bool flag2 = enumerator2.MoveNext();
            while (flag && flag2)
            {
                if (comparer.Compare(enumerator.Current, enumerator2.Current) < 0)
                {
                    output.Add(enumerator.Current);
                    flag = enumerator.MoveNext();
                }
                else
                {
                    output.Add(enumerator2.Current);
                    flag2 = enumerator2.MoveNext();
                }
            }
            while (flag)
            {
                output.Add(enumerator.Current);
                flag = enumerator.MoveNext();
            }
            while (flag2)
            {
                output.Add(enumerator2.Current);
                flag2 = enumerator2.MoveNext();
            }
        }

        public static void SymmetricDifference(IEnumerable set1, IEnumerable set2, IComparer comparer, IOutputIterator output)
        {
            IEnumerator enumerator = set1.GetEnumerator();
            IEnumerator enumerator2 = set2.GetEnumerator();
            bool flag = enumerator.MoveNext();
            bool flag2 = enumerator2.MoveNext();
            while (flag && flag2)
            {
                int num = comparer.Compare(enumerator.Current, enumerator2.Current);
                if (num < 0)
                {
                    output.Add(enumerator.Current);
                    flag = enumerator.MoveNext();
                }
                else if (num > 0)
                {
                    output.Add(enumerator2.Current);
                    flag2 = enumerator2.MoveNext();
                }
                else
                {
                    flag = enumerator.MoveNext();
                    flag2 = enumerator2.MoveNext();
                }
            }
            while (flag)
            {
                output.Add(enumerator.Current);
                flag = enumerator.MoveNext();
            }
            while (flag2)
            {
                output.Add(enumerator2.Current);
                flag2 = enumerator2.MoveNext();
            }
        }

        public static void Union(IEnumerable set1, IEnumerable set2, IComparer comparer, IOutputIterator output)
        {
            IEnumerator enumerator = set1.GetEnumerator();
            IEnumerator enumerator2 = set2.GetEnumerator();
            bool flag = enumerator.MoveNext();
            bool flag2 = enumerator2.MoveNext();
            while (flag && flag2)
            {
                int num = comparer.Compare(enumerator.Current, enumerator2.Current);
                if (num < 0)
                {
                    output.Add(enumerator.Current);
                    flag = enumerator.MoveNext();
                }
                else if (num > 0)
                {
                    output.Add(enumerator2.Current);
                    flag2 = enumerator2.MoveNext();
                }
                else
                {
                    output.Add(enumerator.Current);
                    flag = enumerator.MoveNext();
                    flag2 = enumerator2.MoveNext();
                }
            }
            while (flag)
            {
                output.Add(enumerator.Current);
                flag = enumerator.MoveNext();
            }
            while (flag2)
            {
                output.Add(enumerator2.Current);
                flag2 = enumerator2.MoveNext();
            }
        }
    }
}

