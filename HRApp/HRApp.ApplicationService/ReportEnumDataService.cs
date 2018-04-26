using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
using HRApp.IApplicationService;
namespace HRApp.ApplicationService
{
    public class ReportEnumDataService:IReportEnumDataService
    {
        public string SqlConnString
        {
            get;
            set;
        }

        public Common.Data.JsonData Add(ReportEnumDetail model)
        {
            throw new NotImplementedException();
        }

        public List<ReportEnumDetail> QueryWhere(ReportEnumDetail model)
        {
            throw new NotImplementedException();
        }
    }
}
