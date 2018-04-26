using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
namespace HRApp.Infrastructure
{
    public class ReportEnumDataRepository:IReportEnumDataRepository
    {
        public IList<ReportEnumDetail> QueryList(string cmd, Dictionary<string, object> param)
        {
            throw new NotImplementedException();
        }

        public IList<ReportEnumDetail> QueryAll()
        {
            throw new NotImplementedException();
        }

        public string SqlConnString
        {
            get;
            set;
        }

        public bool Add(ReportEnumDetail entity)
        {
            throw new NotImplementedException();
        }

        public bool Edit(ReportEnumDetail entity)
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

        public ReportEnumDetail Get(object key)
        {
            throw new NotImplementedException();
        }

        public IList<ReportEnumDetail> Query(string cmd)
        {
            throw new NotImplementedException();
        }
    }
}
