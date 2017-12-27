namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    [XmlType(TypeName="login-command")]
    public sealed class LoginCommandSettings
    {
        private string _server;
        private string _type;
        public const string FluorineLoginCommand = "asp.net";

        [XmlAttribute(DataType="string", AttributeName="server")]
        public string Server
        {
            get
            {
                return this._server;
            }
            set
            {
                this._server = value;
            }
        }

        [XmlAttribute(DataType="string", AttributeName="class")]
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

