namespace FluorineFx.Util
{
    using System;
    using System.Runtime.CompilerServices;

    public class DisposableBase : IDisposable
    {
        private volatile bool _disposed = false;

        protected virtual void CheckDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(null);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this.Free();
                }
                this.FreeUnmanaged();
                this._disposed = true;
            }
        }

        ~DisposableBase()
        {
            this.Dispose(false);
        }

        protected virtual void Free()
        {
        }

        protected virtual void FreeUnmanaged()
        {
        }

        public bool IsDisposed
        {
            get
            {
                return this._disposed;
            }
        }
    }
}

