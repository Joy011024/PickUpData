namespace FluorineFx.Messaging.Config
{
    using System;
    using System.Collections;
    using System.Xml;

    public sealed class FactorySettings : Hashtable
    {
        private string _class;
        private string _id;

        private FactorySettings()
        {
        }

        internal FactorySettings(XmlNode factoryDefinitionNode)
        {
            this._id = factoryDefinitionNode.Attributes["id"].Value;
            this._class = factoryDefinitionNode.Attributes["class"].Value;
            XmlNode node = factoryDefinitionNode.SelectSingleNode("properties");
            if (node != null)
            {
                foreach (XmlNode node2 in node.SelectNodes("*"))
                {
                    this[node2.Name] = node2.InnerXml;
                }
            }
        }

        public string ClassId
        {
            get
            {
                return this._class;
            }
            set
            {
                this._class = value;
            }
        }

        public string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }
    }
}

