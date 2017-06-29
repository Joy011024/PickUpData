using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpClientHelper;
using System.Net;
using System.IO;
using Infrastructure.ExtService;
using Domain.CommonData;
using Infrastructure.EFMsSQL;
using System.Configuration;
using System.Data.SqlClient;
namespace GatherImage
{
    public class ConfigurationItems
    {
        public string AppNameEng = "GatherImage";
    }
    public class GatherUinImage
    {
        public string TecentDA
        {
            get
            {
                ConnectionStringSettings sec = ConfigurationManager.ConnectionStrings["TecentDA"];
                if (sec == null) { return string.Empty; }
                return sec.ConnectionString;
            }
        }
        public string ImageDir 
        {
            get 
            {
                return ConfigurationManager.AppSettings["ImageDir"];
            }
        }
        public string GetRelativePath
        {
            get 
            {
                return DateTime.Now.ToString("yyyyMMddHHmm");
            }
        }
        List<WaitGatherImage> GetWaitGatherImageData()
        {
            DateTime today = DateTime.Now;
            string sp = string.Format("exec [SP_GetWaitGatherImageList] ", today);
            MainRespority<FindQQDataTable> main = new MainRespority<FindQQDataTable>(TecentDA);
             List<WaitGatherImage>  list= main.ExecuteSPSelect<WaitGatherImage>(sp, null).ToList();
            //采集到的头像URL可能存在相同的需要进行过滤
            /*
             头像URL采集需要过滤掉 同一个qq没有更换头像，以及同一个头像多个qq使用的情况
             */
             List<WaitGatherImage> result = new List<WaitGatherImage>();
             foreach (WaitGatherImage item in list)
             {
                 if (!result.Any(url => url.HeadImageUrl == item.HeadImageUrl))
                 {
                     result.Add(item);
                 }
             }
             return result;
        }
        string GatherImage(string url,string uin)
        {
            WebResponse response = HttpClientExtend.HttpWebRequestGet(url);
            Stream st = response.GetResponseStream();
            ImageHelper img = new ImageHelper();
            MemoryStream ms = img.ImageZip(st, 100);
            string fileName=uin+"_"+ DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
            string file = img.SaveImage(ms, ImageDir + "\\" + GetRelativePath,  fileName);
            if (!string.IsNullOrEmpty(file))
            {
                return GetRelativePath + "\\" + fileName;
            }
            return string.Empty;
        }
        public void DownLoadImage() 
        {
            List<WaitGatherImage> waits = GetWaitGatherImageData();
            foreach  (WaitGatherImage item in waits)
            {
                string path= GatherImage(item.HeadImageUrl,item.Uin);
                string sp = string.Empty;
                if (!string.IsNullOrEmpty(path))
                {
                    sp = string.Format("exec [SP_SuccessGatherImageList] '{0}','{1}'", item.Id,path);
                  
                }
                else 
                {
                    sp = string.Format("exec [SP_ErrorGatherImageList] '{0}'", item.Id);
                }
                MainRespority<FindQQDataTable> main = new MainRespority<FindQQDataTable>(TecentDA);
                main.ExecuteSPNoQuery(sp, null);
            }

        }
    }
}
