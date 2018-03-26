using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRApp.Web.Controllers
{
    [Description("订单元数据管理")]
    public class OrderManageController : Controller
    {
        //
        // GET: /OrderManage/
        [Description("货物维护管理")]
        public ActionResult GoodsManage()
        {
            return View();
        }
        [Description("录入货物数据对话框")]
        public ActionResult AddGoodsDialog()
        {
            return View();
        }
    }
}
