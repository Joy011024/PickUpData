namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class RtmpServerSettings
    {
        private FluorineFx.Configuration.RtmpConnectionSettings _rtmpConnectionSettings = new FluorineFx.Configuration.RtmpConnectionSettings();
        private FluorineFx.Configuration.RtmptConnectionSettings _rtmptConnectionSettings = new FluorineFx.Configuration.RtmptConnectionSettings();
        private FluorineFx.Configuration.RtmpTransportSettings _rtmpTransportSettings = new FluorineFx.Configuration.RtmpTransportSettings();
        private FluorineFx.Configuration.ThreadPoolSettings _threadPoolSettings = new FluorineFx.Configuration.ThreadPoolSettings();

        [XmlElement(ElementName="rtmpConnection")]
        public FluorineFx.Configuration.RtmpConnectionSettings RtmpConnectionSettings
        {
            get
            {
                return this._rtmpConnectionSettings;
            }
            set
            {
                this._rtmpConnectionSettings = value;
            }
        }

        [XmlElement(ElementName="rtmptConnection")]
        public FluorineFx.Configuration.RtmptConnectionSettings RtmptConnectionSettings
        {
            get
            {
                return this._rtmptConnectionSettings;
            }
            set
            {
                this._rtmptConnectionSettings = value;
            }
        }

        [XmlElement(ElementName="rtmpTransport")]
        public FluorineFx.Configuration.RtmpTransportSettings RtmpTransportSettings
        {
            get
            {
                return this._rtmpTransportSettings;
            }
            set
            {
                this._rtmpTransportSettings = value;
            }
        }

        [XmlElement(ElementName="threadpool")]
        public FluorineFx.Configuration.ThreadPoolSettings ThreadPoolSettings
        {
            get
            {
                return this._threadPoolSettings;
            }
            set
            {
                this._threadPoolSettings = value;
            }
        }
    }
}

