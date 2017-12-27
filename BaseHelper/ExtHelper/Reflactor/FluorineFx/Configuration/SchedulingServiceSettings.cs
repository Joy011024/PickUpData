namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public class SchedulingServiceSettings
    {
        private string _type = typeof(SchedulingService).FullName;

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

