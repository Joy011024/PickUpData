namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class JSonSettings
    {
        private JsonRpcGeneratorCollection _jsonRpcGeneratorCollection;

        [XmlArrayItem("add", typeof(JsonRpcGenerator)), XmlArray("jsonRpcGenerators")]
        public JsonRpcGeneratorCollection JsonRpcGenerators
        {
            get
            {
                if (this._jsonRpcGeneratorCollection == null)
                {
                    this._jsonRpcGeneratorCollection = new JsonRpcGeneratorCollection();
                }
                return this._jsonRpcGeneratorCollection;
            }
            set
            {
                this._jsonRpcGeneratorCollection = value;
            }
        }
    }
}

