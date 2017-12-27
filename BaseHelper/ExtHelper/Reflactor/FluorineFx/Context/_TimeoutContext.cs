namespace FluorineFx.Context
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Security;
    using System;
    using System.Collections;
    using System.Security.Principal;
    using System.Threading;

    internal sealed class _TimeoutContext : FluorineContext
    {
        private IClient _client;
        private IConnection _connection;
        private Hashtable _items;

        private _TimeoutContext()
        {
        }

        internal _TimeoutContext(IConnection connection, IClient client)
        {
            this._client = client;
            this._connection = connection;
        }

        public override void ClearPrincipal()
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        internal override string EncryptCredentials(IEndpoint endpoint, IPrincipal principal, string userId, string password)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        private Hashtable GetItems()
        {
            if (this._items == null)
            {
                this._items = new Hashtable();
            }
            return this._items;
        }

        public override IResource GetResource(string location)
        {
            return new FileSystemResource(location);
        }

        public override IPrincipal RestorePrincipal(ILoginCommand loginCommand)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        internal override IPrincipal RestorePrincipal(ILoginCommand loginCommand, string key)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        internal override void StorePrincipal(IPrincipal principal, string key)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public override void StorePrincipal(IPrincipal principal, string userId, string password)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public override string AbsoluteUri
        {
            get
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }

        public override string ActivationMode
        {
            get
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }

        public override string ApplicationPath
        {
            get
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }

        public override IApplicationState ApplicationState
        {
            get
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }

        public override IClient Client
        {
            get
            {
                return this._client;
            }
        }

        public override IConnection Connection
        {
            get
            {
                return this._connection;
            }
        }

        public override IDictionary Items
        {
            get
            {
                return this.GetItems();
            }
        }

        public override string PhysicalApplicationPath
        {
            get
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }

        public override string RequestApplicationPath
        {
            get
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }

        public override string RequestPath
        {
            get
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }

        public override string RootPath
        {
            get
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }

        public override ISessionState Session
        {
            get
            {
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }

        public override IPrincipal User
        {
            get
            {
                return Thread.CurrentPrincipal;
            }
            set
            {
                Thread.CurrentPrincipal = value;
            }
        }
    }
}

