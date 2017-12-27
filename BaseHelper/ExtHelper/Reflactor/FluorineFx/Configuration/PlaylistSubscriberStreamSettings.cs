namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class PlaylistSubscriberStreamSettings
    {
        private int _bufferCheckInterval = 0xea60;
        private int _underrunTrigger = 0x1388;

        [XmlAttribute(DataType="int", AttributeName="bufferCheckInterval")]
        public int BufferCheckInterval
        {
            get
            {
                return this._bufferCheckInterval;
            }
            set
            {
                this._bufferCheckInterval = value;
            }
        }

        [XmlAttribute(DataType="int", AttributeName="underrunTrigger")]
        public int UnderrunTrigger
        {
            get
            {
                return this._underrunTrigger;
            }
            set
            {
                this._underrunTrigger = value;
            }
        }
    }
}

