namespace FluorineFx.Messaging.Config
{
    using System;
    using System.Collections;
    using System.Xml;

    public sealed class AdapterSettings : Hashtable
    {
        private string _class;
        private bool _defaultAdapter;
        private string _id;

        internal AdapterSettings(XmlNode adapterNode)
        {
            this._id = adapterNode.Attributes["id"].Value;
            this._class = adapterNode.Attributes["class"].Value;
            if ((adapterNode.Attributes["default"] != null) && (adapterNode.Attributes["default"].Value == "true"))
            {
                this._defaultAdapter = true;
            }
        }

        internal AdapterSettings(string id, string adapterClass, bool defaultAdapter)
        {
            this._id = id;
            this._class = adapterClass;
            this._defaultAdapter = defaultAdapter;
        }

        public string Class
        {
            get
            {
                return this._class;
            }
        }

        public bool Default
        {
            get
            {
                return this._defaultAdapter;
            }
        }

        public string Id
        {
            get
            {
                return this._id;
            }
        }
    }
}

