using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace MsSqlHelper
{
    public  class SqlQueryHelp
    {
        public static string SqlConnString { get; set; }
        public bool ConnSqlServerSuccess { get; set; }
        public SqlQueryHelp(string sqlConnString)
        {
            SqlConnString = sqlConnString;
            IsConnnection(sqlConnString);
        }
        /// <summary>
        /// 数据库是否连接成功
        /// </summary>
        /// <param name="SqlConnString"></param>
        /// <returns></returns>
        public KeyValuePair<bool, string> IsConnnection(string SqlConnString) 
        {
            ConnSqlServerSuccess = false;
            try
            {
                SqlConnection conn = new SqlConnection(SqlConnString);
                conn.Open();
                conn.Close();
                ConnSqlServerSuccess = true;
                return new KeyValuePair<bool, string>(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new KeyValuePair<bool, string>(false, ex.Message);
            }
        }
        public bool IsConnection() 
        {
            KeyValuePair<bool, string> success = IsConnnection(SqlConnString);
           return success.Key;
        }
        public KeyValuePair<bool, string> IsConnectionHavaMessage() 
        {
            return IsConnnection(SqlConnString);
        }
        public DataSet GetUserTableNamesInInitialCatagory() 
        {
            DataSet ds = new DataSet();
            if (!ConnSqlServerSuccess) 
            {
                return ds;
            }
            SqlConnection conn = new SqlConnection(SqlConnString);
            SqlCommand comm = new SqlCommand(ImportantSql.GetUserTablesName, conn);
            SqlDataAdapter da = new SqlDataAdapter(comm);
            da.Fill(ds,conn.Database);
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 获取用户自定义创建的表名称信息集合并以table的形式返回
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByUserTableNameInInitialCatagory() 
        {
            DataTable table = new DataTable();
            if (!ConnSqlServerSuccess)
            {
                return table;
            }
            SqlConnection conn = new SqlConnection(SqlConnString);
            SqlCommand comm = new SqlCommand(ImportantSql.GetUserTablesName, conn);
            SqlDataAdapter da = new SqlDataAdapter(comm);
            da.Fill(table);
            table.TableName = conn.Database;
            conn.Close();
            return table;
        }
        public DataTable QueryBySqlString(string sqlString) 
        {
            DataTable table = new DataTable();
            if (!ConnSqlServerSuccess)
            {
                return table;
            }
            SqlConnection conn = new SqlConnection(SqlConnString);
            SqlCommand comm = new SqlCommand(sqlString, conn);
            SqlDataAdapter da = new SqlDataAdapter(comm);
            da.Fill(table);
            conn.Close();
            return table;
        }
        public DataTable GetTableAllFieldInfo(string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(ImportantSql.GetTargetTableAllFiledInfo, tableName);
            DataTable table = QueryBySqlString(sb.ToString());
            table.TableName = tableName;
            return table;
        }
        public DataTable GetSelectTableSelectFieldData(string tableName,List<string> fieldNames) 
        { 
            if(string.IsNullOrEmpty(tableName)||(fieldNames==null||fieldNames.Count==0))
            {//没有指定表进行操作
                return null;
            }
            StringBuilder sb=new StringBuilder();
            sb.AppendFormat(string.Format(ImportantSql.GetSelectTbaleSelectFields,string.Join(",",fieldNames),tableName));
            DataTable table = QueryBySqlString(sb.ToString());
            table.TableName = tableName;
            return table;
        }
    }
}
