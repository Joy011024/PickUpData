namespace FluorineFx.Context
{
    using System;
    using System.Collections;
    using System.Reflection;

    internal class RtmpApplicationState : IApplicationState
    {
        private static SortedList _applicationState;
        private static RtmpApplicationState _instance;
        protected static object _objLock = new object();

        private RtmpApplicationState()
        {
        }

        public void Add(string name, object value)
        {
            lock (_objLock)
            {
                this.GetApplicationStateData().Add(name, value);
            }
        }

        private SortedList GetApplicationStateData()
        {
            if (_applicationState == null)
            {
                lock (_objLock)
                {
                    if (_applicationState == null)
                    {
                        _applicationState = new SortedList();
                    }
                }
            }
            return _applicationState;
        }

        public void Remove(string key)
        {
            lock (_objLock)
            {
                this.GetApplicationStateData().Remove(key);
            }
        }

        public void RemoveAt(int index)
        {
            lock (_objLock)
            {
                this.GetApplicationStateData().RemoveAt(index);
            }
        }

        public static RtmpApplicationState Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new RtmpApplicationState();
                        }
                    }
                }
                return _instance;
            }
        }

        public object this[int index]
        {
            get
            {
                lock (_objLock)
                {
                    return this.GetApplicationStateData().GetByIndex(index);
                }
            }
        }

        public object this[string name]
        {
            get
            {
                lock (_objLock)
                {
                    return this.GetApplicationStateData()[name];
                }
            }
            set
            {
                lock (_objLock)
                {
                    this.GetApplicationStateData()[name] = value;
                }
            }
        }
    }
}

