namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Stream;
    using System;

    [CLSCompliant(false)]
    public abstract class AbstractStream : IStream
    {
        private IStreamCodecInfo _codecInfo;
        private string _name;
        private IScope _scope;
        private object _syncLock = new object();

        protected AbstractStream()
        {
        }

        public virtual void Close()
        {
        }

        protected IStreamAwareScopeHandler GetStreamAwareHandler()
        {
            if (this._scope != null)
            {
                IScopeHandler handler = this._scope.Handler;
                if (handler is IStreamAwareScopeHandler)
                {
                    return (handler as IStreamAwareScopeHandler);
                }
            }
            return null;
        }

        public virtual void Start()
        {
        }

        public virtual void Stop()
        {
        }

        public IStreamCodecInfo CodecInfo
        {
            get
            {
                return this._codecInfo;
            }
            set
            {
                this._codecInfo = value;
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

        public IScope Scope
        {
            get
            {
                return this._scope;
            }
            set
            {
                this._scope = value;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this._syncLock;
            }
        }
    }
}

