namespace FluorineFx.Exceptions
{
    using System;

    [Serializable]
    public class UnexpectedAMF : FluorineException
    {
        public UnexpectedAMF()
        {
        }

        public UnexpectedAMF(string message) : base(message)
        {
        }
    }
}

