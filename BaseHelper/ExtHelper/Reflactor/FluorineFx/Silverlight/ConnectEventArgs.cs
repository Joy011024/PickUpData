namespace FluorineFx.Silverlight
{
    using System;
    using System.Net;

    public class ConnectEventArgs : EventArgs
    {
        private System.Net.EndPoint _endPoint;

        internal ConnectEventArgs(System.Net.EndPoint endPoint)
        {
            this._endPoint = endPoint;
        }

        public System.Net.EndPoint EndPoint
        {
            get
            {
                return this._endPoint;
            }
        }
    }
}

