using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using HRApp.IApplicationService;
using Common.Data;
using IHRApp.Infrastructure;
using Infrastructure.ExtService;
namespace HRApp.ApplicationService
{
    public class AppDataService:IAppDataService
    {
        public IAppRepository appRepostory;
        public AppDataService(IAppRepository _appReposiory) 
        {
            appRepostory = _appReposiory;
        }
        public string SqlConnString
        {
            get;
            set;
        }

        public JsonData Add(AppModel model)
        {
            JsonData json = new JsonData();
            if (string.IsNullOrEmpty(model.AppName))
            { //没有录入应用名称
                return json;
            }
            if (string.IsNullOrEmpty(model.AppCode))
            {//中文转拼音
                model.AppCode = model.AppName.TextConvertChar(true);// 转换形式 助手  =ZhuShou
            }
            try
            {
                json.Success = appRepostory.Add(model);
                json.Result = true;
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return json;
        }
    }
}
