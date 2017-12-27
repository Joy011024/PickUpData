namespace FluorineFx.Messaging.Config
{
    using System;
    using System.Collections;
    using System.Xml;

    public sealed class ServerSettings : Hashtable
    {
        internal ServerSettings()
        {
        }

        internal ServerSettings(XmlNode severDefinitionNode)
        {
            foreach (XmlNode node in severDefinitionNode.SelectNodes("*"))
            {
                if ((node.InnerXml != null) && (node.InnerXml != string.Empty))
                {
                    this[node.Name] = node.InnerXml;
                }
                else if (node.Attributes != null)
                {
                    foreach (XmlAttribute attribute in node.Attributes)
                    {
                        this[node.Name + "_" + attribute.Name] = attribute.Value;
                    }
                }
            }
        }

        public bool AllowSubtopics
        {
            get
            {
                return (this.Contains("allow-subtopics") && ((bool) this["allow-subtopics"]));
            }
        }
    }
}

