namespace FluorineFx.Messaging.Endpoints
{
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Messaging.Rtmpt;
    using log4net;
    using System;
    using System.Web;

    internal class RtmptEndpoint : EndpointBase
    {
        private static object _objLock = new object();
        private RtmptServer _rtmptServer;
        public const string FluorineRtmptEndpointId = "__@fluorinertmpt";
        private static readonly ILog log = LogManager.GetLogger(typeof(RtmptEndpoint));

        public RtmptEndpoint(MessageBroker messageBroker, ChannelSettings channelSettings) : base(messageBroker, channelSettings)
        {
        }

        public override void Push(IMessage message, MessageClient messageClient)
        {
        }

        public override void Service()
        {
            this._rtmptServer.Service(HttpContext.Current.Request, HttpContext.Current.Response);
        }

        public void Service(RtmptRequest rtmptRequest)
        {
            this._rtmptServer.Service(rtmptRequest);
        }

        public override void Start()
        {
            this._rtmptServer = new RtmptServer(this);
        }

        public override void Stop()
        {
        }
    }
}

