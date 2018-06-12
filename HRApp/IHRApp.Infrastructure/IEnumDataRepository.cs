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
        /// <summary>
        /// 执行新增并返回主键
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int AddReturnId(EnumData data);
        List<EnumData> QueryEnumMember(string enumName, bool isContainerDelete);
    }
}
