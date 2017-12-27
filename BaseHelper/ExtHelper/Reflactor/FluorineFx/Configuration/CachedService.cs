namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    [XmlType(TypeName="cachedService")]
    public sealed class CachedService
    {
        private bool _slidingExpiration = false;
        private int _timeout = 30;
        private string _type;

        [XmlAttribute(DataType="boolean", AttributeName="slidingExpiration")]
        public bool SlidingExpiration
        {
            get
            {
                return this._slidingExpiration;
            }
            set
            {
                this._slidingExpiration = value;
            }
        }

        [XmlAttribute(DataType="int", AttributeName="timeout")]
        public int Timeout
        {
            get
            {
                return this._timeout;
            }
            set
            {
                this._timeout = value;
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

