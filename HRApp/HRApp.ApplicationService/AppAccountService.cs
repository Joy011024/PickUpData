using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data;
using HRApp.IApplicationService;
using IHRApp.Infrastructure;
using HRApp.Model;
using DataHelp;
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
                json.Success = succ;
                return json;
            }
            catch (Exception ex)
            {
                logDal.SqlConnString = accountDal.SqlConnString;
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

        /// <summary>
        /// 查询注册的账户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonData QuerySignInAccount(RequestParam param )
        {
            JsonData json = new JsonData() { Result=true};
            try
            {
                if (param.RowBeginIndex <= 0)
                {
                    param.RowBeginIndex = 0;
                    param.RowEndIndex = param.RowBeginIndex + 30;
                }
                else {
                    param.RowBeginIndex = param.RowBeginIndex - 1;
                    param.RowEndIndex = param.RowEndIndex - 1;
                }
                //有条件查找 进入接口1
                List<UserAccount> acc = accountDal.QueryUses(param, false);
                if (acc == null)
                {
                    logDal.WriteLog(Domain.CommonData.ELogType.Account, "QuerySignInAccount not provider query where,will run no found interface", Domain.CommonData.ELogType.Account.ToString(), true);
                    acc = accountDal.QueryUses(param, true);//无条件查询的情形进入接口2
                    json.Data = acc;
                    logDal.WriteLog(Domain.CommonData.ELogType.Account, "QuerySignInAccount call ignore param of interface", Domain.CommonData.ELogType.Account.ToString(), true);
                }
                List<Account> sign = new List<Account>();
                foreach (var item in acc)
                {
                    sign.Add(DataHelp.DataReflection.ConvertMapModel<UserAccount, Account>(item));
                }
                json.Data = sign;//去除加密痕迹
                json.Success = true;
            }
            catch (Exception ex)
            {
                //使用ex.Tostring() 会超出限制
                logDal.WriteLog(Domain.CommonData.ELogType.Account, "QuerySignInAccount happend exception:" + ex.Message, Domain.CommonData.ELogType.Account.ToString(), true);
                json.Message = ex.Message;
            }
            return json;
        }
    }
}
