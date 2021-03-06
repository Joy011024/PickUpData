﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRApp.IApplicationService;
using HRApp.Model;
namespace HRApp.Web.Controllers
{
    public class CommonDialogController : Controller
    {
        //
        // GET: /CommonDialog/
        [Description("公用对话框")]
        public ActionResult SampleDataDialog()
        {
            return View();
        }
        public ActionResult SampleNoteDataDialog() 
        {
            return View();
        }
        public ActionResult CommonValueDialog()
        {
            return View();
        }
        public ActionResult CommonSelectDialog(string enumName, int id) 
        { //公用的选择框的对话框窗体
            List<EnumData> list = new List<EnumData>(); try
            {
                IEnumDataService enumService = IocMvcFactoryHelper.GetInterface<IEnumDataService>();
                list = enumService.QueryEnumMember(enumName);
            }
            catch (Exception ex)
            {
                list = new List<EnumData>();
            }
            ViewData["Id"] = id;
            return View(list);
        }
        [Description("树结构数据来源dataSourceUrl，操作项id")]
        public ActionResult CommonTreeDialog(string dataSourceUrl,int id) 
        {
            ViewData["url"] = dataSourceUrl;
            ViewData["id"] = id;
            return View();
        }
    }
}
