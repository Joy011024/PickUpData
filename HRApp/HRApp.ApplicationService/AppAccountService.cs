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
        IAppAccountRepository accountDal;
        public JsonData SignIn(Model.SignInAccountParam param)
        {
            JsonData json = new JsonData() { Result=true};
            try
            {

                UserAccount acc = new UserAccount();
                bool succ = accountDal.SaveSignInInfo(acc);
                return json;
            }
            catch (Exception ex)
            {
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
