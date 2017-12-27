namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class RtmpConnectionSettings
    {
        private int _maxHandshakeTimeout = 0x1388;
        private int _maxInactivity = 0xea60;
        private int _pingInterval = 0x1388;

        [XmlAttribute(DataType="int", AttributeName="maxHandshakeTimeout")]
        public int MaxHandshakeTimeout
        {
            get
            {
                return this._maxHandshakeTimeout;
            }
            set
            {
                this._maxHandshakeTimeout = value;
            }
        }

        [XmlAttribute(DataType="int", AttributeName="maxInactivity")]
        public int MaxInactivity
        {
            get
            {
                return this._maxInactivity;
            }
            set
            {
                this._maxInactivity = value;
            }
        }

        [XmlAttribute(DataType="int", AttributeName="pingInterval")]
        public int PingInterval
        {
            get
            {
                return this._pingInterval;
            }
            set
            {
                this._pingInterval = value;
            }
        }
    }
}

