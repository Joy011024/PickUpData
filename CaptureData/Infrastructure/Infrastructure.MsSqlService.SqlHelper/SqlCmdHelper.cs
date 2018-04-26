using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using CommonHelperEntity;
namespace Infrastructure.MsSqlService.SqlHelper
{
    public class SqlCmdHelper
    {
        /// <summary>
        /// 超时时长
        /// </summary>
        public int? Timeout { get; set; }
        public string SqlConnString { get; set; }
        /// <summary>
        /// 执行非查询命令时收影响的行数
        /// </summary>
        /// <param name="sqlCmd"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public int ExcuteNoQuery(string sqlCmd, params SqlParameter[] pms)
        {
            if (string.IsNullOrEmpty(SqlConnString))
            {
                return -1;
            }
            SqlConnection conn = new SqlConnection(SqlConnString);
            conn.Open();
            SqlCommand comm = new SqlCommand(sqlCmd, conn);
            if (pms != null && pms.Length > 0)
            {
                comm.Parameters.AddRange(pms);
            }
            int result = comm.ExecuteNonQuery();
            conn.Close();
            return result;
        }
        public int RunProcedureNoQuery(string proceudreCmd, params SqlParameter[] pms)
        {
            SqlConnection conn = new SqlConnection(SqlConnString);
            conn.Open();
            SqlCommand comm = new SqlCommand(proceudreCmd, conn);
            if (pms != null && pms.Length > 0)
            {
                comm.Parameters.AddRange(pms);
            }
            comm.CommandType = CommandType.StoredProcedure;
            int result = comm.ExecuteNonQuery();
            conn.Close();
            return result;
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="table">待批量添加的数据</param>
        /// <param name="destinationTableName">数据库中要批量添加数据的表名</param>
        public void BulkSave(DataTable table, string destinationTableName)
        {
            SqlConnection conn = new SqlConnection(SqlConnString);
            Stopwatch sw = new Stopwatch();
            SqlBulkCopy bulk = new SqlBulkCopy(conn);//批量添加
            bulk.DestinationTableName = destinationTableName;
            foreach (DataColumn item in table.Columns)
            {
                bulk.ColumnMappings.Add(item.ColumnName, item.ColumnName);
            }
            bulk.BatchSize = table.Rows.Count;
            conn.Open();
            sw.Start();
            bulk.WriteToServer(table);
            sw.Stop();
            conn.Close();
        }
        public DataSet QueryDataSet(string sql,string sqlconnString,SqlParameter[] param,int? beginRow,int? endRow,string dataSetName)
        {
            SqlConnection conn = new SqlConnection(sqlconnString);
            Stopwatch sw = new Stopwatch();//语句运行时间检测
            if (Timeout.HasValue)
            {
                Exception exMsg = null;
                bool connSuccess = false;
                Thread t = new Thread(delegate()
                {
                    try
                    {
                        sw.Start();
                        conn.Open(); //如果一开始数据库就没法连接则不会进入到操作超时过程
                        connSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        if (exMsg == null)
                        {
                            exMsg = ex;
                        }
                    }
                });
                t.Start();
                //如果执行成功则跳过这个步骤
                while (Timeout.Value > sw.ElapsedMilliseconds / 1000)
                { //秒级别比较
                    if (t.Join(1)) break;
                    if (connSuccess) {
                        break;
                    }
                }
                if (!connSuccess)
                { //如果没有执行成功的
                    throw exMsg;
                }
            }
            else 
            {
                conn.Open();
            }
            SqlCommand comm = new SqlCommand(sql, conn);
            if (Timeout.HasValue)
            {
                comm.CommandTimeout = Timeout.Value;//单位为秒
            }
            
            if (param != null && param.Length > 0)
            {
                comm.Parameters.AddRange(param);
            }
            SqlDataAdapter dap = new SqlDataAdapter(comm);
            DataSet ds = new DataSet();
            if (!beginRow.HasValue)
            {
                dap.Fill(ds);
            }
            else 
            {
                dap.Fill(ds, beginRow.Value, endRow.HasValue ? endRow.Value : int.MaxValue, dataSetName);
            }
            conn.Close();
            return ds;
        }
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlconnString"></param>
        /// <param name="param"></param>
        /// <returns>查询结果</returns>
        public SqlDataReader ExcuteQuery(string sql, string sqlconnString, SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(sqlconnString);
            conn.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
            if (param != null && param.Length > 0)
            {
                comm.Parameters.AddRange(param);
            }
            SqlDataReader reader= comm.ExecuteReader();
            return reader;
        }
        [Description("执行查询的存储过程")]
        public DataSet ExcuteQuerySP(string sp,string connString,int? beginRow,int? endRow,string tableName,params SqlParameter[] ps) 
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand comm = new SqlCommand();// new SqlCommand(sp, conn);
            comm.CommandType = CommandType.StoredProcedure;//存储过程
            comm.Connection = conn;
            comm.CommandText = sp;
            conn.Open();
            if (ps != null && ps.Length > 0)
            {
                comm.Parameters.AddRange(ps);
            }
            SqlDataAdapter dap = new SqlDataAdapter(comm);
            DataSet ds = new DataSet();
            if (beginRow.HasValue)
            {
                dap.Fill(ds, beginRow.Value-1, (endRow.HasValue ? endRow.Value : int.MaxValue), tableName);
            }
            else {
                dap.Fill(ds);
            }
            return ds;
        }
        /// <summary>
        /// 提供待执行的SQL语句进行参数化执行SQL语句
        /// </summary>
        /// <typeparam name="T">参数实体类</typeparam>
        /// <param name="sqlCmd">sql 中参数预设值为 {paramName}</param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GenerateNoQuerySqlAndExcute<T>(string sqlCmd, T entity) where T : class
        {
            Dictionary<string, object> properties = entity.GetAllPorpertiesNameAndValues();
            List<SqlParameter> pms = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> item in properties)
            {
                string paramName = "@" + item.Key;
                string field = "{" + item.Key + "}";
                if (sqlCmd.Contains(field))
                {
                    sqlCmd = sqlCmd.Replace(field, paramName);
                    //获取参数的数据类型
                    SqlParameter p = new SqlParameter(paramName, item.Value == null ? DBNull.Value : item.Value);
                    pms.Add(p);
                }
            }
            if (string.IsNullOrEmpty(SqlConnString))
            {
                return -1;
            }
            SqlConnection conn = new SqlConnection(SqlConnString);
            conn.Open();
            SqlCommand comm = new SqlCommand(sqlCmd, conn);
            if (pms != null && pms.Count > 0)
            {
                comm.Parameters.AddRange(pms.ToArray());
            }
            int result = comm.ExecuteNonQuery();
            conn.Close();
            return result;
        }
    }
}
