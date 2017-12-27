namespace FluorineFx.Messaging.Rtmp
{
    using System;
    using System.Net.Sockets;

    internal class RtmpNetworkStream : RtmpQueuedWriteStream
    {
        private System.Net.Sockets.Socket _socket;

        public RtmpNetworkStream(System.Net.Sockets.Socket socket) : base(new NetworkStream(socket, false))
        {
            this._socket = socket;
        }

        public override void Close()
        {
            base.Close();
            this._socket.Close();
        }

        public System.Net.Sockets.Socket Socket
        {
            get
            {
                return this._socket;
            }
        }
    }
}

