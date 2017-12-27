namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class MimeTypeEntry
    {
        private string _type;

        [XmlAttribute(AttributeName="type")]
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

