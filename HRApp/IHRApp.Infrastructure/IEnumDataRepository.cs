using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
namespace IHRApp.Infrastructure
{
    public interface IEnumDataRepository:IBaseListRepository<EnumData>
    {
        bool UpdateRemark(int id, string remark);
    }
}
