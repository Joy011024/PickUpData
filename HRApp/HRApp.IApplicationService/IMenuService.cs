using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
namespace HRApp.IApplicationService
{
    public interface IMenuService : IBaseServiceWithSqlConnstring<Menu>
    {
        Common.Data.JsonData QueryAllMenu();
        List<Menu> QueryMenusByAuthor(string userCode);
        bool ChangeMenuType(int id, int type);
        bool ChangeMenuStatue(int id, bool operate);
        bool ChangeParentMenu(int id, int ParentId);
    }
}
