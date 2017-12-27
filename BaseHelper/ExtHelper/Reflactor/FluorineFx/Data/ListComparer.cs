namespace FluorineFx.Data
{
    using System;
    using System.Collections;

    internal sealed class ListComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            IList list = x as IList;
            IList list2 = y as IList;
            if ((list != null) && (list2 != null))
            {
                if (list.Count != list2.Count)
                {
                    return -1;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    if (!((list[i] != null) ? list[i].Equals(list2[i]) : (list2[i] == null)))
                    {
                        return -1;
                    }
                }
            }
            return 0;
        }
    }
}

