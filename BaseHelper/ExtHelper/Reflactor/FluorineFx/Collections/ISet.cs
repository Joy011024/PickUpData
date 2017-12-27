namespace FluorineFx.Collections
{
    using System.Collections;

    public interface ISet : IModifiableCollection, ICollection, IReversible, IEnumerable
    {
        IComparer Comparer { get; }
    }
}

