namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class SwxSettings
    {
        private bool _allowDomain = true;

        [XmlAttribute(DataType="boolean", AttributeName="allowDomain")]
        public bool AllowDomain
        {
            get
            {
                return this._allowDomain;
            }
            set
            {
                this._allowDomain = value;
            }
        }
    }
}

