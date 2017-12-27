namespace FluorineFx.Util
{
    using System;
    using System.Threading;

    internal class AsyncResultNoResult : IAsyncResult
    {
        private readonly AsyncCallback _asyncCallback;
        private readonly object _asyncState;
        private ManualResetEvent _asyncWaitHandle;
        private int _completedState = 0;
        private Exception _exception;
        private const int StateCompletedAsynchronously = 2;
        private const int StateCompletedSynchronously = 1;
        private const int StatePending = 0;

        public AsyncResultNoResult(AsyncCallback asyncCallback, object state)
        {
            this._asyncCallback = asyncCallback;
            this._asyncState = state;
        }

        public void EndInvoke()
        {
            if (!this.IsCompleted)
            {
                this.AsyncWaitHandle.WaitOne();
                this.AsyncWaitHandle.Close();
                this._asyncWaitHandle = null;
            }
            if (this._exception != null)
            {
                throw this._exception;
            }
        }

        public void SetAsCompleted(Exception exception, bool completedSynchronously)
        {
            this._exception = exception;
            if (Interlocked.Exchange(ref this._completedState, completedSynchronously ? 1 : 2) != 0)
            {
                throw new InvalidOperationException("You can set a result only once");
            }
            if (this._asyncWaitHandle != null)
            {
                this._asyncWaitHandle.Set();
            }
            if (this._asyncCallback != null)
            {
                this._asyncCallback(this);
            }
        }

        public object AsyncState
        {
            get
            {
                return this._asyncState;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (this._asyncWaitHandle == null)
                {
                    bool isCompleted = this.IsCompleted;
                    ManualResetEvent event2 = new ManualResetEvent(isCompleted);
                    if (Interlocked.CompareExchange<ManualResetEvent>(ref this._asyncWaitHandle, event2, null) != null)
                    {
                        event2.Close();
                    }
                    else if (!(isCompleted || !this.IsCompleted))
                    {
                        this._asyncWaitHandle.Set();
                    }
                }
                return this._asyncWaitHandle;
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return (Thread.VolatileRead(ref this._completedState) == 1);
            }
        }

        public bool IsCompleted
        {
            get
            {
                return (Thread.VolatileRead(ref this._completedState) != 0);
            }
        }
    }
}

