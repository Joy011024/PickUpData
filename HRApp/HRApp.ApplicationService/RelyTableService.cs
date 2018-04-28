using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
using HRApp.Infrastructure;
using HRApp.IApplicationService;
namespace HRApp.ApplicationService
{
    public class RelyTableService:IRelyTableService
    {
        IRelyTableService relyService;
        public RelyTableService(IRelyTableService rely)
        {
            relyService = rely;
        }
        public string SqlConnString
        {
            get;
            set;
        }
        /// <summary>
        /// 查询表中列
        /// </summary>
        /// <returns></returns>
        public List<RelyTable> QueryAllTableColumns()
        {
           return relyService.QueryAllTableColumns().ToList();
        }

        string IRelyTableService.SqlConnString
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
