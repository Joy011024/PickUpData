namespace FluorineFx.Messaging.Api
{
    using System;
    using System.Collections;

    public sealed class BasicScopeEnumerator : IEnumerator
    {
        private IBasicScope _currentElement;
        private IList _enumerable;
        private int _index = -1;

        internal BasicScopeEnumerator(IList enumerable)
        {
            this._enumerable = enumerable;
        }

        public bool MoveNext()
        {
            if (this._index < (this._enumerable.Count - 1))
            {
                this._index++;
                this._currentElement = this._enumerable[this._index] as IBasicScope;
                return true;
            }
            this._index = this._enumerable.Count;
            return false;
        }

        public void Reset()
        {
            this._currentElement = null;
            this._index = -1;
        }

        public object Current
        {
            get
            {
                if (this._index == -1)
                {
                    throw new InvalidOperationException("Enum not started.");
                }
                if (this._index >= this._enumerable.Count)
                {
                    throw new InvalidOperationException("Enumeration ended.");
                }
                return this._currentElement;
            }
        }
    }
}

