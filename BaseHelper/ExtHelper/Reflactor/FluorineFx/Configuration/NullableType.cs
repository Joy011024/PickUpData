namespace FluorineFx.Configuration
{
    using System;
    using System.Reflection;
    using System.Xml.Serialization;

    [XmlType(TypeName="type")]
    public sealed class NullableType
    {
        private object _nullValue;
        private string _typeName;
        private string _value;

        private void Init()
        {
            if ((this._typeName != null) && (this._value != null))
            {
                Type conversionType = Type.GetType(this._typeName);
                FieldInfo field = conversionType.GetField(this._value, BindingFlags.Public | BindingFlags.Static);
                if (field != null)
                {
                    this._nullValue = field.GetValue(null);
                }
                else
                {
                    this._nullValue = Convert.ChangeType(this._value, conversionType, null);
                }
            }
        }

        [XmlIgnore]
        public object NullValue
        {
            get
            {
                return this._nullValue;
            }
        }

        [XmlAttribute(DataType="string", AttributeName="name")]
        public string TypeName
        {
            get
            {
                return this._typeName;
            }
            set
            {
                this._typeName = value;
                this.Init();
            }
        }

        [XmlAttribute(DataType="string", AttributeName="value")]
        public string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
                this.Init();
            }
        }
    }
}

