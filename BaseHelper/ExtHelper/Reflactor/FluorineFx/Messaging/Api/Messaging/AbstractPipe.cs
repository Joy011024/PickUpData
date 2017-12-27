namespace FluorineFx.Messaging.Api.Messaging
{
    using FluorineFx.Collections;
    using FluorineFx.Messaging.Messages;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal abstract class AbstractPipe : IPipe, IMessageInput, IMessageOutput
    {
        protected CopyOnWriteArray _consumers = new CopyOnWriteArray();
        protected CopyOnWriteArray _listeners = new CopyOnWriteArray();
        protected CopyOnWriteArray _providers = new CopyOnWriteArray();
        private static readonly ILog log = LogManager.GetLogger(typeof(AbstractPipe));

        protected AbstractPipe()
        {
        }

        public void AddPipeConnectionListener(IPipeConnectionListener listener)
        {
            this._listeners.Add(listener);
        }

        protected void FireConsumerConnectionEvent(IConsumer consumer, int type, Dictionary<string, object> parameterMap)
        {
            PipeConnectionEvent evt = new PipeConnectionEvent(this) {
                Consumer = consumer,
                Type = type,
                ParameterMap = parameterMap
            };
            this.FirePipeConnectionEvent(evt);
        }

        protected void FirePipeConnectionEvent(PipeConnectionEvent evt)
        {
            foreach (IPipeConnectionListener listener in this._listeners)
            {
                try
                {
                    listener.OnPipeConnectionEvent(evt);
                }
                catch (Exception exception)
                {
                    log.Error("Exception when handling pipe connection event", exception);
                }
            }
        }

        protected void FireProviderConnectionEvent(IProvider provider, int type, Dictionary<string, object> parameterMap)
        {
            PipeConnectionEvent evt = new PipeConnectionEvent(this) {
                Provider = provider,
                Type = type,
                ParameterMap = parameterMap
            };
            this.FirePipeConnectionEvent(evt);
        }

        public ICollection GetConsumers()
        {
            return this._consumers;
        }

        public ICollection GetProviders()
        {
            return this._providers;
        }

        public virtual IMessage PullMessage()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual IMessage PullMessage(long wait)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void PushMessage(IMessage message)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemovePipeConnectionListener(IPipeConnectionListener listener)
        {
            this._listeners.Remove(listener);
        }

        public void SendOOBControlMessage(IConsumer consumer, OOBControlMessage oobCtrlMsg)
        {
            foreach (IProvider provider in this._providers)
            {
                try
                {
                    provider.OnOOBControlMessage(consumer, this, oobCtrlMsg);
                }
                catch (Exception exception)
                {
                    log.Error("Exception when passing OOBCM from consumer to providers", exception);
                }
            }
        }

        public void SendOOBControlMessage(IProvider provider, OOBControlMessage oobCtrlMsg)
        {
            foreach (IConsumer consumer in this._consumers)
            {
                try
                {
                    consumer.OnOOBControlMessage(provider, this, oobCtrlMsg);
                }
                catch (Exception exception)
                {
                    log.Error("Exception when passing OOBCM from provider to consumers", exception);
                }
            }
        }

        public virtual bool Subscribe(IConsumer consumer, Dictionary<string, object> parameterMap)
        {
            lock (this._consumers.SyncRoot)
            {
                if (this._consumers.Contains(consumer))
                {
                    return false;
                }
                this._consumers.Add(consumer);
            }
            if (consumer is IPipeConnectionListener)
            {
                this._listeners.Add(consumer as IPipeConnectionListener);
            }
            return true;
        }

        public virtual bool Subscribe(IProvider provider, Dictionary<string, object> parameterMap)
        {
            lock (this._providers.SyncRoot)
            {
                if (this._providers.Contains(provider))
                {
                    return false;
                }
                this._providers.Add(provider);
            }
            if (provider is IPipeConnectionListener)
            {
                this._listeners.Add(provider as IPipeConnectionListener);
            }
            return true;
        }

        public bool Unsubscribe(IConsumer consumer)
        {
            lock (this._consumers.SyncRoot)
            {
                if (!this._consumers.Contains(consumer))
                {
                    return false;
                }
                this._consumers.Remove(consumer);
            }
            this.FireConsumerConnectionEvent(consumer, 5, null);
            if (consumer is IPipeConnectionListener)
            {
                this._listeners.Remove(consumer);
            }
            return true;
        }

        public bool Unsubscribe(IProvider provider)
        {
            lock (this._providers.SyncRoot)
            {
                if (!this._providers.Contains(provider))
                {
                    return false;
                }
                this._providers.Remove(provider);
            }
            this.FireProviderConnectionEvent(provider, 2, null);
            if (provider is IPipeConnectionListener)
            {
                this._listeners.Remove(provider);
            }
            return true;
        }

        public CopyOnWriteArray Listeners
        {
            get
            {
                return this._listeners;
            }
            set
            {
                this._listeners = value;
            }
        }
    }
}

