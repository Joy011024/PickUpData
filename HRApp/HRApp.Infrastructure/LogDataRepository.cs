using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
using Domain.CommonData;
using Common.Data;
using Infrastructure.MsSqlService.SqlHelper;
namespace HRApp.Infrastructure
{
    public class LogDataRepository:ILogDataRepository
    {
        public string SqlConnString
        {
            get;
            set;
        }

        public bool WriteLog(ELogType type, string log, string title, bool isNormalLog)
        {
            DateTime appTime = Domain.GlobalModel.AppRunData.RunTime;//程序启动时的时间
            LogData data = new LogData()
            {
                CreateTime = DateTime.Now,
                Category = (short)type.GetHashCode(),
                Id = Guid.NewGuid(),
                IsError = !isNormalLog,
                Note = log,
                Title = title
            };
            data.DayInt =int.Parse( data.CreateTime.ToString(CommonFormat.DateIntFormat));
            SqlCmdHelper cmd = new SqlCmdHelper() {  SqlConnString=SqlConnString};
            string sql= SqlCmdHelper.GenerateInsertSql<LogData>();
            return CommonRepository.ExtInsert(sql, SqlConnString, data);
           // throw new NotImplementedException();
        }
        public List<LogData> QueryLogs(RequestParam param,out int total)
        {
            string day = param.BeginTime;
            if (string.IsNullOrEmpty(day))
            {
                day = DateTime.Now.ToString(Common.Data.CommonFormat.DateFormat);
            }
            total = 0;
            if (param.RowEndIndex < param.RowBeginIndex)
            {
                return new List<LogData>();
            }
            //将这个日期转换为指定的日期串【兼容传递的值为 date或者datetime】
            DateTime qd= DateTime.ParseExact(day, Common.Data.CommonFormat.DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
            int dayInt = int.Parse(qd.ToString(Common.Data.CommonFormat.DateIntFormat));
            //exec SP_QueryDayLog 20180705 ,1,200,@total out
            string exec = string.Format("exec SP_QueryDayLog {0} ,{1},{2},@total out", dayInt, param.RowBeginIndex, param.RowEndIndex);
            OutputParam p = new OutputParam();
            List<LogData> data= CommonRepository.QueryModels<LogData,OutputParam>(exec, p, SqlConnString);
            total = p.Total;
            return data;
        }
    }
}