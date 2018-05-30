using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;

namespace IHRApp.Infrastructure
{
    public interface IAppAccountRepository:IBaseSampleRepository
    {
        bool SaveSignInInfo(UserAccount user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="canNoWhereQuery">是否能无条件查找</param>
        /// <returns></returns>
        List<UserAccount> QueryUses(RequestParam param,bool canNoWhereQuery);
    }
}
