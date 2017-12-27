namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    [XmlType(TypeName="service")]
    public sealed class ServiceConfiguration
    {
        private string _name;
        private RemoteMethodCollection _remoteMethodCollection;
        private string _serviceLocation;

        [XmlArray("methods"), XmlArrayItem("remote-method", typeof(RemoteMethod))]
        public RemoteMethodCollection Methods
        {
            get
            {
                if (this._remoteMethodCollection == null)
                {
                    this._remoteMethodCollection = new RemoteMethodCollection();
                }
                return this._remoteMethodCollection;
            }
        }

        [XmlElement(DataType="string", ElementName="name")]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [XmlElement(DataType="string", ElementName="service-location")]
        public string ServiceLocation
        {
            get
            {
                return this._serviceLocation;
            }
            set
            {
                this._serviceLocation = value;
            }
        }
    }
}

