namespace FluorineFx.Configuration
{
    using FluorineFx.Messaging.Rtmp.Stream;
    using System;
    using System.Xml.Serialization;

    public class StreamServiceConfiguration : ConfigurationSection
    {
        private string _type;

        [XmlAttribute("type")]
        public string Type
        {
            get
            {
                if (this._type != null)
                {
                    return this._type;
                }
                return typeof(StreamService).FullName;
            }
            set
            {
                this._type = value;
            }
        }
    }
}

