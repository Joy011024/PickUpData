namespace FluorineFx.Util
{
    using System;

    internal abstract class CompareUtils
    {
        protected CompareUtils()
        {
        }

        private static bool CoerceTypes(ref object left, ref object right)
        {
            if (NumberUtils.IsNumber(left) && NumberUtils.IsNumber(right))
            {
                NumberUtils.CoerceTypes(ref right, ref left);
                return true;
            }
            return false;
        }

        public static int Compare(object first, object second)
        {
            if (first == null)
            {
                return ((second == null) ? 0 : -1);
            }
            if (second == null)
            {
                return 1;
            }
            if (!first.GetType().Equals(second.GetType()) && !CoerceTypes(ref first, ref second))
            {
                throw new ArgumentException("Cannot compare instances of [" + first.GetType().FullName + "] and [" + second.GetType().FullName + "] because they cannot be coerced to the same type.");
            }
            if (!(first is IComparable))
            {
                throw new ArgumentException("Cannot compare instances of the type [" + first.GetType().FullName + "] because it doesn't implement IComparable");
            }
            return ((IComparable) first).CompareTo(second);
        }
    }
}

