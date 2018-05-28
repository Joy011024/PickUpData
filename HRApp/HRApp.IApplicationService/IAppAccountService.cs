using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using Common.Data;
namespace HRApp.IApplicationService
{
    public interface IAppAccountService:BaseService
    {
        JsonData SignIn(SignInAccountParam param);
    }
}
