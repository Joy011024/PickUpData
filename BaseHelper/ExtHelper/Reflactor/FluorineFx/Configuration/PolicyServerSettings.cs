namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class PolicyServerSettings
    {
        private bool _enable = false;
        private string _policyFile = "clientaccesspolicy.xml";

        [XmlAttribute(DataType="boolean", AttributeName="enable")]
        public bool Enable
        {
            get
            {
                return this._enable;
            }
            set
            {
                this._enable = value;
            }
        }

        [XmlAttribute(DataType="string", AttributeName="policyFile")]
        public string PolicyFile
        {
            get
            {
                return this._policyFile;
            }
            set
            {
                this._policyFile = value;
            }
        }
    }
}

