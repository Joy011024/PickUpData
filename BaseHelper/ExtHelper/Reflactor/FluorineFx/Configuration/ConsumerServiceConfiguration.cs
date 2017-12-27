namespace FluorineFx.Configuration
{
    using FluorineFx.Messaging.Rtmp.Stream;
    using System;
    using System.Xml.Serialization;

    public class ConsumerServiceConfiguration : ConfigurationSection
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
                return typeof(ConsumerService).FullName;
            }
            set
            {
                this._type = value;
            }
        }
    }
}

