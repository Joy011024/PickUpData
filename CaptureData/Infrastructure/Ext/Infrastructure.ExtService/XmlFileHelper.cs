﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.ComponentModel;
namespace Infrastructure.ExtService
{
    public static class XmlFileHelper
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
        static XmlNode GetXmlNode(string xmlFile, string node) 
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
            return xn;
        }
        /// <summary>
        /// 形如Config文件键值形式读取
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="nodeName"></param>
        /// <param name="itemKeyName"></param>
        /// <param name="itemValueName"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ReadAppsettingSimulateConfig(string xmlFile,string nodeName,string itemKeyName,string itemValueName) 
        {
            XmlNode node = GetXmlNode(xmlFile, nodeName);
            if (node == null) 
            {
                return null;
            }
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.NodeType == XmlNodeType.Comment)
                {
                    continue;
                }
                XmlAttribute attKey= item.Attributes[itemKeyName];
                XmlAttribute attrValue = item.Attributes[itemValueName];
                //如果xml文件节点key重复的处理办法？？
                dict.Add(attKey.Value.Trim(), attrValue.Value.Trim());
            }
            return dict;
        }
        public static string ReadXmlItemValue(string xmlDir,string nodeName,string keyItemName,string valueItemName,string key) 
        {
            XmlNode node = GetXmlNode(xmlDir, nodeName);
            if (node == null)
            {
                return null;
            }
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.NodeType == XmlNodeType.Comment)
                {
                    continue;
                }
                XmlAttribute attKey = item.Attributes[keyItemName];
                if (attKey.Value != key)
                {
                    continue;
                }
                XmlAttribute attValue = item.Attributes[valueItemName];
                return attValue.Value;
            }
            return null;
        }
        /// <summary>
        /// 从xml中读取实体相关信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="xmlFile"></param>
        /// <param name="entityNodeName"></param>
        /// <param name="filterPropertyFormat">是否过滤占位的属性串</param>
        /// <returns></returns>
        public static T GetEntityConfig<T>(this T obj, string xmlFile,string entityNodeName,bool filterPropertyFormat=false) where T:class
        {
            XmlNode doc = GetXmlNode(xmlFile, entityNodeName);
            //获取实体属性列表
            if (doc == null)
            {
                return default(T);
            }
            T entity = System.Activator.CreateInstance<T>();
            Type t=typeof(T);
            foreach (XmlNode item in doc.ChildNodes)
            {
                if (item.NodeType == XmlNodeType.Comment)
                {
                    continue;
                }
                string value = item.InnerText.Trim();
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }
                if (filterPropertyFormat&&value=="{"+item.Name+"}")
                {
                    continue;
                }
                PropertyInfo pi = t.GetProperty(item.Name);
                if (pi == null)
                {
                    continue;
                }
                entity.SetPropertyValue(pi.Name, item.InnerText.Trim());
            }
            return entity;
        }
        public static void UpdateConfigNode(this string xmlFile, Dictionary<string, string> obj, XmlNodeDataAttribute entityBelongNode, XmlNodeDataAttribute dictBelongNode)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFile);
            //查找目标节点
            XmlNode parent= doc.SelectSingleNode(entityBelongNode.NodeName+"[@"+entityBelongNode.NodeKeyName+"='"+entityBelongNode.NodeKeyValue+"']");
            //查找目标
            string nodeFormat = "[@{0}='{1}']";
            //查找一个节点模板
            string nodeTemplate = string.Format("<{0} {1}='{2}' /> <!--{3}-->", dictBelongNode.NodeName, dictBelongNode.NodeKeyName, dictBelongNode.NodeKeyValue,dictBelongNode.NodeRemark);
            foreach (var item in obj)
            {
                XmlNode node = parent.SelectSingleNode(dictBelongNode.NodeName+string.Format(nodeFormat,dictBelongNode.NodeKeyName,item.Key));
                if (node == null)
                { //如果没有这个节点则需要增加
                    XmlNode xm = doc.CreateNode("element", dictBelongNode.NodeName, string.Empty);
                    XmlElement ele = (XmlElement)xm;
                    ele.SetAttribute(dictBelongNode.NodeKeyName, item.Key);
                    ele.SetAttribute(dictBelongNode.NodeKeyValue, item.Value);
                    parent.AppendChild(xm);
                }
                else 
                {
                    XmlNode newNode= node.CloneNode(true);
                    XmlElement ele = (XmlElement)newNode;
                    ele.SetAttribute(dictBelongNode.NodeKeyName, item.Key);
                    ele.SetAttribute(dictBelongNode.NodeKeyValue, item.Value);
                    parent.ReplaceChild(newNode, node);//节点更新
                }
            }
            doc.Save(xmlFile);
        }
       
    }
    public class XmlNodeDataAttribute:Attribute
    {
        [Description("节点名")]
        public string NodeName { get; set; }
        [Description("节点标志")]
        public string NodeKeyName { get; set; }
        [Description("节点标志的值")]
        public string NodeKeyValue { get; set; }
        [Description("节点描述")]
        public string NodeRemark { get; set; }
    }
}
