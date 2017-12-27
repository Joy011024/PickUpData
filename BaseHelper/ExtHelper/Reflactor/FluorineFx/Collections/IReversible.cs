namespace FluorineFx.Collections
{
    using System.Collections;

    public interface IReversible : IEnumerable
    {
        IEnumerable Reversed { get; }
    }
}

