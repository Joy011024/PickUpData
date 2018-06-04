using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
namespace HRApp.IApplicationService
{
    public interface IEnumDataService : IBaseServiceWithSqlConnstring<EnumData>
    {
        bool Add(EnumData data);
        List<EnumData> QueryList(RequestParam param);
        bool UpdateRemark(int id, string remark);
    }
}
