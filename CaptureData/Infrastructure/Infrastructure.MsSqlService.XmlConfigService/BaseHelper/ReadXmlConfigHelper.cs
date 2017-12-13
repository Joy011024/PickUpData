using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.ExtService;
namespace Infrastructure.MsSqlService.XmlConfigService
{
    public abstract class ReadXmlConfigHelper
    {
        public string GetXml( string xmlFile, string sqlNodeKeyName, string sqlNodeValueName, string nodeSqlKey)
        {//读取xml中节点数据
            return XmlFileHelper.ReadXmlItemValue(xmlFile, sqlNodeKeyName, sqlNodeKeyName, sqlNodeValueName, nodeSqlKey);
        }
    }
}
