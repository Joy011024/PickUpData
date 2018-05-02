using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using HRApp.IApplicationService;
using IHRApp.Infrastructure;
using Infrastructure.ExtService;
namespace HRApp.ApplicationService
{
    public class MenuService:IMenuService
    {
        public string SqlConnString
        {
            get;
            set;
        }
        public IMenuRepository menuRepository;
        public MenuService(IMenuRepository menu)
        {
            menuRepository = menu;
        }
        public Common.Data.JsonData Add(Menu model)
        {
            Common.Data.JsonData json = new Common.Data.JsonData();
            if (string.IsNullOrEmpty(model.Code))
            {//中文转拼音
                model.Code = model.Name.TextConvertChar(true);// 转换形式 助手  =ZhuShou
            }
            model.CreateTime = DateTime.Now;
            json.Success = menuRepository.Add(model);
            return json;
        }


        public List<Menu> QueryWhere(Menu model)
        {
            throw new NotImplementedException();
        }

        public Common.Data.JsonData QueryAllMenu()
        {
            Common.Data.JsonData json = new Common.Data.JsonData() { Result=true};
            try
            {
                json.Data = menuRepository.QueryMenus();
                json.Success = true;
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
            }
            return json;
        }


        public Menu Get(object id)
        {
            throw new NotImplementedException();
        }


        public bool Update(Menu entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询授权的用户菜单列表
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public List<Menu> QueryMenusByAuthor(string userCode)
        {
            return menuRepository.QueryMenus();
        }
    }
}
