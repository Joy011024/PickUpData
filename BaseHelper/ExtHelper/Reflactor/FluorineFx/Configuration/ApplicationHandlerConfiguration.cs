namespace FluorineFx.Configuration
{
    using FluorineFx.Messaging.Adapter;
    using System;
    using System.Xml.Serialization;

    public class ApplicationHandlerConfiguration : ConfigurationSection
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
                return typeof(ApplicationAdapter).FullName;
            }
            set
            {
                this._type = value;
            }
        }
    }
}

