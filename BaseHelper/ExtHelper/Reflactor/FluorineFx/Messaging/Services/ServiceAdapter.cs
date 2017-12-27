namespace FluorineFx.Messaging.Services
{
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Config;
    using FluorineFx.Messaging.Messages;
    using System;

    [CLSCompliant(false)]
    public abstract class ServiceAdapter
    {
        private FluorineFx.Messaging.Config.AdapterSettings _adapterSettings;
        private FluorineFx.Messaging.Destination _destination;
        private FluorineFx.Messaging.Config.DestinationSettings _destinationSettings;
        private object _syncLock = new object();

        protected ServiceAdapter()
        {
        }

        public virtual void Init()
        {
        }

        public virtual object Invoke(IMessage message)
        {
            return null;
        }

        public virtual object Manage(CommandMessage commandMessage)
        {
            return new AcknowledgeMessage();
        }

        internal void SetAdapterSettings(FluorineFx.Messaging.Config.AdapterSettings value)
        {
            this._adapterSettings = value;
        }

        internal void SetDestination(FluorineFx.Messaging.Destination value)
        {
            this._destination = value;
        }

        internal void SetDestinationSettings(FluorineFx.Messaging.Config.DestinationSettings value)
        {
            this._destinationSettings = value;
        }

        public virtual void Stop()
        {
        }

        public FluorineFx.Messaging.Config.AdapterSettings AdapterSettings
        {
            get
            {
                return this._adapterSettings;
            }
        }

        public FluorineFx.Messaging.Destination Destination
        {
            get
            {
                return this._destination;
            }
        }

        public FluorineFx.Messaging.Config.DestinationSettings DestinationSettings
        {
            get
            {
                return this._destinationSettings;
            }
        }

        public virtual bool HandlesSubscriptions
        {
            get
            {
                return false;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this._syncLock;
            }
        }
    }
}

