namespace FluorineFx.Messaging.Rtmp
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Event;
    using FluorineFx.Messaging.Api.Service;
    using System;

    internal class CoreHandler : IScopeHandler, IEventHandler
    {
        public bool AddChildScope(IBasicScope scope)
        {
            return true;
        }

        public bool Connect(IConnection connection, IScope scope, object[] parameters)
        {
            IScope scope2 = connection.Scope;
            IClient client = null;
            if (connection.IsFlexClient && ((parameters != null) && (parameters.Length > 2)))
            {
                string id = parameters[1] as string;
                client = scope2.Context.ClientRegistry.GetClient(id);
                connection.Initialize(client);
                client.Renew(connection.ClientLeaseTime);
                return true;
            }
            string connectionId = connection.ConnectionId;
            client = scope2.Context.ClientRegistry.GetClient(connectionId);
            connection.Initialize(client);
            client.Renew(connection.ClientLeaseTime);
            return true;
        }

        public void Disconnect(IConnection connection, IScope scope)
        {
        }

        public bool HandleEvent(IEvent evt)
        {
            return false;
        }

        public bool Join(IClient client, IScope scope)
        {
            return true;
        }

        public void Leave(IClient client, IScope scope)
        {
        }

        public void RemoveChildScope(IBasicScope scope)
        {
        }

        public bool ServiceCall(IConnection connection, IServiceCall call)
        {
            IScopeContext service = connection.Scope.Context;
            if (call.ServiceName != null)
            {
                service.ServiceInvoker.Invoke(call, service);
            }
            else
            {
                service.ServiceInvoker.Invoke(call, connection.Scope.Handler);
            }
            return true;
        }

        public bool Start(IScope scope)
        {
            return true;
        }

        public void Stop(IScope scope)
        {
        }
    }
}

