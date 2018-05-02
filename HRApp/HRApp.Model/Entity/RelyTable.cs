using Domain.CommonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRApp.Model
{
    public class RelyTable : IntPrimaryKey
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string Note { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsDelete { get; set; }
        public string GetInitDBRelactiveSql() 
        {
            return @"insert   [dbo].[RelyTable] (TableName,ColumnName,CreateTime,IsDelete,IsPrimaryKey)
select tab.name, cell.name,GETDATE(),0,0 from sys.tables  tab ,sys.columns  cell
where  cell.object_id=object_id(tab.name) and tab.name not in (select TableName from RelyTable)
and cell.name not in  (select  ColumnName from RelyTable)";
        }
        /// <summary>
        /// 简单的查询语句
        /// </summary>
        /// <returns></returns>
        public string GetSampleQuerySql() 
        {
            return @"select TableName,ColumnName,CreateTime,IsDelete,IsPrimaryKey from [dbo].[RelyTable] ";
        }
    }
}
