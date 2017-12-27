namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class OptimizerSettings
    {
        private bool _debug = true;
        private string _provider = "codedom";
        private bool _typeCheck = false;

        [XmlAttribute(DataType="boolean", AttributeName="debug")]
        public bool Debug
        {
            get
            {
                return this._debug;
            }
            set
            {
                this._debug = value;
            }
        }

        [XmlAttribute(DataType="string", AttributeName="provider")]
        public string Provider
        {
            get
            {
                return this._provider;
            }
            set
            {
                this._provider = value;
            }
        }

        [XmlAttribute(DataType="boolean", AttributeName="typeCheck")]
        public bool TypeCheck
        {
            get
            {
                return this._typeCheck;
            }
            set
            {
                this._typeCheck = value;
            }
        }
    }
}

