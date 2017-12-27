namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    [XmlType(TypeName="classMapping")]
    public sealed class ClassMapping
    {
        private string _customClass;
        private string _type;

        [XmlElement(DataType="string", ElementName="customClass")]
        public string CustomClass
        {
            get
            {
                return this._customClass;
            }
            set
            {
                this._customClass = value;
            }
        }

        [XmlElement(DataType="string", ElementName="type")]
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

