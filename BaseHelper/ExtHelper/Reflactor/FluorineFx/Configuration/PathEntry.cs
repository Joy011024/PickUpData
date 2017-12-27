namespace FluorineFx.Configuration
{
    using System;
    using System.Xml.Serialization;

    public sealed class PathEntry
    {
        private string _path;

        [XmlAttribute(AttributeName="path")]
        public string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                this._path = value;
            }
        }
    }
}

