namespace FluorineFx.Net
{
    using FluorineFx;
    using FluorineFx.IO;
    using FluorineFx.Messaging.Api.Service;
    using FluorineFx.Messaging.Rtmp;
    using FluorineFx.Messaging.Rtmp.SO;
    using FluorineFx.Util;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;

    [CLSCompliant(false)]
    public sealed class NetConnection
    {
        private object[] _arguments;
        private object _client;
        private string _clientId = null;
        private System.Net.CookieContainer _cookieContainer;
        private Dictionary<string, AMFHeader> _headers = new Dictionary<string, AMFHeader>();
        private INetConnectionClient _netConnectionClient;
        private FluorineFx.ObjectEncoding _objectEncoding = FluorineFx.ObjectEncoding.AMF0;
        private string _playerVersion = "WIN 9,0,115,0";
        private System.Uri _uri;

        private event ConnectHandler _connectHandler;

        private event DisconnectHandler _disconnectHandler;

        private event NetStatusHandler _netStatusHandler;

        public event NetStatusHandler NetStatus
        {
            add
            {
                this._netStatusHandler += value;
            }
            remove
            {
                this._netStatusHandler -= value;
            }
        }

        public event ConnectHandler OnConnect
        {
            add
            {
                this._connectHandler += value;
            }
            remove
            {
                this._connectHandler -= value;
            }
        }

        public event DisconnectHandler OnDisconnect
        {
            add
            {
                this._disconnectHandler += value;
            }
            remove
            {
                this._disconnectHandler -= value;
            }
        }

        public NetConnection()
        {
            this._client = this;
            this._cookieContainer = new System.Net.CookieContainer();
            TypeHelper._Init();
        }

        public void AddHeader(string operation, bool mustUnderstand, object param)
        {
            if (param == null)
            {
                if (this._headers.ContainsKey(operation))
                {
                    this._headers.Remove(operation);
                }
            }
            else
            {
                AMFHeader header = new AMFHeader(operation, mustUnderstand, param);
                this._headers[operation] = header;
            }
        }

        public IAsyncResult BeginConnect(string command, AsyncCallback callback, object state, params object[] arguments)
        {
            this._uri = new System.Uri(command);
            this._arguments = arguments;
            AsyncResultNoResult result = new AsyncResultNoResult(callback, state);
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.DoConnect), result);
            return result;
        }

        public void Call(string command, IPendingServiceCallback callback, params object[] arguments)
        {
            this._netConnectionClient.Call(command, callback, arguments);
        }

        public void Call(string endpoint, string destination, string source, string operation, IPendingServiceCallback callback, params object[] arguments)
        {
            this._netConnectionClient.Call(endpoint, destination, source, operation, callback, arguments);
        }

        public void Close()
        {
            if (this._netConnectionClient != null)
            {
                this._netConnectionClient.Close();
            }
            this._netConnectionClient = null;
        }

        private void Connect()
        {
            if (this._uri.Scheme == "http")
            {
                this._netConnectionClient = new RemotingClient(this);
                this._netConnectionClient.Connect(this._uri.ToString(), this._arguments);
            }
            else
            {
                if (this._uri.Scheme != "rtmp")
                {
                    throw new UriFormatException();
                }
                this._netConnectionClient = new RtmpClient(this);
                this._netConnectionClient.Connect(this._uri.ToString(), this._arguments);
            }
        }

        public void Connect(string command, params object[] arguments)
        {
            this._uri = new System.Uri(command);
            this._arguments = arguments;
            this.Connect();
        }

        private void DoConnect(object asyncResult)
        {
            AsyncResultNoResult result = asyncResult as AsyncResultNoResult;
            try
            {
                this.Connect();
                result.SetAsCompleted(null, false);
            }
            catch (Exception exception)
            {
                result.SetAsCompleted(exception, false);
            }
        }

        public void EndConnect(IAsyncResult asyncResult)
        {
            (asyncResult as AsyncResultNoResult).EndInvoke();
        }

        internal void OnSharedObject(RtmpConnection connection, RtmpChannel channel, RtmpHeader header, SharedObjectMessage message)
        {
            RemoteSharedObject.Dispatch(message);
        }

        internal void RaiseDisconnect()
        {
            RemoteSharedObject.DispatchDisconnect(this);
            if (this._disconnectHandler != null)
            {
                this._disconnectHandler(this, new EventArgs());
            }
        }

        internal void RaiseNetStatus(ASObject info)
        {
            if (this._netStatusHandler != null)
            {
                this._netStatusHandler(this, new NetStatusEventArgs(info));
            }
        }

        internal void RaiseNetStatus(Exception exception)
        {
            if (this._netStatusHandler != null)
            {
                this._netStatusHandler(this, new NetStatusEventArgs(exception));
            }
        }

        internal void RaiseNetStatus(string message)
        {
            if (this._netStatusHandler != null)
            {
                this._netStatusHandler(this, new NetStatusEventArgs(message));
            }
        }

        internal void RaiseOnConnect()
        {
            if (this._connectHandler != null)
            {
                this._connectHandler(this, new EventArgs());
            }
        }

        internal void SetClientId(string clientId)
        {
            this._clientId = clientId;
        }

        public void SetCredentials(string userid, string password)
        {
            ASObject param = new ASObject();
            param.Add("userid", userid);
            param.Add("password", password);
            this.AddHeader("Credentials", false, param);
        }

        public object Client
        {
            get
            {
                return this._client;
            }
            set
            {
                ValidationUtils.ArgumentNotNull(value, "Client");
                this._client = value;
            }
        }

        public string ClientId
        {
            get
            {
                return this._clientId;
            }
        }

        public bool Connected
        {
            get
            {
                return ((this._netConnectionClient != null) && this._netConnectionClient.Connected);
            }
        }

        public System.Net.CookieContainer CookieContainer
        {
            get
            {
                return this._cookieContainer;
            }
        }

        internal Dictionary<string, AMFHeader> Headers
        {
            get
            {
                return this._headers;
            }
        }

        internal INetConnectionClient NetConnectionClient
        {
            get
            {
                return this._netConnectionClient;
            }
        }

        public FluorineFx.ObjectEncoding ObjectEncoding
        {
            get
            {
                return this._objectEncoding;
            }
            set
            {
                this._objectEncoding = value;
            }
        }

        public string PlayerVersion
        {
            get
            {
                return this._playerVersion;
            }
            set
            {
                this._playerVersion = value;
            }
        }

        public System.Uri Uri
        {
            get
            {
                return this._uri;
            }
        }
    }
}

