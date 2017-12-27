namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public class StreamableFileFactorySettings
    {
        private string _type = typeof(StreamableFileFactory).FullName;

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

