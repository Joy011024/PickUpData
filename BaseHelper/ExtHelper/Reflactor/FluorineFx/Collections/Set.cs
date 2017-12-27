namespace FluorineFx.Collections
{
    using System;
    using System.Collections;

    public class Set : SetBase
    {
        public Set() : base(Comparer.Default, false)
        {
        }

        public Set(ICollection collection) : this()
        {
            foreach (object obj2 in collection)
            {
                base.Add(obj2);
            }
        }

        public Set(IComparer comparer) : base(comparer, false)
        {
        }

        public Set(IComparer comparer, ICollection collection) : this(comparer)
        {
            foreach (object obj2 in collection)
            {
                base.Add(obj2);
            }
        }

        public Set Difference(Set other)
        {
            return Difference(this, other);
        }

        public static Set Difference(Set a, Set b)
        {
            a.CheckComparer(b);
            Set collection = new Set(a.Comparer);
            SetOp.Difference(a, b, a.Comparer, new Inserter(collection));
            return collection;
        }

        public Set Intersection(Set other)
        {
            return Intersection(this, other);
        }

        public static Set Intersection(Set a, Set b)
        {
            a.CheckComparer(b);
            Set collection = new Set(a.Comparer);
            SetOp.Inersection(a, b, a.Comparer, new Inserter(collection));
            return collection;
        }

        public Set SymmetricDifference(Set other)
        {
            return SymmetricDifference(this, other);
        }

        public static Set SymmetricDifference(Set a, Set b)
        {
            a.CheckComparer(b);
            Set collection = new Set(a.Comparer);
            SetOp.SymmetricDifference(a, b, a.Comparer, new Inserter(collection));
            return collection;
        }

        public Set Union(Set other)
        {
            return Union(this, other);
        }

        public static Set Union(Set a, Set b)
        {
            a.CheckComparer(b);
            Set collection = new Set(a.Comparer);
            SetOp.Union(a, b, a.Comparer, new Inserter(collection));
            return collection;
        }
    }
}

