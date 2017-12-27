namespace FluorineFx.Threading
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    internal sealed class WorkItem
    {
        private DateTime _beginProcessTime;
        private WaitCallback _callback;
        private DateTime _endProcessTime;
        private Exception _exception;
        private DateTime _queuedTime;
        private object _state;
        private WorkItemState _workItemState;

        public WorkItem(WaitCallback callback, object state)
        {
            this._callback = callback;
            this._state = state;
            this._workItemState = WorkItemState.InQueue;
        }

        public void DisposeState()
        {
        }

        public void Execute()
        {
            switch (this.GetWorkItemState())
            {
                case WorkItemState.InProgress:
                    this.ExecuteWorkItem();
                    break;

                case WorkItemState.Canceled:
                    break;

                default:
                    Debug.Assert(false);
                    throw new NotSupportedException();
            }
            this._endProcessTime = DateTime.Now;
        }

        private void ExecuteWorkItem()
        {
            Exception exception = null;
            try
            {
                this._callback(this._state);
            }
            catch (Exception exception2)
            {
                exception = exception2;
            }
            this.SetException(exception);
        }

        private WorkItemState GetWorkItemState()
        {
            lock (this)
            {
                return this._workItemState;
            }
        }

        internal void SetException(Exception exception)
        {
            this._exception = exception;
            this.SignalComplete(false);
        }

        internal void SetQueueTime()
        {
            this._queuedTime = DateTime.Now;
        }

        private void SetWorkItemState(WorkItemState workItemState)
        {
            lock (this)
            {
                this._workItemState = workItemState;
            }
        }

        private void SignalComplete(bool canceled)
        {
            this.SetWorkItemState(canceled ? WorkItemState.Canceled : WorkItemState.Completed);
        }

        public bool StartingWorkItem()
        {
            this._beginProcessTime = DateTime.Now;
            lock (this)
            {
                if (this.IsCanceled)
                {
                    return false;
                }
                this.SetWorkItemState(WorkItemState.InProgress);
            }
            return true;
        }

        public bool IsCanceled
        {
            get
            {
                return (this.GetWorkItemState() == WorkItemState.Canceled);
            }
        }

        private bool IsCompleted
        {
            get
            {
                WorkItemState workItemState = this.GetWorkItemState();
                return ((workItemState == WorkItemState.Completed) || (workItemState == WorkItemState.Canceled));
            }
        }

        private enum WorkItemState
        {
            InQueue,
            InProgress,
            Completed,
            Canceled
        }
    }
}

