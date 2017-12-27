namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class SilverlightSettings
    {
        private FluorineFx.Configuration.PolicyServerSettings _policyServerSettings;

        [XmlElement(ElementName="policyServer")]
        public FluorineFx.Configuration.PolicyServerSettings PolicyServerSettings
        {
            get
            {
                if (this._policyServerSettings == null)
                {
                    this._policyServerSettings = new FluorineFx.Configuration.PolicyServerSettings();
                }
                return this._policyServerSettings;
            }
            set
            {
                this._policyServerSettings = value;
            }
        }
    }
}

