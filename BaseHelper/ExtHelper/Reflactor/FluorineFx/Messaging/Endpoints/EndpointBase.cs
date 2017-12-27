namespace FluorineFx.Messaging.Endpoints
{
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Messages;
    using FluorineFx.Util;
    using System;

    internal class EndpointBase : IEndpoint
    {
        protected ChannelSettings _channelSettings;
        private string _id;
        protected MessageBroker _messageBroker;

        public EndpointBase(MessageBroker messageBroker, ChannelSettings channelSettings)
        {
            this._messageBroker = messageBroker;
            this._channelSettings = channelSettings;
            this._id = this._channelSettings.Id;
        }

        public MessageBroker GetMessageBroker()
        {
            return this._messageBroker;
        }

        public ChannelSettings GetSettings()
        {
            return this._channelSettings;
        }

        public virtual void Push(IMessage message, MessageClient messageClient)
        {
            throw new NotSupportedException();
        }

        public virtual void Service()
        {
        }

        public virtual IMessage ServiceMessage(IMessage message)
        {
            ValidationUtils.ArgumentNotNull(message, "message");
            return this._messageBroker.RouteMessage(message, this);
        }

        public virtual void Start()
        {
        }

        public virtual void Stop()
        {
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

        public virtual bool IsSecure
        {
            get
            {
                return false;
            }
        }
    }
}

