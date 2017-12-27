namespace FluorineFx.Messaging.Rtmp.Service
{
    using FluorineFx.Messaging.Api.Service;
    using System;
    using System.Collections.Generic;

    internal class PendingCall : Call, IPendingServiceCall, IServiceCall
    {
        private List<IPendingServiceCallback> _callbacks;
        private object _result;

        public PendingCall(string method) : base(method)
        {
            this._callbacks = new List<IPendingServiceCallback>();
        }

        public PendingCall(string method, object[] args) : base(method, args)
        {
            this._callbacks = new List<IPendingServiceCallback>();
        }

        public PendingCall(string name, string method, object[] args) : base(name, method, args)
        {
            this._callbacks = new List<IPendingServiceCallback>();
        }

        public IPendingServiceCallback[] GetCallbacks()
        {
            return this._callbacks.ToArray();
        }

        public void RegisterCallback(IPendingServiceCallback callback)
        {
            this._callbacks.Add(callback);
        }

        public void UnregisterCallback(IPendingServiceCallback callback)
        {
            this._callbacks.Remove(callback);
        }

        public object Result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
            }
        }
    }
}

