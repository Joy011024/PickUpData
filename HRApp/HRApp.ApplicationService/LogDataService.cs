using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using HRApp.IApplicationService;
using IHRApp.Infrastructure;
using Domain.CommonData;
using Common.Data;
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

        public JsonData QueryLogs(RequestParam param)
        {
            if (string.IsNullOrEmpty(param.BeginTime))
            {
                param.BeginTime = DateTime.Now.ToString(Common.Data.CommonFormat.DateFormat);
            }
            JsonData json = new JsonData() { Result=true};
            if (param.RowBeginIndex > param.RowEndIndex)
            {
                return json;
            }
            if (param.RowBeginIndex == param.RowEndIndex && param.RowBeginIndex == 0)
            {
                return json;
            }
            if (param.RowBeginIndex > param.RowEndIndex)
            {
                return json;
            }
            try
            {
                int total;
                json.Data = LogDal.QueryLogs(param, out total);
                json.Success = true;
                json.Total = total;
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return json;
        }
    }
}
