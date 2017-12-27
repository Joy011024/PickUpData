namespace FluorineFx.Util
{
    using System;
    using System.Collections;
    using System.Diagnostics;

    internal sealed class InvariantStringArray
    {
        private InvariantStringArray()
        {
        }

        public static int BinarySearch(string[] values, string sought)
        {
            Debug.Assert(values != null);
            return Array.BinarySearch(values, sought, InvariantComparer);
        }

        public static void Sort(string[] keys, Array items)
        {
            Debug.Assert(keys != null);
            Debug.Assert(items != null);
            Array.Sort(keys, items, InvariantComparer);
        }

        private static IComparer InvariantComparer
        {
            get
            {
                return Comparer.DefaultInvariant;
            }
        }
    }
}

