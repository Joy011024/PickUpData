namespace FluorineFx.Util
{
    using System;
    using System.Collections;

    internal abstract class ObjectPool : DisposableBase
    {
        private bool _forceGC = true;
        private int _growth = 10;
        private Queue _queue;

        protected ObjectPool()
        {
        }

        private void AddObjects(int count)
        {
            if (!base.IsDisposed)
            {
                if (this._forceGC)
                {
                    GC.Collect();
                }
                for (int i = 1; i <= count; i++)
                {
                    object obj2 = this.GetObject();
                    this._queue.Enqueue(obj2);
                }
                if (this._forceGC)
                {
                    GC.Collect();
                }
            }
        }

        protected void CheckIn(object obj)
        {
            if (!base.IsDisposed)
            {
                lock (this._queue.SyncRoot)
                {
                    this._queue.Enqueue(obj);
                }
            }
        }

        protected object CheckOut()
        {
            if (base.IsDisposed)
            {
                throw new ObjectDisposedException("ObjectPool");
            }
            object obj2 = null;
            lock (this._queue.SyncRoot)
            {
                if (this._queue.Count == 0)
                {
                    this.AddObjects(this._growth);
                }
                obj2 = this._queue.Dequeue();
            }
            if (this._forceGC)
            {
                GC.WaitForPendingFinalizers();
            }
            return obj2;
        }

        protected override void Free()
        {
            lock (this._queue.SyncRoot)
            {
                while (this._queue.Count > 0)
                {
                    object obj2 = this._queue.Dequeue();
                    try
                    {
                        if (obj2 is IDisposable)
                        {
                            (obj2 as IDisposable).Dispose();
                        }
                    }
                    catch
                    {
                    }
                }
            }
            base.Free();
        }

        protected abstract object GetObject();
        protected void Initialize(int capacity)
        {
            if (!base.IsDisposed)
            {
                this._queue = new Queue(capacity);
                lock (this._queue.SyncRoot)
                {
                    this.AddObjects(capacity);
                }
                if (this._forceGC)
                {
                    GC.WaitForPendingFinalizers();
                }
            }
        }

        protected void Initialize(int capacity, int growth)
        {
            if (!base.IsDisposed)
            {
                this._growth = growth;
                this.Initialize(capacity);
            }
        }

        protected void Initialize(int capacity, int growth, bool forceGCOnGrowth)
        {
            if (!base.IsDisposed)
            {
                this._forceGC = forceGCOnGrowth;
                this.Initialize(capacity, growth);
            }
        }

        public int Growth
        {
            get
            {
                if (base.IsDisposed)
                {
                    throw new ObjectDisposedException("ObjectPool");
                }
                return this._growth;
            }
        }

        protected int Length
        {
            get
            {
                if (base.IsDisposed)
                {
                    throw new ObjectDisposedException("ObjectPool");
                }
                lock (this._queue.SyncRoot)
                {
                    return this._queue.Count;
                }
            }
        }

        protected object SyncRoot
        {
            get
            {
                if (base.IsDisposed)
                {
                    throw new ObjectDisposedException("ObjectPool");
                }
                return this._queue.SyncRoot;
            }
        }
    }
}

