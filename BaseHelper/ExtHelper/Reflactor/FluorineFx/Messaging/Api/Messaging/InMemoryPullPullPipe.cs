namespace FluorineFx.Messaging.Api.Messaging
{
    using FluorineFx.Messaging.Messages;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class InMemoryPullPullPipe : AbstractPipe
    {
        private static ILog log = LogManager.GetLogger(typeof(InMemoryPullPullPipe));

        public override IMessage PullMessage()
        {
            IMessage message = null;
            foreach (IProvider provider in base._providers)
            {
                if (provider is IPullableProvider)
                {
                    try
                    {
                        message = (provider as IPullableProvider).PullMessage(this);
                        if (message != null)
                        {
                            return message;
                        }
                    }
                    catch (Exception exception)
                    {
                        if (exception is IOException)
                        {
                            throw exception;
                        }
                        log.Error("Exception when pulling message from provider", exception);
                    }
                }
            }
            return message;
        }

        public override IMessage PullMessage(long wait)
        {
            IMessage message = null;
            int count = base._providers.Count;
            long num2 = (count > 0) ? (wait / ((long) count)) : 0L;
            foreach (IProvider provider in base._providers)
            {
                if (provider is IPullableProvider)
                {
                    try
                    {
                        message = (provider as IPullableProvider).PullMessage(this, num2);
                        if (message != null)
                        {
                            return message;
                        }
                    }
                    catch (Exception exception)
                    {
                        log.Error("Exception when pulling message from provider", exception);
                    }
                }
            }
            return message;
        }

        public override void PushMessage(IMessage message)
        {
        }

        public override bool Subscribe(IConsumer consumer, Dictionary<string, object> parameterMap)
        {
            bool flag = base.Subscribe(consumer, parameterMap);
            if (flag)
            {
                base.FireConsumerConnectionEvent(consumer, 3, parameterMap);
            }
            return flag;
        }

        public override bool Subscribe(IProvider provider, Dictionary<string, object> parameterMap)
        {
            if (!(provider is IPullableProvider))
            {
                throw new NotSupportedException("Non-pullable provider not supported by PullPullPipe");
            }
            bool flag = base.Subscribe(provider, parameterMap);
            if (flag)
            {
                base.FireProviderConnectionEvent(provider, 0, parameterMap);
            }
            return flag;
        }
    }
}

