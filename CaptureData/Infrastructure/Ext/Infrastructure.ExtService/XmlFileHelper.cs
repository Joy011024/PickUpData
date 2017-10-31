using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Infrastructure.ExtService
{
    public class XmlFileHelper
    {
        /// <summary>
        /// 读取节点下各项属性内容 如：  <add key="key" value="value"/>
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> ReadXmlNodeItemInAttribute(string xmlFile, string node)
        {
            if (string.IsNullOrEmpty(xmlFile))
            {
                return null;
            }
            if (!File.Exists(xmlFile))
            {
                return null;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFile);
            XmlNode xn = doc.SelectSingleNode(node);
            if (xn == null)
            {
                return null;
            }
            //通过提供的节点找不到任何子节点数据
            XmlNodeList xnl = xn.ChildNodes;
            List<Dictionary<string, string>> nodes = new List<Dictionary<string, string>>();
            foreach (XmlNode item in xnl)
            {
                if (item.NodeType == XmlNodeType.Comment) { continue; }//忽略注释列
                Dictionary<string, string> nodeItem = new Dictionary<string, string>();
                XmlAttributeCollection attrs = item.Attributes;
                foreach (XmlAttribute attr in attrs)
                {
                    nodeItem.Add(attr.Name, attr.Value);
                }
                nodes.Add(nodeItem);
            }
            return nodes;
        }
        /// <summary>
        /// 读取xml文件下配置节点的文本  如:<key>value</key>
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ReadXmlNodeItemInText(string xmlFile, string node)
        {
            if (string.IsNullOrEmpty(xmlFile))
            {
                return null;
            }
            if (!File.Exists(xmlFile))
            {
                return null;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFile);
            XmlNode xn = doc.SelectSingleNode(node);
            if (xn == null)
            {
                return null;
            }
            //通过提供的节点找不到任何子节点数据
            XmlNodeList xnl = xn.ChildNodes;
            Dictionary<string, string> entity = new Dictionary<string, string>();
            foreach (XmlNode item in xnl)
            {
                if (item.NodeType == XmlNodeType.Comment) { continue; }//忽略注释列
                entity.Add(item.Name, item.InnerText);
               // string item.Name
               // nodes.Add(nodeItem);
            }
            return entity;
        }
    }
}
