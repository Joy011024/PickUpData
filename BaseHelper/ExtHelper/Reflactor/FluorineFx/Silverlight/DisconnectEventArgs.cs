namespace FluorineFx.Silverlight
{
    using System;
    using System.Net;

    public class DisconnectEventArgs : EventArgs
    {
        private System.Net.EndPoint _endPoint;

        internal DisconnectEventArgs(System.Net.EndPoint endPoint)
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

