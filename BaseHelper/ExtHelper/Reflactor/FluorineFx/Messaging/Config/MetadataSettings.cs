namespace FluorineFx.Messaging.Config
{
    using System;
    using System.Collections;
    using System.Xml;

    public sealed class MetadataSettings : Hashtable
    {
        private ArrayList _associations;
        private ArrayList _identity;

        private MetadataSettings()
        {
            this._identity = new ArrayList();
            this._associations = new ArrayList();
        }

        internal MetadataSettings(XmlNode metadataDefinitionNode)
        {
            this._identity = new ArrayList();
            this._associations = new ArrayList();
            foreach (XmlNode node in metadataDefinitionNode.SelectNodes("*"))
            {
                if ((node.InnerXml != null) && (node.InnerXml != string.Empty))
                {
                    this[node.Name] = node.InnerXml;
                }
                else
                {
                    if (node.Name == "identity")
                    {
                        foreach (XmlAttribute attribute in node.Attributes)
                        {
                            this._identity.Add(attribute.Value);
                        }
                    }
                    if (node.Name == "many-to-one")
                    {
                        Hashtable hashtable = new Hashtable(3);
                        foreach (XmlAttribute attribute in node.Attributes)
                        {
                            hashtable[attribute.Name] = attribute.Value;
                        }
                        this._associations.Add(hashtable);
                    }
                }
            }
        }

        public ArrayList Identity
        {
            get
            {
                return this._identity;
            }
        }
    }
}

