namespace FluorineFx.Messaging.Endpoints
{
    using FluorineFx.Messaging;
    using log4net;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Net;
    using System.Web;

    internal class RemotingConnection : BaseConnection
    {
        private IEndpoint _endpoint;
        private IPEndPoint _remoteEndPoint;
        private static ILog log = LogManager.GetLogger(typeof(RemotingConnection));

        public RemotingConnection(IEndpoint endpoint, string path, string connectionId, Hashtable parameters) : base(path, connectionId, parameters)
        {
            this._endpoint = endpoint;
            IPAddress address = IPAddress.Parse(HttpContext.Current.Request.UserHostAddress);
            this._remoteEndPoint = new IPEndPoint(address, 80);
        }

        public override int ClientLeaseTime
        {
            get
            {
                int timeoutMinutes = this.Endpoint.GetMessageBroker().FlexClientSettings.TimeoutMinutes;
                if (this.Endpoint is AMFEndpoint)
                {
                    timeoutMinutes = Math.Max(timeoutMinutes, 1);
                    AMFEndpoint endpoint = this.Endpoint as AMFEndpoint;
                    Debug.Assert(endpoint.GetSettings().IsPollingEnabled);
                    int num2 = endpoint.GetSettings().PollingIntervalSeconds / 60;
                    timeoutMinutes = Math.Max(timeoutMinutes, num2 + 1);
                }
                return timeoutMinutes;
            }
        }

        public IEndpoint Endpoint
        {
            get
            {
                return this._endpoint;
            }
        }

        public override int LastPingTime
        {
            get
            {
                return -1;
            }
        }

        public override long ReadBytes
        {
            get
            {
                return 0L;
            }
        }

        public override IPEndPoint RemoteEndPoint
        {
            get
            {
                return this._remoteEndPoint;
            }
        }

        public override long WrittenBytes
        {
            get
            {
                return 0L;
            }
        }
    }
}

