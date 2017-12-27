namespace FluorineFx.Messaging.Messages
{
    using FluorineFx;
    using System;
    using System.Collections.Generic;

    [CLSCompliant(false)]
    public class MessageBase : IMessage, ICloneable
    {
        protected object _body;
        protected object _clientId;
        protected string _destination;
        protected Dictionary<string, object> _headers = new Dictionary<string, object>();
        protected string _messageId;
        protected long _timestamp;
        protected long _timeToLive;
        public const string DestinationClientIdHeader = "DSDstClientId";
        public const string EndpointHeader = "DSEndpoint";
        public const string FlexClientIdHeader = "DSId";
        public const string RemoteCredentialsHeader = "DSRemoteCredentials";
        public const string RequestTimeoutHeader = "DSRequestTimeout";

        public virtual object Clone()
        {
            MessageBase base2 = base.MemberwiseClone() as MessageBase;
            if (this._headers != null)
            {
                base2.headers = new Dictionary<string, object>(this._headers);
            }
            return base2;
        }

        public object GetHeader(string name)
        {
            if (this._headers != null)
            {
                return this._headers[name];
            }
            return null;
        }

        public bool HeaderExists(string name)
        {
            return ((this._headers != null) && this._headers.ContainsKey(name));
        }

        public void SetHeader(string name, object value)
        {
            if (this._headers == null)
            {
                this._headers = new ASObject();
            }
            this._headers[name] = value;
        }

        public object body
        {
            get
            {
                return this._body;
            }
            set
            {
                this._body = value;
            }
        }

        public object clientId
        {
            get
            {
                return this._clientId;
            }
            set
            {
                this._clientId = value;
            }
        }

        public string destination
        {
            get
            {
                return this._destination;
            }
            set
            {
                this._destination = value;
            }
        }

        public Dictionary<string, object> headers
        {
            get
            {
                return this._headers;
            }
            set
            {
                this._headers = value;
            }
        }

        public string messageId
        {
            get
            {
                return this._messageId;
            }
            set
            {
                this._messageId = value;
            }
        }

        public long timestamp
        {
            get
            {
                return this._timestamp;
            }
            set
            {
                this._timestamp = value;
            }
        }

        public long timeToLive
        {
            get
            {
                return this._timeToLive;
            }
            set
            {
                this._timeToLive = value;
            }
        }
    }
}

