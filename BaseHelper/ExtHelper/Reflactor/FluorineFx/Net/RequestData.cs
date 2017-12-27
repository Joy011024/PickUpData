namespace FluorineFx.Net
{
    using FluorineFx.IO;
    using FluorineFx.Messaging.Api.Service;
    using FluorineFx.Messaging.Rtmp.Service;
    using System;
    using System.Net;

    internal class RequestData
    {
        private AMFMessage _amfMessage;
        private PendingCall _call;
        private IPendingServiceCallback _callback;
        private HttpWebRequest _request;

        public RequestData(HttpWebRequest request, AMFMessage amfMessage, PendingCall call, IPendingServiceCallback callback)
        {
            this._call = call;
            this._request = request;
            this._amfMessage = amfMessage;
            this._callback = callback;
        }

        public AMFMessage AmfMessage
        {
            get
            {
                return this._amfMessage;
            }
        }

        internal PendingCall Call
        {
            get
            {
                return this._call;
            }
        }

        public IPendingServiceCallback Callback
        {
            get
            {
                return this._callback;
            }
        }

        public HttpWebRequest Request
        {
            get
            {
                return this._request;
            }
        }
    }
}

