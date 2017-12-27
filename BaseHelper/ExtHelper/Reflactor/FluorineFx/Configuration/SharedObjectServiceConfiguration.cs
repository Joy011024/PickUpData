namespace FluorineFx.Configuration
{
    using FluorineFx.Messaging.Rtmp.SO;
    using System;
    using System.Xml.Serialization;

    public class SharedObjectServiceConfiguration : ConfigurationSection
    {
        private PersistenceStoreConfiguration _persistenceStore;
        private string _type;

        [XmlElement("persistenceStore")]
        public PersistenceStoreConfiguration PersistenceStore
        {
            get
            {
                if (this._persistenceStore == null)
                {
                    return (this._persistenceStore = new PersistenceStoreConfiguration());
                }
                return this._persistenceStore;
            }
            set
            {
                this._persistenceStore = value;
            }
        }

        [XmlAttribute("type")]
        public string Type
        {
            get
            {
                if (this._type != null)
                {
                    return this._type;
                }
                return typeof(SharedObjectService).FullName;
            }
            set
            {
                this._type = value;
            }
        }
    }
}

