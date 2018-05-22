using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using HRApp.IApplicationService;
using IHRApp.Infrastructure;
namespace HRApp.ApplicationService
{
    public class LogDataService:ILogDataService
    {
        public ILogDataRepository LogDal;
        public LogDataService(ILogDataRepository logRepository) 
        {
            LogDal = logRepository;
        }
        public string SqlConnString
        {
            get;
            set;
        }

        public List<LogData> QueryLogs(RequestParam param)
        {
            throw new NotImplementedException();
        }
    }
}
