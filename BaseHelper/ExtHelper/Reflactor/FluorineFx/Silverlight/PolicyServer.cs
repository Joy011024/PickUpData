namespace FluorineFx.Silverlight
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class PolicyServer
    {
        private Socket _listener;
        private byte[] _policy;

        private event ConnectHandler _connectHandler;

        private event DisconnectHandler _disconnectHandler;

        public event ConnectHandler Connect
        {
            add
            {
                this._connectHandler += value;
            }
            remove
            {
                this._connectHandler -= value;
            }
        }

        public event DisconnectHandler Disconnect
        {
            add
            {
                this._disconnectHandler += value;
            }
            remove
            {
                this._disconnectHandler -= value;
            }
        }

        public PolicyServer(string policyFile)
        {
            FileStream stream = new FileStream(policyFile, FileMode.Open);
            this._policy = new byte[stream.Length];
            stream.Read(this._policy, 0, this._policy.Length);
            stream.Close();
            this._listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._listener.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, 0);
            this._listener.Bind(new IPEndPoint(IPAddress.Any, 0x3af));
            this._listener.Listen(10);
            this._listener.BeginAccept(new AsyncCallback(this.OnConnection), null);
        }

        public void Close()
        {
            this._listener.Close();
        }

        public void OnConnection(IAsyncResult res)
        {
            Socket client = null;
            try
            {
                client = this._listener.EndAccept(res);
                if (this._connectHandler != null)
                {
                    this._connectHandler(this, new ConnectEventArgs(client.RemoteEndPoint));
                }
            }
            catch (SocketException)
            {
                return;
            }
            PolicyConnection connection = new PolicyConnection(this, client, this._policy);
            this._listener.BeginAccept(new AsyncCallback(this.OnConnection), null);
        }

        internal void RaiseDisconnect(PolicyConnection connection)
        {
            if (this._disconnectHandler != null)
            {
                this._disconnectHandler(this, new DisconnectEventArgs(connection.Socket.RemoteEndPoint));
            }
        }
    }
}

