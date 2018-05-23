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

        public bool WriteLog(ELogType type,string log,string title,bool  isError)
        {
            LogData data = new LogData()
            {
                CreateTime = DateTime.Now,
                Category = (short)type.GetHashCode(),
                Id = Guid.NewGuid(),
                IsError = isError,
                Note = log,
                Title = title
            };
            data.DayInt =int.Parse( data.CreateTime.ToString(CommonFormat.DateTimeIntFormat));
            SqlCmdHelper cmd = new SqlCmdHelper() {  SqlConnString=SqlConnString};
            string sql= SqlCmdHelper.GenerateInsertSql<LogData>();
            return CommonRepository.ExtInsert(sql, SqlConnString, data);
           // throw new NotImplementedException();
        }
    }
}
