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
            string dir=Server.MapPath("/UploadFile/")+DateTime.Now.ToString(Common.Data.CommonFormat.DateIntFormat);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            int succNum=0;
            json.Message = string.Format("本次总共上传文件 {0} 份",files.Count);
            //进行文件存储
            for (int i = 0; i < files.Count; i++)
            {
                try
                {
                    HttpPostedFileBase hf = files[i];
                    string[] fileInfo = hf.FileName.Split('.');
                    hf.SaveAs(dir + "/" + DateTime.Now.ToString(Common.Data.CommonFormat.DateTimeIntFormat) + "." + fileInfo[fileInfo.Length - 1]);
                    succNum++;
                    json.Message += string.Format("<br/>第{0}张上传成功。", i + 1);
                }
                catch (Exception ex)
                {
                    json.Message += string.Format("<br/>第{0}张上传失败。" ,i+1);
                }
            }
            json.Success = succNum > 0;
            return Json(json);
        }
    }
}
