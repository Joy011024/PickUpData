using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
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
            return comm.ExecuteNonQuery();
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
            return comm.ExecuteNonQuery();
        }
    }
}
