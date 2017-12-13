using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.ExtService;
using System.Data;
using System.Data.SqlClient;
using Infrastructure.MsSqlService.SqlHelper;
namespace Infrastructure.MsSqlService.XmlConfigService
{
    public class XmlConfigItemService<T>:ReadXmlConfigHelper where T:class
    {
        //从xml中读取节点项
        public string XmlPath { get; set; }
        public string SqlNoteItemName { get; set; }
        public string SqlConnectionString { get; set; }
        public XmlConfigItemService(string dbConnString) 
        {
            SqlConnectionString = dbConnString;
        }
        public void  Add(T entity,string xmlFile, string sqlNodeKeyName,string sqlNodeValueName,string nodeSqlKey)
        {
            //读取xml中节点数据
            string sql = GetXml(xmlFile, sqlNodeKeyName, sqlNodeValueName, nodeSqlKey+"/"+typeof(T).Name);
            if (string.IsNullOrEmpty(sql))
            { //没有找到节点
                
            }
            //是否存在指定格式的属性字段
            string[] properties = entity.GetEntityProperties();//获取的指定属性
            List<SqlParameter> ps = new List<SqlParameter>();
            foreach (string item in properties)
            {
                if (!sql.Contains("{" + item + "}"))
                {
                    continue;
                }
                string value = entity.GetPropertyValue(item);//需要判断该字段的数据类型在进行赋值
                //数字或者bit类型才不需要增加 '' 组装值
                Type t = entity.GetPropertyType(item);
                value = string.IsNullOrEmpty(value) ? null : value;
                string pname = "@" + item;
                if (t.Name != typeof(int).Name && t.Name != typeof(bool).Name)
                {
                    sql = sql.Replace("'{" + item + "}'", pname);
                }
                else 
                {
                    sql = sql.Replace("{" + item + "}", pname);
                }
                ps.Add(new SqlParameter(pname, value));
            }
            SqlCmdHelper helper = new SqlCmdHelper();
            helper.SqlConnString = SqlConnectionString;
            int row= helper.ExcuteNoQuery(sql, ps.ToArray());
        }
        public void BatchInsert() 
        {
        
        }
    }
}
