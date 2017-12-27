namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public class SharedObjectSecurityServiceConfiguration : ConfigurationSection
    {
        private string _type;

        [XmlAttribute("type")]
        public string Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }
    }
}

