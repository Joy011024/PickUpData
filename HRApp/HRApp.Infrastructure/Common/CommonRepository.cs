using Infrastructure.MsSqlService.SqlHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonHelperEntity;
using System.ComponentModel;
namespace HRApp.Infrastructure
{
    public class CommonRepository
    {
        /// <summary>
        /// 扩展拼接SQL语句函数
        /// </summary>
        /// <typeparam name="T">提供数据的实体类【实体属性和SQL参数存在关联关系 {属性名} 】</typeparam>
        /// <param name="cmd"></param>
        /// <param name="connString"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool ExtInsert<T>(string cmd, string connString, T entity) where T : class
        {
            Dictionary<string, object> properties = entity.GetAllPorpertiesNameAndValues();
            List<SqlParameter> ps = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> item in properties)
            {
                string paramName = "@" + item.Key;
                string field = "{" + item.Key + "}";
                if (cmd.Contains(field))
                {
                    cmd = cmd.Replace(field, paramName);
                    //获取参数的数据类型
                    SqlParameter p = new SqlParameter(paramName, item.Value == null ? DBNull.Value : item.Value);
                    ps.Add(p);
                }
            }
            SqlCmdHelper helper = new SqlCmdHelper() { SqlConnString = connString };
            return helper.ExcuteNoQuery(cmd, ps.ToArray()) > 0;
        }
        
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">单挑新增的SQL脚本</param>
        /// <param name="connString">数据库连接串</param>
        /// <param name="entitys"></param>
        /// <returns>批量执行非查询的数目</returns>
        public static int ExtBatchInsert<T>(string sql, string connString, List<T> entitys) where T : class
        {
            if (entitys.Count == 0)
            { 
            
            }
            string[] properties = entitys[0].GetAllProperties();
            List<string> pis = new List<string>();
            foreach (string item in properties)
            {
                string field = "{" + item + "}";
                if (sql.Contains(field))
                {
                    pis.Add(item);
                }
            }
            List<SqlParameter> ps = new List<SqlParameter>();
            List<string> batchSql = new List<string>();
            for (int i=0; i<entitys.Count;i++ )
            {
                T item = entitys[i];
                string  cmd=sql ;
                foreach (var pi in pis)
                {
                    string paramName = "@" + pi+i;
                    string field = "{" +pi + "}";
                    cmd = cmd.Replace(field, paramName);
                    bool exists=false;
                    object obj = item.GetPropertyValue(pi, out exists);
                    //获取参数的数据类型
                    SqlParameter p = new SqlParameter(paramName, obj == null ? DBNull.Value : obj);
                    ps.Add(p);
                }
                batchSql.Add(cmd);
            }
            SqlCmdHelper helper = new SqlCmdHelper() { SqlConnString = connString };
            return helper.ExcuteNoQuery(string.Join(";", batchSql), ps.ToArray());
        }
         class SqlExcuteParam
        {
            public string Cmd { get; set; }
            public List<SqlParameter> Param { get; set; }
        }
        static SqlExcuteParam BuilderSqlParamter<T>(string cmd, T entity) where T : class
        {
            Dictionary<string, object> properties = entity.GetAllPorpertiesNameAndValues();
            List<SqlParameter> ps = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> item in properties)
            {
                string paramName = "@" + item.Key;
                string field = "{" + item.Key + "}";
                if (cmd.Contains(field))
                {
                    cmd = cmd.Replace(field, paramName);
                    //获取参数的数据类型
                    SqlParameter p = new SqlParameter(paramName, item.Value == null ? DBNull.Value : item.Value);
                    ps.Add(p);
                }
            }
            SqlExcuteParam pp = new SqlExcuteParam() { Cmd = cmd, Param = ps };
            return pp;
        }
        public static int RunProcedureNoQuery<T>(string procdureCmd, string connString, T entity, 
            string outPutName, SqlDbType outPutType,out object procedureOut) where T : class
        {
            SqlExcuteParam pms = BuilderSqlParamter(procdureCmd, entity);
            pms.Param.Add(new SqlParameter() { ParameterName = outPutName, SqlDbType = outPutType, Direction = ParameterDirection.Output });
            SqlCmdHelper helper = new SqlCmdHelper() { SqlConnString = connString };
            int result= helper.RunProcedureNoQuery(pms.Cmd, pms.Param.ToArray());
            procedureOut = pms.Param[pms.Param.Count - 1].Value;
            return result;
        }
        public static void BulkSave<T>(string table,string connString,List<T> rows) where T:class
        {
            T def = System.Activator.CreateInstance<T>();
            string[] pnames = def.GetAllProperties();
            DataTable dt = new DataTable();
            string specialType=typeof(bool).Name;
            foreach (var item in pnames)
            {
                //获取属性的数据类型
                Type pt= def.GetPropertyType(item);
                dt.Columns.Add(new DataColumn(item, pt.Name == specialType?typeof(int):pt));
            }
            foreach (T item in rows)
            {
                DataRow row = dt.NewRow();
                foreach (var column in pnames)
                {
                    bool hasProperty = false;
                    Type pt= item.GetPropertyType(column);
                    object value = item.GetPropertyValue(column, out hasProperty);
                    if (pt.Name == specialType)
                    {
                        row[column] = ((bool)value ? 1 : 0);
                    }
                    else
                    {
                        row[column] = value;
                    }
                }
                dt.Rows.Add(row);
            }
            SqlCmdHelper helper = new SqlCmdHelper() { SqlConnString = connString };
            helper.BulkSave(dt, table);
        }
        public static List<T> QueryModelList<T>(string sql, SqlParameter[] param, string sqlConnString,int beginRow,int endRow) where T:class
        {
            DataSet ds = new SqlCmdHelper() { Timeout=60}.QueryDataSet(sql, sqlConnString, param, beginRow, endRow, typeof(T).Name);
            return DataHelp.DataReflection.DataSetConvert<T>(ds);
        }
        public static int ExecuteCount(string sql,SqlParameter[] param,string sqlConnString)
        {
            SqlDataReader read = new SqlCmdHelper().ExcuteQuery(sql, sqlConnString, param);
            if (read.HasRows)
            {
                read.Read();
                object obj = read[0];
                int count = -1;
                if (obj != null && int.TryParse(obj.ToString(), out count))
                {
                    return count;
                }
                return -1;
            }
            read.Close();
            return -1;
        }
        [Description("执行查询的存储过程，并转化为响应实体对象")]
        public static List<T> QuerySPModelList<T>(string sql, SqlParameter[] param, string sqlConnString, int beginRow, int endRow) where T : class
        {
            DataSet ds = new SqlCmdHelper().ExcuteQuerySP(sql, sqlConnString, beginRow, endRow, typeof(T).Name, param);
            return DataHelp.DataReflection.DataSetConvert<T>(ds);
        }
    }
}
