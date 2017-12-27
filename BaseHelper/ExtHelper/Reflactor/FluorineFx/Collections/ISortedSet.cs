namespace FluorineFx.Collections
{
    using System;
    using System.Collections;

    public interface ISortedSet : IList, ICollection, IEnumerable
    {
        ISortedSet TailSet(object limit);
    }
}

