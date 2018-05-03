using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRApp.Model;
using Infrastructure.MsSqlService.SqlHelper;
namespace HRApp.Web.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            Test();
            return View();
        }
        public ActionResult UILayout() 
        {
            return View();
        }
        void Test() 
        {
            CategoryItems appSetting = new CategoryItems();
            string sql = appSetting.MssqlExportInsertSql();
            SqlCmdHelper cmd = new SqlCmdHelper();
            cmd.GetPropertiesFromString(sql, appSetting);
            string edit = appSetting.MssqlExportEditSql();
            cmd.GetPropertiesFromString(edit, appSetting);
        }
    }
}
