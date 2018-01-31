using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
namespace Infrastructure.MsSqlService.SqlHelper
{
    public class SqlCmdHelper
    {
        public string SqlConnString { get; set; }
        /// <summary>
        /// 执行非查询命令时收影响的行数
        /// </summary>
        /// <param name="sqlCmd"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public int ExcuteNoQuery(string sqlCmd,params SqlParameter[] pms) 
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
            int result= comm.ExecuteNonQuery();
            conn.Close();
            return result;
        }
        public int RunProcedureNoQuery(string proceudreCmd,params SqlParameter[] pms) 
        {
            SqlConnection conn = new SqlConnection(SqlConnString);
            conn.Open();
            SqlCommand comm = new SqlCommand(proceudreCmd,conn);
            if (pms != null && pms.Length > 0)
            {
                comm.Parameters.AddRange(pms);
            }
            comm.CommandType = CommandType.StoredProcedure;
            int result= comm.ExecuteNonQuery();
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
            bulk.BatchSize = table.Rows.Count;
            conn.Open();
            sw.Start();
            bulk.WriteToServer(table);
            sw.Stop();
            conn.Close();
        }
    }
}
