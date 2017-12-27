namespace FluorineFx.Messaging.Rtmpt
{
    using FluorineFx.Messaging.Rtmp;
    using FluorineFx.Util;
    using System;
    using System.Collections;

    internal class RtmptRequest
    {
        private RtmpServerConnection _connection;
        private ByteBuffer _data;
        private Hashtable _headers;
        private string _httpMethod;
        private string _protocol;
        private string _url;

        public RtmptRequest(RtmpServerConnection connection, string url, string protocol, string httpMethod, Hashtable headers)
        {
            this._connection = connection;
            this._url = url;
            this._protocol = protocol;
            this._httpMethod = httpMethod;
            this._headers = headers;
        }

        public RtmpServerConnection Connection
        {
            get
            {
                return this._connection;
            }
        }

        public int ContentLength
        {
            get
            {
                if (this._headers.Contains("Content-Length"))
                {
                    return Convert.ToInt32(this._headers["Content-Length"]);
                }
                return 0;
            }
        }

        public ByteBuffer Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }

        public Hashtable Headers
        {
            get
            {
                return this._headers;
            }
        }

        public string HttpMethod
        {
            get
            {
                return this._httpMethod;
            }
        }

        public int HttpVersion
        {
            get
            {
                return ((this._protocol == "HTTP/1.1") ? 1 : 0);
            }
        }

        public string Protocol
        {
            get
            {
                return this._protocol;
            }
        }

        public string Url
        {
            get
            {
                return this._url;
            }
        }
    }
}

