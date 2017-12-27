namespace FluorineFx.Messaging.Adapter
{
    using FluorineFx.Messaging.Api;
    using System;

    public interface IApplication
    {
        bool OnAppConnect(IConnection connection, object[] parameters);
        void OnAppDisconnect(IConnection connection);
        bool OnAppJoin(IClient client, IScope application);
        void OnAppLeave(IClient client, IScope application);
        bool OnAppStart(IScope application);
        void OnAppStop(IScope application);
        bool OnRoomConnect(IConnection connection, object[] parameters);
        void OnRoomDisconnect(IConnection connection);
        bool OnRoomJoin(IClient client, IScope room);
        void OnRoomLeave(IClient client, IScope room);
        bool OnRoomStart(IScope room);
        void OnRoomStop(IScope room);
    }
}

