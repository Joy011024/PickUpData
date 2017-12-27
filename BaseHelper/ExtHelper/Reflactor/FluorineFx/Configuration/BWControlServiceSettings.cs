namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public class BWControlServiceSettings
    {
        private int _defaultCapacity = 0x6400000;
        private int _interval = 100;
        private string _type = typeof(DummyBWControlService).FullName;

        [XmlAttribute(DataType="int", AttributeName="defaultCapacity")]
        public int DefaultCapacity
        {
            get
            {
                return this._defaultCapacity;
            }
            set
            {
                this._defaultCapacity = value;
            }
        }

        [XmlAttribute(DataType="int", AttributeName="interval")]
        public int Interval
        {
            get
            {
                return this._interval;
            }
            set
            {
                this._interval = value;
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

