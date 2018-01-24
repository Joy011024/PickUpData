using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data;
using HRApp.Model;
namespace HRApp.IApplicationService
{
    public interface IAppDataService:IBaseServiceWithSqlConnstring<AppModel>
    {
       
    }
}
