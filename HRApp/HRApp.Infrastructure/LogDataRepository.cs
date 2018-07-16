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
using System.Data;
using System.Data.SqlClient;
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
            //exec SP_QueryDayLog 20180705 ,1,200,@total out 使用这样的形式没法获取到输出参数，修改未调用存储过程
            List<SqlParameter> ps = new List<SqlParameter>();
            string exec = "  exec SP_QueryDayLog {day},{beginRow},{endRow} ,{total} output";
            /*
             * 如果存在一个参数被赋值：
             必须传递参数 4，并以 '@name = value' 的形式传递后续的参数。一旦使用了 '@name = value' 形式之后，所有后续的参数就必须以 '@name = value' 的形式传递。
             */
            LogQueryParam p = new LogQueryParam() { day=dayInt,beginRow=param.RowBeginIndex,endRow=param.RowEndIndex};
            Dictionary<ParameterDirection, string[]> dict = new Dictionary<ParameterDirection, string[]>();
            dict.Add(ParameterDirection.Output, new string[] { "total" });
            List<LogData> data = CommonRepository.QueryModels<LogData, LogQueryParam>(exec, p, SqlConnString, dict);
            total = p.total;
            return data;
        }
    }
}