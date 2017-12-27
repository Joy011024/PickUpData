namespace FluorineFx.Threading
{
    using System;
    using System.Threading;

    public class ThreadPoolStartInfo
    {
        private int _idleTimeout;
        private int _maxWorkerThreads;
        private int _minWorkerThreads;
        private string _perfCounterInstanceName;
        private bool _startSuspended;
        private System.Threading.ThreadPriority _threadPriority;

        public ThreadPoolStartInfo()
        {
            this._idleTimeout = 0xea60;
            this._minWorkerThreads = 0;
            this._maxWorkerThreads = 0x3e8;
            this._threadPriority = System.Threading.ThreadPriority.Normal;
            this._startSuspended = false;
            this._perfCounterInstanceName = ThreadPoolEx.DefaultPerformanceCounterInstanceName;
        }

        public ThreadPoolStartInfo(ThreadPoolStartInfo threadPoolStartInfo)
        {
            this._idleTimeout = threadPoolStartInfo._idleTimeout;
            this._minWorkerThreads = threadPoolStartInfo._minWorkerThreads;
            this._maxWorkerThreads = threadPoolStartInfo._maxWorkerThreads;
            this._threadPriority = threadPoolStartInfo._threadPriority;
            this._perfCounterInstanceName = threadPoolStartInfo._perfCounterInstanceName;
            this._startSuspended = threadPoolStartInfo._startSuspended;
        }

        public int IdleTimeout
        {
            get
            {
                return this._idleTimeout;
            }
            set
            {
                this._idleTimeout = value;
            }
        }

        public int MaxWorkerThreads
        {
            get
            {
                return this._maxWorkerThreads;
            }
            set
            {
                this._maxWorkerThreads = value;
            }
        }

        public int MinWorkerThreads
        {
            get
            {
                return this._minWorkerThreads;
            }
            set
            {
                this._minWorkerThreads = value;
            }
        }

        public string PerformanceCounterInstanceName
        {
            get
            {
                return this._perfCounterInstanceName;
            }
            set
            {
                this._perfCounterInstanceName = value;
            }
        }

        public bool StartSuspended
        {
            get
            {
                return this._startSuspended;
            }
            set
            {
                this._startSuspended = value;
            }
        }

        public System.Threading.ThreadPriority ThreadPriority
        {
            get
            {
                return this._threadPriority;
            }
            set
            {
                this._threadPriority = value;
            }
        }
    }
}

