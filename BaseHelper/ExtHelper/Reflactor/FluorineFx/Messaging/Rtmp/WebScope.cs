namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx;
    using FluorineFx.Configuration;
    using FluorineFx.Messaging;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.SO;
    using FluorineFx.Messaging.Api.Stream;
    using FluorineFx.Messaging.Endpoints;
    using FluorineFx.Messaging.Rtmp.Stream;
    using System;
    using System.Collections;

    internal class WebScope : Scope
    {
        private ApplicationConfiguration _appConfig;
        protected string _contextPath;
        private RtmpEndpoint _endpoint;
        protected bool _isRegistered;
        protected bool _isShuttingDown;

        internal WebScope(RtmpEndpoint endpoint, IGlobalScope globalScope, ApplicationConfiguration appConfig) : base(null, globalScope.ServiceProvider)
        {
            this._endpoint = endpoint;
            this._appConfig = appConfig;
            base.Parent = globalScope;
        }

        public void Register()
        {
            lock (base.SyncRoot)
            {
                if (!this._isRegistered)
                {
                    IStreamFilenameGenerator generator = ObjectFactory.CreateInstance(this._appConfig.StreamFilenameGenerator.Type) as IStreamFilenameGenerator;
                    base.AddService(typeof(IStreamFilenameGenerator), generator, false);
                    generator.Start(this._appConfig.StreamFilenameGenerator);
                    ISharedObjectService service = ObjectFactory.CreateInstance(this._appConfig.SharedObjectServiceConfiguration.Type) as ISharedObjectService;
                    base.AddService(typeof(ISharedObjectService), service, false);
                    service.Start(this._appConfig.SharedObjectServiceConfiguration);
                    IProviderService service2 = ObjectFactory.CreateInstance(this._appConfig.ProviderServiceConfiguration.Type) as IProviderService;
                    base.AddService(typeof(IProviderService), service2, false);
                    service2.Start(this._appConfig.ProviderServiceConfiguration);
                    IConsumerService service3 = ObjectFactory.CreateInstance(this._appConfig.ConsumerServiceConfiguration.Type) as IConsumerService;
                    base.AddService(typeof(IConsumerService), service3, false);
                    service3.Start(this._appConfig.ConsumerServiceConfiguration);
                    IStreamService service4 = ObjectFactory.CreateInstance(this._appConfig.StreamService.Type) as IStreamService;
                    base.AddService(typeof(IStreamService), service4, false);
                    service4.Start(this._appConfig.StreamService);
                    if (this._appConfig.SharedObjectSecurityService.Type != null)
                    {
                        ISharedObjectSecurityService service5 = ObjectFactory.CreateInstance(this._appConfig.SharedObjectSecurityService.Type) as ISharedObjectSecurityService;
                        base.AddService(typeof(ISharedObjectSecurityService), service5, false);
                        service5.Start(this._appConfig.SharedObjectSecurityService);
                    }
                    base.Init();
                    base._keepOnDisconnect = true;
                    this._isRegistered = true;
                }
            }
        }

        public void SetContextPath(string contextPath)
        {
            this._contextPath = contextPath;
            base.Name = this._contextPath.Substring(1);
        }

        public void Unregister()
        {
            lock (base.SyncRoot)
            {
                if (this._isRegistered)
                {
                    this._isShuttingDown = true;
                    base._keepOnDisconnect = false;
                    base.Uninit();
                    IEnumerator connections = base.GetConnections();
                    while (connections.MoveNext())
                    {
                        (connections.Current as IConnection).Close();
                    }
                    base.Parent = null;
                    this._isRegistered = false;
                    this._isShuttingDown = false;
                }
            }
        }

        public override string ContextPath
        {
            get
            {
                return this._contextPath;
            }
        }

        public bool IsShuttingDown
        {
            get
            {
                return this._isShuttingDown;
            }
        }

        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                throw new InvalidOperationException("Cannot set name, you must set context path");
            }
        }

        public override IScope Parent
        {
            get
            {
                return base.Parent;
            }
            set
            {
                throw new InvalidOperationException("Cannot set parent, you must set global scope");
            }
        }
    }
}

