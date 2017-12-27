namespace FluorineFx.Messaging.Config
{
    using FluorineFx.Util;
    using System;
    using System.Collections;
    using System.Xml;

    public sealed class ChannelSettings : Hashtable
    {
        private string _endpointClass;
        private string _endpointUri;
        private string _id;
        private int _maxWaitingPollRequests;
        private UriBase _uri;
        private int _waitIntervalMillis;
        public const string BindAddressKey = "bind-address";
        public const string ContextRoot = "{context.root}";
        public const string MaxWaitingPollRequestsKey = "max-waiting-poll-requests";
        public const string PollingEnabledKey = "polling-enabled";
        public const string PollingIntervalMillisKey = "polling-interval-millis";
        public const string PollingIntervalSecondsKey = "polling-interval-seconds";
        public const string WaitIntervalMillisKey = "wait-interval-millis";

        internal ChannelSettings()
        {
            this._maxWaitingPollRequests = 0;
            this._waitIntervalMillis = 0;
        }

        internal ChannelSettings(XmlNode channelDefinitionNode)
        {
            this._id = channelDefinitionNode.Attributes["id"].Value;
            XmlNode node = channelDefinitionNode.SelectSingleNode("endpoint");
            this._endpointClass = node.Attributes["class"].Value;
            this._endpointUri = node.Attributes["uri"].Value;
            this._uri = new UriBase(this._endpointUri);
            XmlNode node2 = channelDefinitionNode.SelectSingleNode("properties");
            if (node2 != null)
            {
                foreach (XmlNode node3 in node2.SelectNodes("*"))
                {
                    this[node3.Name] = node3.InnerXml;
                }
            }
            if (this.ContainsKey("max-waiting-poll-requests"))
            {
                this._maxWaitingPollRequests = Convert.ToInt32(this["max-waiting-poll-requests"]);
            }
            if (this.ContainsKey("wait-interval-millis"))
            {
                this._waitIntervalMillis = Convert.ToInt32(this["wait-interval-millis"]);
            }
        }

        internal ChannelSettings(string id, string endpointClass) : this()
        {
            this._id = id;
            this._endpointClass = endpointClass;
        }

        internal ChannelSettings(string id, string endpointClass, string endpointUri) : this()
        {
            this._id = id;
            this._endpointClass = endpointClass;
            this._endpointUri = endpointUri;
            this._uri = new UriBase(this._endpointUri);
        }

        internal bool Bind(string path, string contextPath)
        {
            if (this._uri != null)
            {
                string str = this._uri.Path;
                if (!str.StartsWith("/"))
                {
                    str = "/" + str;
                }
                if (contextPath == "/")
                {
                    contextPath = string.Empty;
                }
                if (str.IndexOf("/{context.root}") != -1)
                {
                    str = str.Replace("/{context.root}", contextPath);
                }
                else
                {
                    str = str.Replace("{context.root}", contextPath);
                }
                if (str.ToLower() == path.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public UriBase GetUri()
        {
            return this._uri;
        }

        public override string ToString()
        {
            return ("Channel id = " + this._id + " uri: " + this._uri.Uri + " endpointPath: " + this._uri.Path);
        }

        public string BindAddress
        {
            get
            {
                if (this.ContainsKey("bind-address"))
                {
                    return this["bind-address"].ToString();
                }
                return null;
            }
        }

        public string Class
        {
            get
            {
                return this._endpointClass;
            }
        }

        public string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public bool IsPollingEnabled
        {
            get
            {
                return (this.ContainsKey("polling-enabled") && Convert.ToBoolean(this["polling-enabled"]));
            }
        }

        public int MaxWaitingPollRequests
        {
            get
            {
                return this._maxWaitingPollRequests;
            }
        }

        public int PollingIntervalMillis
        {
            get
            {
                if (this.ContainsKey("polling-interval-millis"))
                {
                    return Convert.ToInt32(this["polling-interval-millis"]);
                }
                return 0xbb8;
            }
        }

        public int PollingIntervalSeconds
        {
            get
            {
                if (this.ContainsKey("polling-interval-seconds"))
                {
                    return Convert.ToInt32(this["polling-interval-seconds"]);
                }
                return 8;
            }
        }

        public string Uri
        {
            set
            {
                this._uri = new UriBase(value);
            }
        }

        public int WaitIntervalMillis
        {
            get
            {
                return this._waitIntervalMillis;
            }
        }
    }
}

