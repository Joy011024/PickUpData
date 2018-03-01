using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using HRApp.IApplicationService;
using IHRApp.Infrastructure;
using Common.Data;
using Infrastructure.ExtService;
namespace HRApp.ApplicationService
{
    public class AppSettingService:IAppSettingService
    {
        public IAppSettingRepository appSettingRepository;
        public AppSettingService(IAppSettingRepository appSet)
        {
            appSettingRepository = appSet;
        }
        public string SqlConnString
        {
            get;
            set;
        }
        /// <summary>
        /// 新增系统配置
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public JsonData Add(CategoryItems item)
        {
            JsonData json = new JsonData() { Result=true};
            if (!item.CreateTime.HasValue)
            {
                item.CreateTime = DateTime.Now;
            }
            if (!item.ParentId.HasValue)
            {//表示这是跟节点
                item.ParentId = -1;
            }
            if (string.IsNullOrEmpty(item.Code))
            {
                item.Code = item.Name.TextConvertChar(true);
            }
            if (string.IsNullOrEmpty(item.ParentCode))
            {
                item.ParentCode = item.ParentId.Value.ToString();
            }
            try
            {
                json.Success = appSettingRepository.Add(item);
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return json;
        }


        public List<CategoryItems> QueryWhere(CategoryItems model)
        {

            throw new NotImplementedException();
        }


        public Common.Data.JsonData SelectNodesByParent(string parentNode)
        {
            Common.Data.JsonData json = new JsonData() { Result=true};
            try
            {
                List<CategoryItems> data = appSettingRepository.GetNodeListByParent(parentNode);
                json.Success = true;
                json.Data = data;
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return json;
        }
    }
}
