using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data;
using HRApp.IApplicationService;
using IHRApp.Infrastructure;
using HRApp.Model;
namespace HRApp.ApplicationService
{
    public class AppAccountService:IAppAccountService
    {
        public IAppAccountRepository accountDal;
        public ILogDataRepository logDal;
        public JsonData SignIn(Model.SignInAccountParam param)
        {
            JsonData json = new JsonData() { Result=true};
            try
            {
               //去除首位空格
                param.Trim();
                //用户名不能含有中文
                int code=param.AccountValid();
                if (code!=0)
                {
                    // ErrorCode转换为对应的错误码
                    json.StatueCode = code;
                    return json;
                }
                UserAccount acc = new UserAccount()
                {
                    UserName = param.UserName,
                    IsActive = true,
                    Nick = param.Nick,
                    Psw = param.Psw
                };
                bool succ = accountDal.SaveSignInInfo(acc);
                logDal.WriteLog(Domain.CommonData.ELogType.Account, "Sign in 【" + param.UserName + "】 Success", "Sign in", true);
                return json;
            }
            catch (Exception ex)
            {
                logDal.WriteLog(Domain.CommonData.ELogType.Account,"Sign in 【"+param.UserName+"】"+ ex.Message, "Sign in", false);
                json.Message = ex.Message;
                return json;
            }
        }

        public string SqlConnString
        {
            get;
            set;
        }
    }
}
