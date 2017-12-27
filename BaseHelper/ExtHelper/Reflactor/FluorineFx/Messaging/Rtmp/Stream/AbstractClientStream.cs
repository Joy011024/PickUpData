namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Stream;
    using System;

    [CLSCompliant(false)]
    public abstract class AbstractClientStream : AbstractStream, IClientStream, IStream, IBWControllable
    {
        private IBandwidthConfigure _bwConfig;
        private int _clientBufferDuration;
        private WeakReference _streamCapableConnection;
        private int _streamId;

        protected AbstractClientStream()
        {
        }

        public IBWControllable GetParentBWControllable()
        {
            return this.Connection;
        }

        public void SetClientBufferDuration(int bufferTime)
        {
            this._clientBufferDuration = bufferTime;
        }

        public virtual IBandwidthConfigure BandwidthConfiguration
        {
            get
            {
                return this._bwConfig;
            }
            set
            {
                this._bwConfig = value;
            }
        }

        public int ClientBufferDuration
        {
            get
            {
                return this._clientBufferDuration;
            }
        }

        public IStreamCapableConnection Connection
        {
            get
            {
                if ((this._streamCapableConnection != null) && this._streamCapableConnection.IsAlive)
                {
                    return (this._streamCapableConnection.Target as IStreamCapableConnection);
                }
                return null;
            }
            set
            {
                this._streamCapableConnection = new WeakReference(value);
            }
        }

        public int StreamId
        {
            get
            {
                return this._streamId;
            }
            set
            {
                this._streamId = value;
            }
        }
    }
}

