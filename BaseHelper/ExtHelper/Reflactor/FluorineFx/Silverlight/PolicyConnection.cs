namespace FluorineFx.Silverlight
{
    using System;
    using System.Net.Sockets;
    using System.Text;

    internal class PolicyConnection
    {
        private byte[] _buffer;
        private System.Net.Sockets.Socket _connection;
        private byte[] _policy;
        private static string _policyRequestString = "<policy-file-request/>";
        private PolicyServer _policyServer;
        private int _received;

        public PolicyConnection(PolicyServer policyServer, System.Net.Sockets.Socket client, byte[] policy)
        {
            this._policyServer = policyServer;
            this._connection = client;
            this._policy = policy;
            this._buffer = new byte[_policyRequestString.Length];
            this._received = 0;
            try
            {
                this._connection.BeginReceive(this._buffer, 0, _policyRequestString.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), null);
            }
            catch (SocketException)
            {
                this._connection.Close();
            }
        }

        private void OnReceive(IAsyncResult res)
        {
            try
            {
                this._received += this._connection.EndReceive(res);
                if (this._received < _policyRequestString.Length)
                {
                    this._connection.BeginReceive(this._buffer, this._received, _policyRequestString.Length - this._received, SocketFlags.None, new AsyncCallback(this.OnReceive), null);
                }
                else
                {
                    string x = Encoding.UTF8.GetString(this._buffer, 0, this._received);
                    if (StringComparer.InvariantCultureIgnoreCase.Compare(x, _policyRequestString) != 0)
                    {
                        this._connection.Close();
                    }
                    else
                    {
                        this._connection.BeginSend(this._policy, 0, this._policy.Length, SocketFlags.None, new AsyncCallback(this.OnSend), null);
                    }
                }
            }
            catch (SocketException)
            {
                this._connection.Close();
            }
        }

        public void OnSend(IAsyncResult res)
        {
            try
            {
                this._connection.EndSend(res);
            }
            finally
            {
                this._policyServer.RaiseDisconnect(this);
                this._connection.Close();
            }
        }

        public System.Net.Sockets.Socket Socket
        {
            get
            {
                return this._connection;
            }
        }
    }
}

