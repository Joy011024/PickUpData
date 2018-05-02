using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
namespace HRApp.Infrastructure
{
    public class RelyTableRepository:IRelyTableRepository
    {
        public IList<RelyTable> QueryList(string cmd, Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public IList<RelyTable> QueryAll()
        {
            InitTableData();
            RelyTable rely = new RelyTable();
            string sql = rely.GetSampleQuerySql();
            return CommonRepository.QueryModelList<RelyTable>(sql, null, SqlConnString, 0, int.MaxValue);
        }

        public string SqlConnString
        {
            get;
            set;
        }

        public bool Add(RelyTable entity)
        {
            throw new NotImplementedException();
        }

        public bool Edit(RelyTable entity)
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

        public RelyTable Get(object key)
        {
            throw new NotImplementedException();
        }

        public IList<RelyTable> Query(string cmd)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitTableData()
        {
            RelyTable rely = new RelyTable();
            //添加忽略项
            string sql = rely.GetInitDBRelactiveSql();
            CommonRepository.ExtInsert(sql, SqlConnString, rely);
        }
    }
}
