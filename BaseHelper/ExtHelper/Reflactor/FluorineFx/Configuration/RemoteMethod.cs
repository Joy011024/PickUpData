namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class RemoteMethod
    {
        private string _method;
        private string _name;

        [XmlElement(DataType="string", ElementName="method")]
        public string Method
        {
            get
            {
                return this._method;
            }
            set
            {
                this._method = value;
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
    }
}

