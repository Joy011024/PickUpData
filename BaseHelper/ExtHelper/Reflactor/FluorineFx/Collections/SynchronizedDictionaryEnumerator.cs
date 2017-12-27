namespace FluorineFx.Collections
{
    using System;
    using System.Collections;

    internal class SynchronizedDictionaryEnumerator : SynchronizedEnumerator, IDictionaryEnumerator, IEnumerator
    {
        public SynchronizedDictionaryEnumerator(object syncRoot, IDictionaryEnumerator enumerator) : base(syncRoot, enumerator)
        {
        }

        public DictionaryEntry Entry
        {
            get
            {
                lock (base._syncRoot)
                {
                    return this.Enumerator.Entry;
                }
            }
        }

        protected IDictionaryEnumerator Enumerator
        {
            get
            {
                return (IDictionaryEnumerator) base._enumerator;
            }
        }

        public object Key
        {
            get
            {
                lock (base._syncRoot)
                {
                    return this.Enumerator.Key;
                }
            }
        }

        public object Value
        {
            get
            {
                lock (base._syncRoot)
                {
                    return this.Enumerator.Value;
                }
            }
        }
    }
}

