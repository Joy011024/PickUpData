namespace FluorineFx.Data
{
    using System;
    using System.Collections;

    internal sealed class ListHashCodeProvider : IEqualityComparer
    {
        public bool Equals(object x, object y)
        {
            IList list = x as IList;
            IList list2 = y as IList;
            if ((list != null) && (list2 != null))
            {
                if (list.Count != list2.Count)
                {
                    return false;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    if (!((list[i] != null) ? list[i].Equals(list2[i]) : (list2[i] == null)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static int GenerateHashCode(IList list)
        {
            if (list != null)
            {
                int num = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] != null)
                    {
                        num ^= list[i].GetHashCode();
                    }
                    else
                    {
                        num = num;
                    }
                }
                return num;
            }
            return 0;
        }

        public int GetHashCode(object obj)
        {
            IList list = obj as IList;
            return GenerateHashCode(list);
        }
    }
}

