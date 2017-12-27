namespace FluorineFx.Context
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Web.SessionState;

    internal sealed class HttpSessionStateWrapper : ISessionState, ICollection, IEnumerable
    {
        private HttpSessionState _httpSessionState;

        private HttpSessionStateWrapper(HttpSessionState httpSessionState)
        {
            this._httpSessionState = httpSessionState;
        }

        public void Add(string name, object value)
        {
            this._httpSessionState.Add(name, value);
        }

        public void Clear()
        {
            this._httpSessionState.Clear();
        }

        public void CopyTo(Array array, int index)
        {
            this._httpSessionState.CopyTo(array, index);
        }

        internal static HttpSessionStateWrapper CreateSessionWrapper(HttpSessionState httpSessionState)
        {
            return new HttpSessionStateWrapper(httpSessionState);
        }

        public IEnumerator GetEnumerator()
        {
            return this._httpSessionState.GetEnumerator();
        }

        public void Remove(string name)
        {
            this._httpSessionState.Remove(name);
        }

        public void RemoveAll()
        {
            this._httpSessionState.RemoveAll();
        }

        public void RemoveAt(int index)
        {
            this._httpSessionState.RemoveAt(index);
        }

        public int Count
        {
            get
            {
                return this._httpSessionState.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return this._httpSessionState.IsSynchronized;
            }
        }

        public object this[string name]
        {
            get
            {
                return this._httpSessionState[name];
            }
            set
            {
                this._httpSessionState[name] = value;
            }
        }

        public object this[int index]
        {
            get
            {
                return this._httpSessionState[index];
            }
            set
            {
                this._httpSessionState[index] = value;
            }
        }

        public string SessionID
        {
            get
            {
                return this._httpSessionState.SessionID;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this._httpSessionState.SyncRoot;
            }
        }
    }
}

