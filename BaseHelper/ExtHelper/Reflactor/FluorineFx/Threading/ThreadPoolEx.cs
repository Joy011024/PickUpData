namespace FluorineFx.Threading
{
    using FluorineFx.Collections;
    using System;
    using System.Security;
    using System.Threading;

    public sealed class ThreadPoolEx : IDisposable
    {
        [ThreadStatic]
        private static WorkItem _currentWorkItem;
        private int _currentWorkItemsCount;
        private int _inUseWorkerThreads;
        private bool _isDisposed;
        private ManualResetEvent _isIdleWaitHandle;
        private string _name;
        private bool _shutdown;
        private ManualResetEvent _shuttingDownEvent;
        private int _threadCounter;
        [ThreadStatic]
        private static ThreadPoolEx _threadPool;
        private ThreadPoolStartInfo _threadPoolStartInfo;
        private SynchronizedHashtable _workerThreads;
        private long _workItemsProcessed;
        private WorkItemsQueue _workItemsQueue;
        public const int DefaultIdleTimeout = 0xea60;
        public const int DefaultMaxWorkerThreads = 0x3e8;
        public const int DefaultMinWorkerThreads = 0;
        public static readonly string DefaultPerformanceCounterInstanceName = null;
        public const bool DefaultStartSuspended = false;
        public const ThreadPriority DefaultThreadPriority = ThreadPriority.Normal;
        private static ThreadPoolEx GlobalThreadPool;

        public ThreadPoolEx()
        {
            this._threadPoolStartInfo = new ThreadPoolStartInfo();
            this._workItemsQueue = new WorkItemsQueue();
            this._workerThreads = new SynchronizedHashtable();
            this._inUseWorkerThreads = 0;
            this._isIdleWaitHandle = new ManualResetEvent(true);
            this._shuttingDownEvent = new ManualResetEvent(false);
            this._shutdown = false;
            this._threadCounter = 0;
            this._isDisposed = false;
            this._workItemsProcessed = 0L;
            this._currentWorkItemsCount = 0;
            this._name = "ThreadPoolEx";
            this.Initialize();
        }

        public ThreadPoolEx(int idleTimeout)
        {
            this._threadPoolStartInfo = new ThreadPoolStartInfo();
            this._workItemsQueue = new WorkItemsQueue();
            this._workerThreads = new SynchronizedHashtable();
            this._inUseWorkerThreads = 0;
            this._isIdleWaitHandle = new ManualResetEvent(true);
            this._shuttingDownEvent = new ManualResetEvent(false);
            this._shutdown = false;
            this._threadCounter = 0;
            this._isDisposed = false;
            this._workItemsProcessed = 0L;
            this._currentWorkItemsCount = 0;
            this._name = "ThreadPoolEx";
            this._threadPoolStartInfo.IdleTimeout = idleTimeout;
            this.Initialize();
        }

        public ThreadPoolEx(int idleTimeout, int maxWorkerThreads)
        {
            this._threadPoolStartInfo = new ThreadPoolStartInfo();
            this._workItemsQueue = new WorkItemsQueue();
            this._workerThreads = new SynchronizedHashtable();
            this._inUseWorkerThreads = 0;
            this._isIdleWaitHandle = new ManualResetEvent(true);
            this._shuttingDownEvent = new ManualResetEvent(false);
            this._shutdown = false;
            this._threadCounter = 0;
            this._isDisposed = false;
            this._workItemsProcessed = 0L;
            this._currentWorkItemsCount = 0;
            this._name = "ThreadPoolEx";
            this._threadPoolStartInfo.IdleTimeout = idleTimeout;
            this._threadPoolStartInfo.MaxWorkerThreads = maxWorkerThreads;
            this.Initialize();
        }

        public ThreadPoolEx(int idleTimeout, int maxWorkerThreads, int minWorkerThreads)
        {
            this._threadPoolStartInfo = new ThreadPoolStartInfo();
            this._workItemsQueue = new WorkItemsQueue();
            this._workerThreads = new SynchronizedHashtable();
            this._inUseWorkerThreads = 0;
            this._isIdleWaitHandle = new ManualResetEvent(true);
            this._shuttingDownEvent = new ManualResetEvent(false);
            this._shutdown = false;
            this._threadCounter = 0;
            this._isDisposed = false;
            this._workItemsProcessed = 0L;
            this._currentWorkItemsCount = 0;
            this._name = "ThreadPoolEx";
            this._threadPoolStartInfo.IdleTimeout = idleTimeout;
            this._threadPoolStartInfo.MaxWorkerThreads = maxWorkerThreads;
            this._threadPoolStartInfo.MinWorkerThreads = minWorkerThreads;
            this.Initialize();
        }

        private void CheckDisposed()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException(base.GetType().ToString());
            }
        }

        private void DecrementWorkItemsCount()
        {
            this._workItemsProcessed += 1L;
            if (Interlocked.Decrement(ref this._currentWorkItemsCount) == 0)
            {
                this._isIdleWaitHandle.Set();
            }
        }

        private WorkItem Dequeue()
        {
            return this._workItemsQueue.DequeueWorkItem(this._threadPoolStartInfo.IdleTimeout, this._shuttingDownEvent);
        }

        public void Dispose()
        {
            if (!this._isDisposed)
            {
                if (!this._shutdown)
                {
                    this.Shutdown();
                }
                if (null != this._shuttingDownEvent)
                {
                    this._shuttingDownEvent.Close();
                    this._shuttingDownEvent = null;
                }
                this._workerThreads.Clear();
                this._isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        private void Enqueue(WorkItem workItem)
        {
            this.Enqueue(workItem, true);
        }

        internal void Enqueue(WorkItem workItem, bool incrementWorkItems)
        {
            if (incrementWorkItems)
            {
                this.IncrementWorkItemsCount();
            }
            this._workItemsQueue.EnqueueWorkItem(workItem);
            workItem.SetQueueTime();
            if ((this.InUseThreads + this.WaitingCallbacks) > this._workerThreads.Count)
            {
                this.StartThreads(1);
            }
        }

        private void ExecuteWorkItem(WorkItem workItem)
        {
            try
            {
                try
                {
                    workItem.Execute();
                }
                catch
                {
                    throw;
                }
            }
            finally
            {
            }
        }

        ~ThreadPoolEx()
        {
            this.Dispose();
        }

        private void IncrementWorkItemsCount()
        {
            if (Interlocked.Increment(ref this._currentWorkItemsCount) == 1)
            {
                this._isIdleWaitHandle.Reset();
            }
        }

        private void Initialize()
        {
            this.ValidateStartInfo();
            this.StartOptimalNumberOfThreads();
        }

        internal static void LoopSleep(ref int loopIndex)
        {
            int num;
            if ((Environment.ProcessorCount == 1) || ((num % (Environment.ProcessorCount * 50)) == 0))
            {
                Thread.Sleep(5);
            }
            else
            {
                Thread.SpinWait(20);
            }
        }

        private void ProcessQueuedItems()
        {
            _threadPool = this;
            try
            {
                bool flag = false;
                while (!this._shutdown)
                {
                    this._workerThreads[Thread.CurrentThread] = DateTime.Now;
                    WorkItem workItem = this.Dequeue();
                    this._workerThreads[Thread.CurrentThread] = DateTime.Now;
                    if ((workItem == null) && (this._workerThreads.Count > this._threadPoolStartInfo.MinWorkerThreads))
                    {
                        lock (this._workerThreads.SyncRoot)
                        {
                            if (this._workerThreads.Count > this._threadPoolStartInfo.MinWorkerThreads)
                            {
                                if (this._workerThreads.Contains(Thread.CurrentThread))
                                {
                                    this._workerThreads.Remove(Thread.CurrentThread);
                                }
                                return;
                            }
                        }
                    }
                    if (workItem != null)
                    {
                        int num;
                        try
                        {
                            flag = false;
                            if (!workItem.StartingWorkItem())
                            {
                                continue;
                            }
                            num = Interlocked.Increment(ref this._inUseWorkerThreads);
                            flag = true;
                            _currentWorkItem = workItem;
                            this.ExecuteWorkItem(workItem);
                        }
                        catch (Exception)
                        {
                        }
                        finally
                        {
                            if (workItem != null)
                            {
                                workItem.DisposeState();
                            }
                            _currentWorkItem = null;
                            if (flag)
                            {
                                num = Interlocked.Decrement(ref this._inUseWorkerThreads);
                            }
                            this.DecrementWorkItemsCount();
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (this._workerThreads.Contains(Thread.CurrentThread))
                {
                    this._workerThreads.Remove(Thread.CurrentThread);
                }
            }
        }

        public void QueueUserWorkItem(WaitCallback callback, object state)
        {
            this.CheckDisposed();
            WorkItem workItem = new WorkItem(callback, state);
            this.Enqueue(workItem);
        }

        public void Shutdown()
        {
            this.Shutdown(true, 0);
        }

        public void Shutdown(bool forceAbort, int millisecondsTimeout)
        {
            this.CheckDisposed();
            Thread[] array = null;
            lock (this._workerThreads.SyncRoot)
            {
                this._workItemsQueue.Dispose();
                this._shutdown = true;
                this._shuttingDownEvent.Set();
                array = new Thread[this._workerThreads.Count];
                this._workerThreads.Keys.CopyTo(array, 0);
            }
            int num = millisecondsTimeout;
            DateTime now = DateTime.Now;
            bool flag = -1 == millisecondsTimeout;
            bool flag2 = false;
            foreach (Thread thread in array)
            {
                if (!(flag || (num >= 0)))
                {
                    flag2 = true;
                    break;
                }
                if (!thread.Join(num))
                {
                    flag2 = true;
                    break;
                }
                if (!flag)
                {
                    TimeSpan span = (TimeSpan) (DateTime.Now - now);
                    num = millisecondsTimeout - ((int) span.TotalMilliseconds);
                }
            }
            if (flag2 && forceAbort)
            {
                foreach (Thread thread in array)
                {
                    if ((thread != null) && thread.IsAlive)
                    {
                        try
                        {
                            thread.Abort("Shutdown");
                        }
                        catch (SecurityException)
                        {
                        }
                        catch (ThreadStateException)
                        {
                        }
                    }
                }
            }
        }

        public void Shutdown(bool forceAbort, TimeSpan timeout)
        {
            this.Shutdown(forceAbort, (int) timeout.TotalMilliseconds);
        }

        private void StartOptimalNumberOfThreads()
        {
            int threadsCount = Math.Min(Math.Max(this._workItemsQueue.Count, this._threadPoolStartInfo.MinWorkerThreads), this._threadPoolStartInfo.MaxWorkerThreads);
            this.StartThreads(threadsCount);
        }

        private void StartThreads(int threadsCount)
        {
            if (!this._threadPoolStartInfo.StartSuspended)
            {
                lock (this._workerThreads.SyncRoot)
                {
                    if (!this._shutdown)
                    {
                        for (int i = 0; i < threadsCount; i++)
                        {
                            if (this._workerThreads.Count >= this._threadPoolStartInfo.MaxWorkerThreads)
                            {
                                return;
                            }
                            Thread thread = new Thread(new ThreadStart(this.ProcessQueuedItems)) {
                                Name = this._name + " Thread #" + this._threadCounter,
                                IsBackground = true,
                                Priority = ThreadPriority.Normal
                            };
                            thread.Start();
                            this._threadCounter++;
                            this._workerThreads[thread] = DateTime.Now;
                        }
                    }
                }
            }
        }

        private void ValidateStartInfo()
        {
            if (this._threadPoolStartInfo.MinWorkerThreads < 0)
            {
                throw new ArgumentOutOfRangeException("MinWorkerThreads", "MinWorkerThreads cannot be negative");
            }
            if (this._threadPoolStartInfo.MaxWorkerThreads <= 0)
            {
                throw new ArgumentOutOfRangeException("MaxWorkerThreads", "MaxWorkerThreads must be greater than zero");
            }
            if (this._threadPoolStartInfo.MinWorkerThreads > this._threadPoolStartInfo.MaxWorkerThreads)
            {
                throw new ArgumentOutOfRangeException("MinWorkerThreads, maxWorkerThreads", "MaxWorkerThreads must be greater or equal to MinWorkerThreads");
            }
        }

        public int AvailableThreads
        {
            get
            {
                return (this._threadPoolStartInfo.MaxWorkerThreads - this._inUseWorkerThreads);
            }
        }

        public static ThreadPoolEx Global
        {
            get
            {
                if (GlobalThreadPool == null)
                {
                    lock (typeof(ThreadPoolEx))
                    {
                        if (GlobalThreadPool == null)
                        {
                            GlobalThreadPool = new ThreadPoolEx();
                        }
                    }
                }
                return GlobalThreadPool;
            }
        }

        public int InUseThreads
        {
            get
            {
                this.CheckDisposed();
                return this._inUseWorkerThreads;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public int WaitingCallbacks
        {
            get
            {
                this.CheckDisposed();
                return this._workItemsQueue.Count;
            }
        }
    }
}

