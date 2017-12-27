namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class JsonRpcGenerator
    {
        private string _name;
        private string _type;

        [XmlAttribute(DataType="string", AttributeName="name")]
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

        [XmlAttribute(DataType="string", AttributeName="type")]
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

