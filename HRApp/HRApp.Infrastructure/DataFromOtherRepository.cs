using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
using HRApp.Model;
using IHRApp.Infrastructure;
using System.Data.SqlClient;
namespace HRApp.Infrastructure
{
    public class DataFromOtherRepository : IDataFromOtherRepository
    {
        [DescriptionSort("查询结果集")]
        public List<Domain.CommonData.FindQQDataTable> QueryUinList(DateTime beginTime, DateTime endTime, int beginRow, int endRow, out int count)
        {//必须声明标量变量 "@return_value
            string sql = @" EXEC @return_value = [dbo].[SP_QueryAccount]
		@beginDayInt=@bi ,
		@endDayInt=@ei ,
		@beginRow=@br,
		@endRow=@er,
		@total=@total  OUTPUT ";
            List<SqlParameter> ps = new List<SqlParameter>();
            string format = "yyyyMMdd";//String[4]: Size 属性具有无效大小值 0
            ps.Add(new SqlParameter() { ParameterName = "@beginDayInt", Value = int.Parse(beginTime.ToString(format)), DbType = DbType.Int32 });
            ps.Add(new SqlParameter() { ParameterName = "@endDayInt", Value = int.Parse(endTime.ToString(format)), DbType = DbType.Int32 });
            ps.Add(new SqlParameter() { ParameterName = "@beginRow", Value = beginRow, DbType = DbType.Int32 });
            ps.Add(new SqlParameter() { ParameterName = "@endRow", Value = endRow, DbType = DbType.Int32 });
            ps.Add(new SqlParameter() { ParameterName = "@total", Direction = ParameterDirection.Output, DbType = DbType.Int32 });
            List<FindQQDataTable> data = CommonRepository.QuerySPModelList<FindQQDataTable>("SP_QueryAccount", ps.ToArray(), SqlConnString, beginRow, endRow);
            count = (int)ps[ps.Count - 1].Value;
            return data;
        }

        public IList<Domain.CommonData.FindQQDataTable> QueryList(string cmd, Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public string SqlConnString
        {
            get;
            set;
        }

        public bool Add(Domain.CommonData.FindQQDataTable entity)
        {
            throw new NotImplementedException();
        }

        public bool Edit(Domain.CommonData.FindQQDataTable entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(object key)
        {
            throw new NotImplementedException();
        }

        public bool LogicDel(object key)
        {
            throw new NotImplementedException();
        }

        public Domain.CommonData.FindQQDataTable Get(object key)
        {
            throw new NotImplementedException();
        }

        public IList<Domain.CommonData.FindQQDataTable> Query(string cmd)
        {
            throw new NotImplementedException();
        }


        public IList<FindQQDataTable> QueryAll()
        {
            throw new NotImplementedException();
        }
    }
}
