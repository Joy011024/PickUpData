namespace FluorineFx.Messaging.Api.Messaging
{
    using System;
    using System.Collections.Generic;

    [CLSCompliant(false)]
    public class PipeConnectionEvent
    {
        private IConsumer _consumer;
        private Dictionary<string, object> _parameterMap;
        private IProvider _provider;
        private object _source;
        private int _type;
        public const int CONSUMER_CONNECT_PULL = 3;
        public const int CONSUMER_CONNECT_PUSH = 4;
        public const int CONSUMER_DISCONNECT = 5;
        public const int PROVIDER_CONNECT_PULL = 0;
        public const int PROVIDER_CONNECT_PUSH = 1;
        public const int PROVIDER_DISCONNECT = 2;

        public PipeConnectionEvent(object source)
        {
            this._source = source;
        }

        public IConsumer Consumer
        {
            get
            {
                return this._consumer;
            }
            set
            {
                this._consumer = value;
            }
        }

        public Dictionary<string, object> ParameterMap
        {
            get
            {
                return this._parameterMap;
            }
            set
            {
                this._parameterMap = value;
            }
        }

        public IProvider Provider
        {
            get
            {
                return this._provider;
            }
            set
            {
                this._provider = value;
            }
        }

        public object Source
        {
            get
            {
                return this._source;
            }
            set
            {
                this._source = value;
            }
        }

        public int Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }
    }
}

