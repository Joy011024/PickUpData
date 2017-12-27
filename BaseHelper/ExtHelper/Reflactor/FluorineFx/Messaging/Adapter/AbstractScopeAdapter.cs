namespace FluorineFx.Messaging.Adapter
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Service;
    using System;

    public abstract class AbstractScopeAdapter : IScopeHandler, IEventHandler
    {
        private bool _canAddChildScope = true;
        private bool _canCallService = true;
        private bool _canConnect = true;
        private bool _canHandleEvent = true;
        private bool _canJoin = true;
        private bool _canStart = true;

        public bool AddChildScope(IBasicScope scope)
        {
            return this._canAddChildScope;
        }

        public virtual bool Connect(IConnection connection, IScope scope, object[] parameters)
        {
            return this._canConnect;
        }

        public virtual void Disconnect(IConnection connection, IScope scope)
        {
        }

        public bool HandleEvent(IEvent evt)
        {
            return this._canHandleEvent;
        }

        public virtual bool Join(IClient client, IScope scope)
        {
            return this._canJoin;
        }

        public virtual void Leave(IClient client, IScope scope)
        {
        }

        public void RemoveChildScope(IBasicScope scope)
        {
        }

        public bool ServiceCall(IConnection connection, IServiceCall call)
        {
            return this._canCallService;
        }

        public virtual bool Start(IScope scope)
        {
            return this._canStart;
        }

        public virtual void Stop(IScope scope)
        {
        }

        public bool CanCallService
        {
            set
            {
                this._canCallService = value;
            }
        }

        public bool CanConnect
        {
            set
            {
                this._canConnect = value;
            }
        }

        public bool CanJoin
        {
            set
            {
                this._canJoin = value;
            }
        }

        public bool CanStart
        {
            set
            {
                this._canStart = value;
            }
        }
    }
}

