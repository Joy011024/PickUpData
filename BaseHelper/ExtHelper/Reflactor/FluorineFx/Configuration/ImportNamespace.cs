namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class ImportNamespace
    {
        private string _assembly;
        private string _namespace;

        [XmlAttribute(DataType="string", AttributeName="assembly")]
        public string Assembly
        {
            get
            {
                return this._assembly;
            }
            set
            {
                this._assembly = value;
            }
        }

        [XmlAttribute(DataType="string", AttributeName="namespace")]
        public string Namespace
        {
            get
            {
                return this._namespace;
            }
            set
            {
                this._namespace = value;
            }
        }
    }
}

