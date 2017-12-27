namespace FluorineFx.Messaging.Api.Messaging
{
    using FluorineFx.Exceptions;
    using FluorineFx.Messaging.Messages;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class InMemoryPushPushPipe : AbstractPipe
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(InMemoryPushPushPipe));

        public override IMessage PullMessage()
        {
            return null;
        }

        public override IMessage PullMessage(long wait)
        {
            return null;
        }

        public override void PushMessage(IMessage message)
        {
            foreach (IConsumer consumer in base.GetConsumers())
            {
                try
                {
                    (consumer as IPushableConsumer).PushMessage(this, message);
                }
                catch (Exception exception)
                {
                    if (exception is IOException)
                    {
                        throw exception;
                    }
                    log.Error("Exception when pushing message to consumer", exception);
                }
            }
        }

        public override bool Subscribe(IConsumer consumer, Dictionary<string, object> parameterMap)
        {
            if (!(consumer is IPushableConsumer))
            {
                throw new FluorineException("Non-pushable consumer not supported by PushPushPipe");
            }
            bool flag = base.Subscribe(consumer, parameterMap);
            if (flag)
            {
                base.FireConsumerConnectionEvent(consumer, 4, parameterMap);
            }
            return flag;
        }

        public override bool Subscribe(IProvider provider, Dictionary<string, object> parameterMap)
        {
            bool flag = base.Subscribe(provider, parameterMap);
            if (flag)
            {
                base.FireProviderConnectionEvent(provider, 1, parameterMap);
            }
            return flag;
        }
    }
}

