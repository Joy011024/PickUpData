namespace FluorineFx.Configuration
{
    using FluorineFx.Messaging.Rtmp.Persistence;
    using System;
    using System.Xml.Serialization;

    public class PersistenceStoreConfiguration
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
                return typeof(MemoryStore).FullName;
            }
            set
            {
                this._type = value;
            }
        }
    }
}

