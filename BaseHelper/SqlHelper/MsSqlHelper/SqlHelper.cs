using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MsSqlHelper
{
    public class SqlHelper
    {
        public bool ConnectionSuccess { get; private set; }
        public string SqlConnString { get;private set; }
        public SqlHelper(string connString)
        {
            SqlConnString = connString;
            ConnectionSuccess = CanConnection(connString);
        }
        /// <summary>
        /// 判断数据库实例是否能连接
        /// </summary>
        /// <returns></returns>
        public bool CanConnection(string SqlConnString) 
        {
            bool can = false;
            if (string.IsNullOrEmpty(SqlConnString)) { return can; }
            SqlConnection conn = new SqlConnection(SqlConnString);
            try
            {
                conn.Open();
                can = true;
                conn.Close();
            }
            catch (Exception ex) 
            {
                
            }
            return can;
        }
        public bool ExecuteCmdNonQuery(string cmd) 
        {
            return ExecuteCmdNonQuery(cmd, null);
        }
        public bool ExecuteCmdNonQuery(string cmd, SqlParameter[] param)
        {
            if (!ConnectionSuccess)
            {
                return false;
            }
            SqlConnection conn = new SqlConnection(SqlConnString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(cmd,conn);
                if (param != null && param.Length > 0) 
                {
                    comm.Parameters.AddRange(param);
                }
                return comm.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 根据命令和提供的参数拼接待执行命令的存储过程【在执行操作前请先comm.Connection.Open(); 】
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public SqlCommand PrepareExcuteProcedure(string cmd, SqlParameter[] param) 
        {
            if (!ConnectionSuccess) { return null; }
            SqlConnection conn = new SqlConnection(SqlConnString);
            try
            {
                SqlCommand comm = new SqlCommand(cmd, conn);
                comm.CommandType = CommandType.StoredProcedure;
                if(param!=null&&param.Length>0)
                {
                    comm.Parameters.AddRange(param);
                }
                return comm;
            }
            catch (Exception ex) { return null; }
        }
    }
}
