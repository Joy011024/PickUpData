using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.MsSqlService.XmlConfigService
{
    public class XmlConfigItemService<T> where T:class
    {
        //从xml中读取节点项
        public string XmlPath { get; set; }
        public string SqlNoteItemName { get; set; }
        public string SqlConnectionString { get; set; }
        public XmlConfigItemService(string dbConnString) 
        {
            SqlConnectionString = dbConnString;
        }
        public void Add(T entity,string nodeSqlKey)
        {
            string entityNode = typeof(T).Name;
            //读取xml中节点数据

        }
    }
}
