namespace FluorineFx.Collections
{
    using System;
    using System.Collections;

    internal class SynchronizedEnumerator : IEnumerator
    {
        protected IEnumerator _enumerator;
        protected object _syncRoot;

        public SynchronizedEnumerator(object syncRoot, IEnumerator enumerator)
        {
            this._syncRoot = syncRoot;
            this._enumerator = enumerator;
        }

        public bool MoveNext()
        {
            lock (this._syncRoot)
            {
                return this._enumerator.MoveNext();
            }
        }

        public void Reset()
        {
            lock (this._syncRoot)
            {
                this._enumerator.Reset();
            }
        }

        public object Current
        {
            get
            {
                lock (this._syncRoot)
                {
                    return this._enumerator.Current;
                }
            }
        }
    }
}

