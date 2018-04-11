using Common.Data;
using HRApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRApp.IApplicationService
{
    public interface IAppSettingService : IBaseAllWithSqlConnString<CategoryItems>
    {
       new  string SqlConnString { get; set; }
       new  JsonData Add(CategoryItems item);
       Common.Data.JsonData SelectNodesByParent(string parentNodeCode);
       List<CategoryItems> SelectNodeItemByParentCode(string parentCode);
    }
}
