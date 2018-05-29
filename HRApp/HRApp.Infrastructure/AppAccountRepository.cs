using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
using Infrastructure.MsSqlService.SqlHelper;
namespace HRApp.Infrastructure
{
    public class AppAccountRepository:IAppAccountRepository
    {
        public bool SaveSignInInfo(UserAccount user)
        {//保存数据
            if (string.IsNullOrEmpty(user.Encry))
            {
                user.Encry = "-1";
            }
            user.Id = Guid.NewGuid();
            user.CreateTime = DateTime.Now;
            SqlCmdHelper help = new SqlCmdHelper() { SqlConnString=SqlConnString};
            string sql= SqlCmdHelper.GenerateInsertSql<UserAccount>();
            return help.GenerateNoQuerySqlAndExcute(sql, user) > 0;
        }

        public string SqlConnString
        {
            get;
            set;
        }
    }
}
