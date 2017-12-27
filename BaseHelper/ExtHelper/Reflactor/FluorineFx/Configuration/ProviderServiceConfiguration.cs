namespace FluorineFx.Configuration
{
    using FluorineFx.Messaging.Rtmp.Stream;
    using System;
    using System.Xml.Serialization;

    public class ProviderServiceConfiguration : ConfigurationSection
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
                return typeof(ProviderService).FullName;
            }
            set
            {
                this._type = value;
            }
        }
    }
}

