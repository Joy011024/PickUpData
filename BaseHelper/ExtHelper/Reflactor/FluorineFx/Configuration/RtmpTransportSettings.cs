namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class RtmpTransportSettings
    {
        private int _receiveBufferSize = 0x1000;
        private int _sendBufferSize = 0x1000;
        private bool _tcpNoDelay = true;

        [XmlAttribute(DataType="int", AttributeName="receiveBufferSize")]
        public int ReceiveBufferSize
        {
            get
            {
                return this._receiveBufferSize;
            }
            set
            {
                this._receiveBufferSize = value;
            }
        }

        [XmlAttribute(DataType="int", AttributeName="sendBufferSize")]
        public int SendBufferSize
        {
            get
            {
                return this._sendBufferSize;
            }
            set
            {
                this._sendBufferSize = value;
            }
        }

        [XmlAttribute(DataType="boolean", AttributeName="tcpNoDelay")]
        public bool TcpNoDelay
        {
            get
            {
                return this._tcpNoDelay;
            }
            set
            {
                this._tcpNoDelay = value;
            }
        }
    }
}

