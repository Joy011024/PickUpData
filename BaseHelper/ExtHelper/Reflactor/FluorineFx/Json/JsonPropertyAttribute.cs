namespace FluorineFx.Json
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple=false)]
    public sealed class JsonPropertyAttribute : Attribute
    {
        private string _propertyName;

        public JsonPropertyAttribute(string propertyName)
        {
            this._propertyName = propertyName;
        }

        public string PropertyName
        {
            get
            {
                return this._propertyName;
            }
            set
            {
                this._propertyName = value;
            }
        }
    }
}

