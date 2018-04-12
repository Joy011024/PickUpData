using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DataHelp;
namespace CaptureWebData
{
    public class VIpAddressService
    {
        public VIpAddressService(string sqlConnString) 
        {
            ConnString = sqlConnString;
        }
        public string ConnString { get; private set; }
        public List<VIpAddress> GetIpData(int start, int end)
        {
            List<VIpAddress> ips = new List<VIpAddress>();
            SqlConnection conn = new SqlConnection(ConnString);
            string sql = @"select id,startiptext,endiptext from 
(
	select row_number() over(order by id)  as num,id,startiptext,endiptext  from  V_IpAddress
)as t
where t.num>={0} and t.num<={1}";
            sql = string.Format(sql, start, end);
            SqlCommand comm = new SqlCommand(sql, conn);
            SqlDataAdapter dap = new SqlDataAdapter(comm);
            DataSet ds = new DataSet();
            dap.Fill(ds);
            return ds.DataSetConvert<VIpAddress>();
        }
        /// <summary>
        /// 总共条数
        /// </summary>
        /// <returns></returns>
        public int Count() 
        {
            string sql = @"select count(id) from  V_IpAddress";
            SqlConnection conn = new SqlConnection(ConnString);
            conn.Open();
            SqlCommand comm = new SqlCommand(sql, conn);
            int result= (int) comm.ExecuteScalar();
            conn.Close();
            return result;
        }
    }
}
