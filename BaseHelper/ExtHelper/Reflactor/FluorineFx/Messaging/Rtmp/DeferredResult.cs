namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx.Messaging.Api.Service;
    using FluorineFx.Messaging.Rtmp.Event;
    using System;

    [CLSCompliant(false)]
    public class DeferredResult
    {
        private IPendingServiceCall _call;
        private WeakReference _channel;
        private int _invokeId;
        private bool _resultSent = false;

        public RtmpChannel Channel
        {
            set
            {
                this._channel = new WeakReference(value);
            }
        }

        public int InvokeId
        {
            get
            {
                return this._invokeId;
            }
            set
            {
                this._invokeId = value;
            }
        }

        public object Result
        {
            set
            {
                if (this._resultSent)
                {
                    throw new Exception("You can only set the result once.");
                }
                this._resultSent = true;
                if (this._channel.IsAlive)
                {
                    RtmpChannel target = this._channel.Target as RtmpChannel;
                    Invoke message = new Invoke();
                    this._call.Result = value;
                    message.ServiceCall = this._call;
                    message.InvokeId = this._invokeId;
                    target.Write(message);
                    target.Connection.UnregisterDeferredResult(this);
                }
            }
        }

        public bool ResultSent
        {
            get
            {
                return this._resultSent;
            }
        }

        public IPendingServiceCall ServiceCall
        {
            get
            {
                return this._call;
            }
            set
            {
                this._call = value;
            }
        }
    }
}

