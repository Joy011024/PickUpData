namespace FluorineFx.Messaging.Api.Statistics
{
    using System;
    using System.Threading;

    internal class StatisticsCounter
    {
        private int _current;
        private int _max;
        private int _total;

        public void Decrement()
        {
            Interlocked.Decrement(ref this._current);
        }

        public void Increment()
        {
            Interlocked.Increment(ref this._total);
            Interlocked.Increment(ref this._current);
            this._max = Math.Max(this._max, this._current);
        }

        public int Current
        {
            get
            {
                return this._current;
            }
        }

        public int Max
        {
            get
            {
                return this._max;
            }
        }

        public int Total
        {
            get
            {
                return this._total;
            }
        }
    }
}

