namespace FluorineFx.Messaging.Config
{
    using System;
    using System.Collections;
    using System.Xml;

    public sealed class MsmqSettings : Hashtable
    {
        public const string BinaryMessageFormatter = "BinaryMessageFormatter";
        public const string XmlMessageFormatter = "XmlMessageFormatter";

        private MsmqSettings()
        {
        }

        internal MsmqSettings(XmlNode msmqDefinitionNode)
        {
            foreach (XmlNode node in msmqDefinitionNode.SelectNodes("*"))
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

        public string Formatter
        {
            get
            {
                if (this.ContainsKey("formatter"))
                {
                    return (this["formatter"] as string);
                }
                return null;
            }
        }

        public string Label
        {
            get
            {
                if (this.ContainsKey("label"))
                {
                    return (this["label"] as string);
                }
                return null;
            }
        }

        public string Name
        {
            get
            {
                if (this.ContainsKey("name"))
                {
                    return (this["name"] as string);
                }
                return null;
            }
        }
    }
}

