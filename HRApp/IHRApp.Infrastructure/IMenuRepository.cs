using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
namespace IHRApp.Infrastructure
{
    /// <summary>
    /// 菜单操作的数据访问层
    /// </summary>
    public interface IMenuRepository:IBaseRepository<Menu>
    {
        List<Menu> QueryMenus();
    }
}
