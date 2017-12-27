namespace FluorineFx.Context
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Security;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Security.Principal;
    using System.Threading;
    using System.Web;

    internal sealed class FluorineRtmpContext : FluorineContext, ISessionState, ICollection, IEnumerable
    {
        private IConnection _connection;
        private Hashtable _items;

        public FluorineRtmpContext(IConnection connection)
        {
            this._connection = connection;
        }

        public void Add(string name, object value)
        {
            this._connection.SetAttribute(name, value);
        }

        public void Clear()
        {
            this._connection.RemoveAttributes();
        }

        public override void ClearPrincipal()
        {
            this._connection.RemoveAttribute("__@fluorineprincipal");
        }

        public void CopyTo(Array array, int index)
        {
            object[] objArray = null;
            this._connection.CopyTo(objArray, index);
            array = objArray;
        }

        internal override string EncryptCredentials(IEndpoint endpoint, IPrincipal principal, string userId, string password)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public IEnumerator GetEnumerator()
        {
            return this._connection.GetAttributeNames().GetEnumerator();
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

        internal static void Initialize(IConnection connection)
        {
            FluorineRtmpContext context = new FluorineRtmpContext(connection);
            WebSafeCallContext.SetData("__@fluorinecontext", context);
        }

        public void Remove(string name)
        {
            this._connection.RemoveAttribute(name);
        }

        public void RemoveAll()
        {
            this.Clear();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public override IPrincipal RestorePrincipal(ILoginCommand loginCommand)
        {
            IPrincipal attribute = this._connection.GetAttribute("__@fluorineprincipal") as IPrincipal;
            if (attribute != null)
            {
                Thread.CurrentPrincipal = attribute;
            }
            return attribute;
        }

        internal override IPrincipal RestorePrincipal(ILoginCommand loginCommand, string key)
        {
            IPrincipal attribute = this._connection.GetAttribute(key) as IPrincipal;
            if (attribute != null)
            {
                Thread.CurrentPrincipal = attribute;
            }
            return attribute;
        }

        internal override void StorePrincipal(IPrincipal principal, string key)
        {
            this._connection.SetAttribute(key, principal);
        }

        public override void StorePrincipal(IPrincipal principal, string userId, string password)
        {
            this._connection.SetAttribute("__@fluorineprincipal", principal);
        }

        public override string AbsoluteUri
        {
            get
            {
                return null;
            }
        }

        public override string ActivationMode
        {
            get
            {
                return null;
            }
        }

        public override string ApplicationPath
        {
            get
            {
                return null;
            }
        }

        public override IApplicationState ApplicationState
        {
            get
            {
                return RtmpApplicationState.Instance;
            }
        }

        public override IClient Client
        {
            get
            {
                return this._connection.Client;
            }
        }

        public override IConnection Connection
        {
            get
            {
                return this._connection;
            }
        }

        public int Count
        {
            get
            {
                return this._connection.AttributesCount;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return true;
            }
        }

        public object this[string name]
        {
            get
            {
                return this._connection.GetAttribute(name);
            }
            set
            {
                this._connection.SetAttribute(name, value);
            }
        }

        public object this[int index]
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
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
                return HttpRuntime.AppDomainAppPath;
            }
        }

        public override string RequestApplicationPath
        {
            get
            {
                return null;
            }
        }

        public override string RequestPath
        {
            get
            {
                return null;
            }
        }

        public override string RootPath
        {
            get
            {
                return HttpRuntime.AppDomainAppPath;
            }
        }

        public override ISessionState Session
        {
            get
            {
                return this;
            }
        }

        public string SessionID
        {
            get
            {
                return this._connection.ConnectionId;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this._connection.SyncRoot;
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

