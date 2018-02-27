using Domain.CommonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRApp.Web.Controllers
{
    public class FileHelperController : BaseController
    {
        //
        // GET: /FileHelper/
        [DescriptionSort("文件上传")]
        public ActionResult FileUpload()
        {
            return View();
        }
        public JsonResult GetUploadFile() 
        {
            Common.Data.JsonData json = new Common.Data.JsonData() { Result=true};
            HttpFileCollectionBase files = HttpContext.Request.Files;
            if (files.Count < 1)
            {
                json.Message = AppLanguage.Lang.Tip_PleaseSelectFile;
                json.Success = false;
                return Json(json);
            }
            return Json(json);
        }
    }
}
