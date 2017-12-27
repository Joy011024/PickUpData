namespace FluorineFx.Messaging.Adapter
{
    using FluorineFx;
    using FluorineFx.Collections;
    using FluorineFx.Configuration;
    using FluorineFx.Context;
    using FluorineFx.Exceptions;
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Service;
    using FluorineFx.Messaging.Api.SO;
    using FluorineFx.Messaging.Api.Stream;
    using FluorineFx.Messaging.Rtmp.Stream;
    using FluorineFx.Messaging.Server;
    using log4net;
    using System;
    using System.Collections;

    public class ApplicationAdapter : StatefulScopeWrappingAdapter, ISharedObjectService, ISharedObjectSecurityService, IStreamSecurityService, IScopeService, IService
    {
        private ArrayList _listeners = new ArrayList();
        private CopyOnWriteArray _playbackSecurityHandlers = new CopyOnWriteArray();
        private CopyOnWriteArray _publishSecurityHandlers = new CopyOnWriteArray();
        private CopyOnWriteArray _sharedObjectSecurityHandlers = new CopyOnWriteArray();
        private static readonly ILog log = LogManager.GetLogger(typeof(ApplicationAdapter));

        public void AddListener(IApplication listener)
        {
            this._listeners.Add(listener);
        }

        public virtual bool AppConnect(IConnection connection, object[] parameters)
        {
            log.Debug(__Res.GetString("AppAdapter_AppConnect", new object[] { connection.ToString() }));
            foreach (IApplication application in this._listeners)
            {
                application.OnAppConnect(connection, parameters);
            }
            return true;
        }

        public virtual void AppDisconnect(IConnection connection)
        {
            log.Debug(__Res.GetString("AppAdapter_AppDisconnect", new object[] { connection.ToString() }));
            foreach (IApplication application in this._listeners)
            {
                application.OnAppDisconnect(connection);
            }
        }

        public virtual bool AppJoin(IClient client, IScope application)
        {
            log.Debug(__Res.GetString("AppAdapter_AppJoin", new object[] { client.ToString(), application.ToString() }));
            foreach (IApplication application2 in this._listeners)
            {
                application2.OnAppJoin(client, application);
            }
            return true;
        }

        public virtual void AppLeave(IClient client, IScope application)
        {
            log.Debug(__Res.GetString("AppAdapter_AppLeave", new object[] { client.ToString(), application.ToString() }));
            foreach (IApplication application2 in this._listeners)
            {
                application2.OnAppLeave(client, application);
            }
        }

        public virtual bool AppStart(IScope application)
        {
            log.Debug(__Res.GetString("AppAdapter_AppStart", new object[] { application.ToString() }));
            foreach (IApplication application2 in this._listeners)
            {
                application2.OnAppStart(application);
            }
            return true;
        }

        public virtual void AppStop(IScope application)
        {
            log.Debug(__Res.GetString("AppAdapter_AppStop", new object[] { application.ToString() }));
            foreach (IApplication application2 in this._listeners)
            {
                application2.OnAppStop(application);
            }
        }

        protected virtual void CalculateClientBw()
        {
            this.CalculateClientBw(FluorineContext.Current.Connection);
        }

        protected virtual void CalculateClientBw(IConnection client)
        {
            new BWCheck(client).CalculateClientBw();
        }

        public bool ClearSharedObjects(IScope scope, string name)
        {
            ISharedObjectService scopeService = (ISharedObjectService) ScopeUtils.GetScopeService(scope, typeof(ISharedObjectService));
            return scopeService.ClearSharedObjects(scope, name);
        }

        public override bool Connect(IConnection connection, IScope scope, object[] parameters)
        {
            if (!base.Connect(connection, scope, parameters))
            {
                return false;
            }
            bool flag = false;
            if (ScopeUtils.IsApplication(scope))
            {
                flag = this.AppConnect(connection, parameters);
            }
            else if (ScopeUtils.IsRoom(scope))
            {
                flag = this.RoomConnect(connection, parameters);
            }
            return flag;
        }

        public bool CreateSharedObject(IScope scope, string name, bool persistent)
        {
            ISharedObjectService scopeService = (ISharedObjectService) ScopeUtils.GetScopeService(scope, typeof(ISharedObjectService));
            return scopeService.CreateSharedObject(scope, name, persistent);
        }

        public override void Disconnect(IConnection connection, IScope scope)
        {
            if (ScopeUtils.IsApplication(scope))
            {
                this.AppDisconnect(connection);
            }
            else if (ScopeUtils.IsRoom(scope))
            {
                this.RoomDisconnect(connection);
            }
            base.Disconnect(connection, scope);
        }

        public IEnumerator GetBroadcastStreamNames(IScope scope)
        {
            IProviderService scopeService = ScopeUtils.GetScopeService(scope, typeof(IProviderService)) as IProviderService;
            return scopeService.GetBroadcastStreamNames(scope);
        }

        public ISharedObject GetSharedObject(IScope scope, string name)
        {
            ISharedObjectService scopeService = (ISharedObjectService) ScopeUtils.GetScopeService(scope, typeof(ISharedObjectService));
            return scopeService.GetSharedObject(scope, name);
        }

        public ISharedObject GetSharedObject(IScope scope, string name, bool persistent)
        {
            ISharedObjectService scopeService = (ISharedObjectService) ScopeUtils.GetScopeService(scope, typeof(ISharedObjectService));
            return scopeService.GetSharedObject(scope, name, persistent);
        }

        public ICollection GetSharedObjectNames(IScope scope)
        {
            ISharedObjectService scopeService = (ISharedObjectService) ScopeUtils.GetScopeService(scope, typeof(ISharedObjectService));
            return scopeService.GetSharedObjectNames(scope);
        }

        public IEnumerator GetSharedObjectSecurity()
        {
            return this._sharedObjectSecurityHandlers.GetEnumerator();
        }

        public IEnumerator GetStreamPlaybackSecurity()
        {
            return this._playbackSecurityHandlers.GetEnumerator();
        }

        public IEnumerator GetStreamPublishSecurity()
        {
            return this._publishSecurityHandlers.GetEnumerator();
        }

        public bool HasSharedObject(IScope scope, string name)
        {
            ISharedObjectService scopeService = (ISharedObjectService) ScopeUtils.GetScopeService(scope, typeof(ISharedObjectService));
            return scopeService.HasSharedObject(scope, name);
        }

        protected void InvokeClients(string method, object[] arguments, IPendingServiceCallback callback)
        {
            this.InvokeClients(method, arguments, callback, false);
        }

        protected void InvokeClients(string method, object[] arguments, IPendingServiceCallback callback, bool ignoreSelf)
        {
            this.InvokeClients(method, arguments, callback, ignoreSelf, base.Scope);
        }

        protected void InvokeClients(string method, object[] arguments, IPendingServiceCallback callback, bool ignoreSelf, IScope targetScope)
        {
            IServiceCapableConnection connection = FluorineContext.Current.Connection as IServiceCapableConnection;
            IEnumerator connections = targetScope.GetConnections();
            while (connections.MoveNext())
            {
                IConnection current = connections.Current as IConnection;
                if (current.Scope.Name.Equals(targetScope.Name) && ((current is IServiceCapableConnection) && (!ignoreSelf || (current != connection))))
                {
                    (current as IServiceCapableConnection).Invoke(method, arguments, callback);
                }
            }
        }

        public override bool Join(IClient client, IScope scope)
        {
            if (!base.Join(client, scope))
            {
                return false;
            }
            if (ScopeUtils.IsApplication(scope))
            {
                return this.AppJoin(client, scope);
            }
            return (ScopeUtils.IsRoom(scope) && this.RoomJoin(client, scope));
        }

        public override void Leave(IClient client, IScope scope)
        {
            if (ScopeUtils.IsApplication(scope))
            {
                this.AppLeave(client, scope);
            }
            else if (ScopeUtils.IsRoom(scope))
            {
                this.RoomLeave(client, scope);
            }
            base.Leave(client, scope);
        }

        public void RegisterSharedObjectSecurity(ISharedObjectSecurity handler)
        {
            this._sharedObjectSecurityHandlers.Add(handler);
        }

        public void RegisterStreamPlaybackSecurity(IStreamPlaybackSecurity handler)
        {
            this._playbackSecurityHandlers.Add(handler);
        }

        public void RegisterStreamPublishSecurity(IStreamPublishSecurity handler)
        {
            this._publishSecurityHandlers.Add(handler);
        }

        protected void RejectClient()
        {
            throw new ClientRejectedException(null);
        }

        protected void RejectClient(object reason)
        {
            throw new ClientRejectedException(reason);
        }

        public void RemoveListener(IApplication listener)
        {
            this._listeners.Remove(listener);
        }

        public virtual bool RoomConnect(IConnection connection, object[] parameters)
        {
            log.Debug(__Res.GetString("AppAdapter_RoomConnect", new object[] { connection.ToString() }));
            foreach (IApplication application in this._listeners)
            {
                application.OnRoomConnect(connection, parameters);
            }
            return true;
        }

        public virtual void RoomDisconnect(IConnection connection)
        {
            log.Debug(__Res.GetString("AppAdapter_RoomDisconnect", new object[] { connection.ToString() }));
            foreach (IApplication application in this._listeners)
            {
                application.OnRoomDisconnect(connection);
            }
        }

        public virtual bool RoomJoin(IClient client, IScope room)
        {
            log.Debug(__Res.GetString("AppAdapter_RoomJoin", new object[] { client.ToString(), room.ToString() }));
            foreach (IApplication application in this._listeners)
            {
                application.OnRoomJoin(client, room);
            }
            return true;
        }

        public virtual void RoomLeave(IClient client, IScope room)
        {
            log.Debug(__Res.GetString("AppAdapter_RoomLeave", new object[] { client.ToString(), room.ToString() }));
            foreach (IApplication application in this._listeners)
            {
                application.OnRoomLeave(client, room);
            }
        }

        public virtual bool RoomStart(IScope room)
        {
            log.Debug(__Res.GetString("AppAdapter_RoomStart", new object[] { room.ToString() }));
            foreach (IApplication application in this._listeners)
            {
                application.OnRoomStart(room);
            }
            return true;
        }

        public virtual void RoomStop(IScope room)
        {
            log.Debug(__Res.GetString("AppAdapter_RoomStop", new object[] { room.ToString() }));
            foreach (IApplication application in this._listeners)
            {
                application.OnRoomStop(room);
            }
        }

        public void Shutdown()
        {
        }

        public void Start(ConfigurationSection configuration)
        {
        }

        public override bool Start(IScope scope)
        {
            if (!base.Start(scope))
            {
                return false;
            }
            if (ScopeUtils.IsApplication(scope))
            {
                return this.AppStart(scope);
            }
            return (ScopeUtils.IsRoom(scope) && this.RoomStart(scope));
        }

        public override void Stop(IScope scope)
        {
            if (ScopeUtils.IsApplication(scope))
            {
                this.AppStop(scope);
            }
            else if (ScopeUtils.IsRoom(scope))
            {
                this.RoomStop(scope);
            }
            base.Stop(scope);
        }

        public void UnregisterSharedObjectSecurity(ISharedObjectSecurity handler)
        {
            this._sharedObjectSecurityHandlers.Remove(handler);
        }

        public void UnregisterStreamPlaybackSecurity(IStreamPlaybackSecurity handler)
        {
            this._playbackSecurityHandlers.Remove(handler);
        }

        public void UnregisterStreamPublishSecurity(IStreamPublishSecurity handler)
        {
            this._publishSecurityHandlers.Remove(handler);
        }
    }
}

