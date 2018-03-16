﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.ComponentModel;
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
            conn.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
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
                dap.Fill(ds, beginRow.Value, (endRow.HasValue ? endRow.Value : int.MaxValue), tableName);
            }
            else {
                dap.Fill(ds);
            }
            return ds;
        }
    }
}
