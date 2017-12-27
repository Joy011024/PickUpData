namespace FluorineFx
{
    using FluorineFx.Threading;
    using System;
    using System.Threading;
    using System.Web;

    internal class AsyncHandler : IAsyncResult
    {
        private AsyncCallback _callback;
        private bool _completed;
        private FluorineGateway _gateway;
        private HttpApplication _httpApplication;
        private object _state;

        public AsyncHandler(AsyncCallback callback, FluorineGateway gateway, HttpApplication httpApplication, object state)
        {
            this._gateway = gateway;
            this._callback = callback;
            this._httpApplication = httpApplication;
            this._state = state;
            this._completed = false;
        }

        private void AsyncTask(object state)
        {
            HttpContext.Current = this._httpApplication.Context;
            this._gateway.HandleXAmfEx(this._httpApplication);
            this._gateway.HandleSWX(this._httpApplication);
            this._gateway.HandleJSONRPC(this._httpApplication);
            this._gateway.HandleRtmpt(this._httpApplication);
            this._gateway = null;
            this._httpApplication = null;
            this._completed = true;
            this._callback(this);
        }

        public void Start()
        {
            ThreadPoolEx.Global.QueueUserWorkItem(new WaitCallback(this.AsyncTask), null);
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
                return null;
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return false;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return this._completed;
            }
        }
    }
}

