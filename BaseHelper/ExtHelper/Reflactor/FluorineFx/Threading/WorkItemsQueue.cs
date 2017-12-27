namespace FluorineFx.Threading
{
    using System;
    using System.Collections;
    using System.Threading;

    internal sealed class WorkItemsQueue : IDisposable
    {
        private WaitEntry _headWaitEntry = new WaitEntry();
        private bool _isDisposed = false;
        private bool _isWorkItemsQueueActive = true;
        [ThreadStatic]
        private static WaitEntry _waitEntry;
        private int _waitersCount = 0;
        private Queue _workItems = new Queue();

        private void CheckDisposed()
        {
            if (this._isDisposed)
            {
                throw new ObjectDisposedException(base.GetType().ToString());
            }
        }

        private void Cleanup()
        {
            lock (this)
            {
                if (this._isWorkItemsQueueActive)
                {
                    this._isWorkItemsQueueActive = false;
                    foreach (WorkItem item in this._workItems)
                    {
                        item.DisposeState();
                    }
                    this._workItems.Clear();
                    while (this._waitersCount > 0)
                    {
                        this.PopWaiter().Timeout();
                    }
                }
            }
        }

        public WorkItem DequeueWorkItem(int millisecondsTimeout, WaitHandle cancelEvent)
        {
            WaitEntry newWaiterEntry = null;
            WorkItem workItem = null;
            WorkItemsQueue queue;
            lock ((queue = this))
            {
                this.CheckDisposed();
                if (this._workItems.Count > 0)
                {
                    return (this._workItems.Dequeue() as WorkItem);
                }
                newWaiterEntry = this.GetThreadWaitEntry();
                this.PushWaitEntry(newWaiterEntry);
            }
            WaitHandle[] waitHandles = new WaitHandle[] { newWaiterEntry.WaitHandle, cancelEvent };
            int num = WaitHandle.WaitAny(waitHandles, millisecondsTimeout, true);
            lock ((queue = this))
            {
                bool flag = 0 == num;
                if (!flag)
                {
                    bool flag2 = newWaiterEntry.Timeout();
                    if (flag2)
                    {
                        this.RemoveWaiter(newWaiterEntry, false);
                    }
                    flag = !flag2;
                }
                if (flag)
                {
                    workItem = newWaiterEntry.WorkItem;
                    if (workItem == null)
                    {
                        workItem = this._workItems.Dequeue() as WorkItem;
                    }
                }
            }
            return workItem;
        }

        public void Dispose()
        {
            if (!this._isDisposed)
            {
                this.Cleanup();
                this._isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        public bool EnqueueWorkItem(WorkItem workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException("workItem");
            }
            bool flag = true;
            lock (this)
            {
                this.CheckDisposed();
                if (!this._isWorkItemsQueueActive)
                {
                    return false;
                }
                while (this._waitersCount > 0)
                {
                    if (this.PopWaiter().Signal(workItem))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    this._workItems.Enqueue(workItem);
                }
            }
            return true;
        }

        ~WorkItemsQueue()
        {
            this.Cleanup();
        }

        private WaitEntry GetThreadWaitEntry()
        {
            if (null == _waitEntry)
            {
                _waitEntry = new WaitEntry();
            }
            _waitEntry.Reset();
            return _waitEntry;
        }

        private WaitEntry PopWaiter()
        {
            WaitEntry waitEntry = this._headWaitEntry._nextWaiterEntry;
            WaitEntry entry2 = waitEntry._nextWaiterEntry;
            this.RemoveWaiter(waitEntry, true);
            this._headWaitEntry._nextWaiterEntry = entry2;
            if (null != entry2)
            {
                entry2._prevWaiterEntry = this._headWaitEntry;
            }
            return waitEntry;
        }

        public void PushWaitEntry(WaitEntry newWaiterEntry)
        {
            this.RemoveWaiter(newWaiterEntry, false);
            if (null == this._headWaitEntry._nextWaiterEntry)
            {
                this._headWaitEntry._nextWaiterEntry = newWaiterEntry;
                newWaiterEntry._prevWaiterEntry = this._headWaitEntry;
            }
            else
            {
                WaitEntry entry = this._headWaitEntry._nextWaiterEntry;
                this._headWaitEntry._nextWaiterEntry = newWaiterEntry;
                newWaiterEntry._nextWaiterEntry = entry;
                newWaiterEntry._prevWaiterEntry = this._headWaitEntry;
                entry._prevWaiterEntry = newWaiterEntry;
            }
            this._waitersCount++;
        }

        private void RemoveWaiter(WaitEntry WaitEntry, bool popDecrement)
        {
            WorkItemsQueue.WaitEntry entry = WaitEntry._prevWaiterEntry;
            WorkItemsQueue.WaitEntry entry2 = WaitEntry._nextWaiterEntry;
            bool flag = popDecrement;
            WaitEntry._prevWaiterEntry = null;
            WaitEntry._nextWaiterEntry = null;
            if (null != entry)
            {
                entry._nextWaiterEntry = entry2;
                flag = true;
            }
            if (null != entry2)
            {
                entry2._prevWaiterEntry = entry;
                flag = true;
            }
            if (flag)
            {
                this._waitersCount--;
            }
        }

        public int Count
        {
            get
            {
                lock (this)
                {
                    this.CheckDisposed();
                    return this._workItems.Count;
                }
            }
        }

        public int WaitersCount
        {
            get
            {
                lock (this)
                {
                    this.CheckDisposed();
                    return this._waitersCount;
                }
            }
        }

        public sealed class WaitEntry : IDisposable
        {
            private bool _isDisposed = false;
            private bool _isSignaled = false;
            private bool _isTimedout = false;
            internal WorkItemsQueue.WaitEntry _nextWaiterEntry = null;
            internal WorkItemsQueue.WaitEntry _prevWaiterEntry = null;
            private AutoResetEvent _waitHandle = new AutoResetEvent(false);
            private FluorineFx.Threading.WorkItem _workItem = null;

            public WaitEntry()
            {
                this.Reset();
            }

            public void Close()
            {
                if (null != this._waitHandle)
                {
                    this._waitHandle.Close();
                    this._waitHandle = null;
                }
            }

            public void Dispose()
            {
                if (!this._isDisposed)
                {
                    this.Close();
                    this._isDisposed = true;
                }
            }

            ~WaitEntry()
            {
                this.Dispose();
            }

            public void Reset()
            {
                this._workItem = null;
                this._isTimedout = false;
                this._isSignaled = false;
                this._waitHandle.Reset();
            }

            public bool Signal(FluorineFx.Threading.WorkItem workItem)
            {
                lock (this)
                {
                    if (!this._isTimedout)
                    {
                        this._workItem = workItem;
                        this._isSignaled = true;
                        this._waitHandle.Set();
                        return true;
                    }
                }
                return false;
            }

            public bool Timeout()
            {
                lock (this)
                {
                    if (!this._isSignaled)
                    {
                        this._isTimedout = true;
                        return true;
                    }
                }
                return false;
            }

            public System.Threading.WaitHandle WaitHandle
            {
                get
                {
                    return this._waitHandle;
                }
            }

            public FluorineFx.Threading.WorkItem WorkItem
            {
                get
                {
                    lock (this)
                    {
                        return this._workItem;
                    }
                }
            }
        }
    }
}

