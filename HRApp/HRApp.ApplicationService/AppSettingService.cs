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
        public IAppSettingRepository appSettingRepository { get; set; }
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
                //首先检测编码是否已经使用
                string code = item.Code;
                if (appSettingRepository.ValideExists(code) == 0)
                {
                    json.Success = appSettingRepository.Add(item);
                }
                else 
                {
                    json.Success = false;
                    json.Message = string.Format(AppLanguage.Lang.Tip_CodeHasUsed, code);
                }
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return json;
        }


        public List<CategoryItems> QueryWhere(CategoryItems model)
        {
            string cmd = string.Empty;
            return appSettingRepository.Query(cmd).ToList();
            //throw new NotImplementedException();
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


        public List<CategoryItems> SelectNodeItemByParentCode(string parentCode)
        {
            return appSettingRepository.GetNodeListByParent(parentCode);
        }
    }
}
