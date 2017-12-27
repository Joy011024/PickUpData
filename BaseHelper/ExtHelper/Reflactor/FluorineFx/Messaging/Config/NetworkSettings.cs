namespace FluorineFx.Messaging.Config
{
    using System;
    using System.Collections;
    using System.Xml;

    public sealed class NetworkSettings : Hashtable
    {
        private NetworkSettings()
        {
        }

        internal NetworkSettings(XmlNode networkDefinitionNode)
        {
            foreach (XmlNode node in networkDefinitionNode.SelectNodes("*"))
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

        public bool PagingEnabled
        {
            get
            {
                return (this.ContainsKey("paging_enabled") && Convert.ToBoolean(this["paging_enabled"]));
            }
        }

        public int PagingSize
        {
            get
            {
                if (this.ContainsKey("paging_pageSize"))
                {
                    return Convert.ToInt32(this["paging_pageSize"]);
                }
                return 0;
            }
        }

        public int SessionTimeout
        {
            get
            {
                if (this.ContainsKey("session-timeout"))
                {
                    return Convert.ToInt32(this["session-timeout"]);
                }
                return 20;
            }
        }
    }
}

