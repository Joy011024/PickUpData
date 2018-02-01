using Infrastructure.MsSqlService.SqlHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonHelperEntity;
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
    }
}
