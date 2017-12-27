namespace FluorineFx.Messaging.Rtmp.Stream
{
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Messaging;
    using FluorineFx.Messaging.Api.Persistence;
    using FluorineFx.Messaging.Messages;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class BroadcastScope : BasicScope, IBroadcastScope, IBasicScope, ICoreObject, IAttributeStore, IEventDispatcher, IEventHandler, IEventListener, IEventObservable, IPersistable, IEnumerable, IPipe, IMessageInput, IMessageOutput, IPipeConnectionListener
    {
        private int _compCounter;
        private bool _hasRemoved;
        private InMemoryPushPushPipe _pipe;

        public BroadcastScope(IScope parent, string name) : base(parent, "bs", name, false)
        {
            this._pipe = new InMemoryPushPushPipe();
            this._pipe.AddPipeConnectionListener(this);
            this._compCounter = 0;
            this._hasRemoved = false;
            base._keepOnDisconnect = true;
        }

        public void AddPipeConnectionListener(IPipeConnectionListener listener)
        {
            this._pipe.AddPipeConnectionListener(listener);
        }

        public ICollection GetConsumers()
        {
            return this._pipe.GetConsumers();
        }

        public ICollection GetProviders()
        {
            return this._pipe.GetProviders();
        }

        public void OnPipeConnectionEvent(PipeConnectionEvent evt)
        {
            switch (evt.Type)
            {
                case 0:
                case 1:
                case 3:
                case 4:
                    this._compCounter++;
                    break;

                case 2:
                case 5:
                    this._compCounter--;
                    if (this._compCounter <= 0)
                    {
                        if (base.HasParent)
                        {
                            (ScopeUtils.GetScopeService(this.Parent, typeof(IProviderService)) as IProviderService).UnregisterBroadcastStream(this.Parent, this.Name);
                        }
                        this._hasRemoved = true;
                    }
                    break;

                default:
                    throw new NotSupportedException("Event type not supported: " + evt.Type);
            }
        }

        public IMessage PullMessage()
        {
            return this._pipe.PullMessage();
        }

        public IMessage PullMessage(long wait)
        {
            return this._pipe.PullMessage(wait);
        }

        public void PushMessage(IMessage message)
        {
            this._pipe.PushMessage(message);
        }

        public void RemovePipeConnectionListener(IPipeConnectionListener listener)
        {
            this._pipe.RemovePipeConnectionListener(listener);
        }

        public void SendOOBControlMessage(IConsumer consumer, OOBControlMessage oobCtrlMsg)
        {
            this._pipe.SendOOBControlMessage(consumer, oobCtrlMsg);
        }

        public void SendOOBControlMessage(IProvider provider, OOBControlMessage oobCtrlMsg)
        {
            this._pipe.SendOOBControlMessage(provider, oobCtrlMsg);
        }

        public bool Subscribe(IConsumer consumer, Dictionary<string, object> parameterMap)
        {
            lock (this._pipe)
            {
                return (!this._hasRemoved && this._pipe.Subscribe(consumer, parameterMap));
            }
        }

        public bool Subscribe(IProvider provider, Dictionary<string, object> parameterMap)
        {
            lock (this._pipe)
            {
                return (!this._hasRemoved && this._pipe.Subscribe(provider, parameterMap));
            }
        }

        public bool Unsubscribe(IConsumer consumer)
        {
            return this._pipe.Unsubscribe(consumer);
        }

        public bool Unsubscribe(IProvider provider)
        {
            return this._pipe.Unsubscribe(provider);
        }
    }
}

