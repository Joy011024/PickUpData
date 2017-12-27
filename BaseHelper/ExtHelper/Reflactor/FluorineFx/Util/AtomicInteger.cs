namespace FluorineFx.Util
{
    using System;
    using System.Threading;

    internal class AtomicInteger
    {
        private int _counter;

        public AtomicInteger() : this(0)
        {
        }

        public AtomicInteger(int initialValue)
        {
            this._counter = initialValue;
        }

        public int Decrement()
        {
            return Interlocked.Decrement(ref this._counter);
        }

        public int Decrement(int delta)
        {
            return Interlocked.Add(ref this._counter, -delta);
        }

        public int Increment()
        {
            return Interlocked.Increment(ref this._counter);
        }

        public int Increment(int delta)
        {
            return Interlocked.Add(ref this._counter, delta);
        }

        public int PostDecrement()
        {
            return (Interlocked.Decrement(ref this._counter) + 1);
        }

        public int PostDecrement(int delta)
        {
            return (Interlocked.Add(ref this._counter, -delta) + delta);
        }

        public int PostIncrement()
        {
            return (Interlocked.Increment(ref this._counter) - 1);
        }

        public int PostIncrement(int delta)
        {
            return (Interlocked.Add(ref this._counter, delta) - delta);
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public int Value
        {
            get
            {
                return this._counter;
            }
            set
            {
                Interlocked.Exchange(ref this._counter, value);
            }
        }
    }
}

