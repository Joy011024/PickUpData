namespace FluorineFx.Net
{
    using FluorineFx.Messaging.Api;
    using FluorineFx.Messaging.Api.Service;
    using FluorineFx.Messaging.Rtmp.Event;
    using System;

    internal interface INetConnectionClient
    {
        void Call(string command, IPendingServiceCallback callback, params object[] arguments);
        void Call(string endpoint, string destination, string source, string operation, IPendingServiceCallback callback, params object[] arguments);
        void Close();
        void Connect(string command, params object[] arguments);
        void Write(IRtmpEvent message);

        bool Connected { get; }

        IConnection Connection { get; }
    }
}

