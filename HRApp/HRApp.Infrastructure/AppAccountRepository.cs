using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using IHRApp.Infrastructure;
namespace HRApp.Infrastructure
{
    public class AppAccountRepository:IAppAccountRepository
    {
        public bool SaveSignInInfo(UserAccount user)
        {
            throw new NotImplementedException();
        }

        public string SqlConnString
        {
            get;
            set;
        }
    }
}
