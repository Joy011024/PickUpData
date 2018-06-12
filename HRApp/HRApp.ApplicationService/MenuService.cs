using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRApp.Model;
using HRApp.IApplicationService;
using IHRApp.Infrastructure;
using Infrastructure.ExtService;
using Domain.CommonData;
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
        public ILogDataRepository logDal;
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
            try
            {
                bool succ = menuRepository.Add(model);
                logDal.WriteLog(ELogType.DataInDBLog,
                  string.Format(LogData.InsertDbNoteFormat(), typeof(Menu).Name),
                  "insert", succ);
                json.Success = true;
            }
            catch (Exception ex)
            {
                string msg=ex.Message;
                logDal.WriteLog(ELogType.DataInDBLog,
                     string.Format(LogData.InsertDbNoteFormat(), typeof(Menu).Name)+"\r\n"+msg,
                     "insert Error", false);
                json.Message = msg;
            }
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
        public bool ChangeMenuType(int id, int type)
        {
           bool succ=  menuRepository.ChangeMenuType(id, type);
           logDal.WriteLog(ELogType.DataInDBLog, string.Format("Update menu type,id={0},target of menu type={1}",id,type), typeof(Menu).Name, succ);
           return succ;
        }
        public bool ChangeMenuStatue(int id, bool operate)
        {
            bool succ = menuRepository.ChangeMenuStatue(id, operate);
            logDal.WriteLog(ELogType.DataInDBLog, string.Format("Update menu isEnable,id={0},target of isEnable={1}", id, operate), typeof(Menu).Name, succ);
            return succ;
        }
    }
}
