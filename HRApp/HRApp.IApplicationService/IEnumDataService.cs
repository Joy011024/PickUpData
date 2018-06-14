using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using Common.Data;
namespace HRApp.IApplicationService
{
    public interface IEnumDataService : IBaseServiceWithSqlConnstring<EnumData>
    {
        JsonData Add(EnumData data);
        List<EnumData> QueryList(RequestParam param);
        bool UpdateRemark(int id, string remark);
        /// <summary>
        /// 这是供后台使用以实现系统保障数据使用的接口【在系统使用时该接口处于屏蔽状态】
        /// </summary>
        /// <typeparam name="EnumFieldAttribute"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        int BatchInsert<EnumFieldAttribute>(Enum e) where EnumFieldAttribute : Attribute;
        List<EnumData> QueryEnumMember(string enumName);
    }
}
