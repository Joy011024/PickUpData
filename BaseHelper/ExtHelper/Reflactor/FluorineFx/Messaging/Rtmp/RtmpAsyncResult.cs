namespace FluorineFx.Messaging.Rtmp
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal sealed class RtmpAsyncResult : IAsyncResult
    {
        private byte[] _buffer;
        private AsyncCallback _callback;
        private int _count;
        private System.Exception _exception;
        private ManualResetEvent _handle;
        private volatile bool _isComplete;
        private int _offset;
        private object _state;

        internal RtmpAsyncResult(AsyncCallback callback, object state, byte[] buffer, int offset, int count)
        {
            this._callback = callback;
            this._state = state;
            this._buffer = buffer;
            this._offset = offset;
            this._count = count;
        }

        internal bool HasException()
        {
            return (this._exception != null);
        }

        internal void SetComplete(System.Exception ex)
        {
            this._exception = ex;
            lock (this)
            {
                if (this._isComplete)
                {
                    return;
                }
                this._isComplete = true;
                if (this._handle != null)
                {
                    this._handle.Set();
                }
            }
            if (this._callback != null)
            {
                this._callback(this);
            }
        }

        public object AsyncState
        {
            get
            {
                return this._state;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                lock (this)
                {
                    if (this._handle == null)
                    {
                        this._handle = new ManualResetEvent(this._isComplete);
                    }
                }
                return this._handle;
            }
        }

        internal byte[] Buffer
        {
            get
            {
                return this._buffer;
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return false;
            }
        }

        internal int Count
        {
            get
            {
                return this._count;
            }
        }

        internal System.Exception Exception
        {
            get
            {
                return this._exception;
            }
        }

        public bool IsCompleted
        {
            get
            {
                lock (this)
                {
                    return this._isComplete;
                }
            }
        }

        internal int Offset
        {
            get
            {
                return this._offset;
            }
        }
    }
}

