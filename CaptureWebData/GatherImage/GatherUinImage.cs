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

        public string LocalCityJoinSign 
        {
            get
            {
                return ConfigurationManager.AppSettings["LocalCityJoinSign"];
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
        string GatherImage(string url, string uin, string local, bool isZipImage)
        {
            WebResponse response = HttpClientExtend.HttpWebRequestGet(url);
            Stream st = response.GetResponseStream();
            ImageHelper img = new ImageHelper();
            MemoryStream ms;
            string imgType = "Zip_";
            if (isZipImage)
            {
                ms = img.ImageZip(st, 100);
            }
            else 
            {
                ms = img.OriginImage(st);
                imgType = "Ori_";
            }
            string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string address = "Null";
            string relative = address + "\\" + GetRelativePath;
            string fileName = imgType+address + "_" + uin + "_" + time + ".jpg";
            if (!string.IsNullOrEmpty(local) && local != LocalCityJoinSign)
            {
                string[] asd = local.Split(new string[] { LocalCityJoinSign }, StringSplitOptions.None);
                address = asd[0];
                if (!string.IsNullOrEmpty(address))
                {
                    relative = address + "\\" + asd [1]+"\\"+ GetRelativePath;//获取所在的省份作为文件目录
                    fileName = local + "_" + uin + "_" + time + ".jpg";
                }
            }
            string file = img.SaveImage(ms, ImageDir + "\\"+relative, fileName);
            if (!string.IsNullOrEmpty(file))
            {
                return relative + "\\" + fileName;
            }
            return string.Empty;
        }
        /// <summary>
        /// 下载的图片是否为压缩图
        /// </summary>
        /// <param name="isZipImage">压缩图</param>
        /// <returns></returns>
        public List<string> DownLoadImage(bool isZipImage) 
        {
            List<WaitGatherImage> waits = GetWaitGatherImageData();
            List<string> paths = new List<string>();
            foreach  (WaitGatherImage item in waits)
            {
                string path= GatherImage(item.HeadImageUrl,item.Uin, item.LocalCity,isZipImage);
                string sp = string.Empty;
                if (!string.IsNullOrEmpty(path))
                {
                    sp = string.Format("exec [SP_SuccessGatherImageList] '{0}','{1}'", item.Id,path);
                    paths.Add(path);
                }
                else 
                {
                    sp = string.Format("exec [SP_ErrorGatherImageList] '{0}'", item.Id);
                }
                MainRespority<FindQQDataTable> main = new MainRespority<FindQQDataTable>(TecentDA);
                main.ExecuteSPNoQuery(sp, null);
            }
            return paths;
        }
    }
}
