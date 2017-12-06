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
        /// <summary>
        /// 每个文件夹下最大存储的文件数目
        /// </summary>
        public int FolderFilesMaxNumder 
        {
            get 
            {
                int def = 0;
                string str = ConfigurationManager.AppSettings["FolderFilesMaxNumder"];
                int.TryParse(str, out def);
                if (def <= 1)
                {
                    def = 200;
                }
                return def;
            }
        }
        List<WaitGatherImage> GetWaitGatherImageData()
        {
            DateTime today = DateTime.Now;
            string sp = string.Format("exec [SP_GetWaitGatherImageList] ", today);
            MainRespority<FindQQDataTable> main = new MainRespority<FindQQDataTable>(TecentDA);
            List<WaitGatherImage> list = main.ExecuteSPSelect<WaitGatherImage>(sp, null).ToList();
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
            //如何判断响应是否正常
            string type= response.ContentType;
            //响应内容是否为预期
            Stream st = response.GetResponseStream();
            ImageHelper img = new ImageHelper();
            MemoryStream ms;
            string imgType = "Zip_";
            try
            {
                if (isZipImage)
                {
                    ms = img.ImageZip(st, 100);
                }
                else
                {
                    ms = img.OriginImage(st);
                    imgType = "Ori_";
                }
            }
            catch (Exception ex)
            { //此处存在的bug http://q3.qlogo.cn/g?b=qq&k=jOSrVyJKynwRulkkbBRZfw&s=100&t=1496888983 
                //虽然该URL在浏览器上可以打开，但是打开的图片不可用，不能转换为图片进行存储
                return string.Empty;
            }
            string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string address = "Null";
            string relative = address ;
            string fileName = imgType+address + "_" + uin + "_" + time + ".jpg";
            if (!string.IsNullOrEmpty(local) && local != LocalCityJoinSign)
            {
                string[] asd = local.Split(new string[] { LocalCityJoinSign }, StringSplitOptions.None);
                address = asd[0];
                if (!string.IsNullOrEmpty(address))
                {
                    relative = address + "\\" + asd [1];//获取所在的省份作为文件目录
                    fileName = imgType + local + "_" + uin + "_" + time + ".jpg";
                }
                else if (asd.Length == 1)
                {
                    address += "\\" + address;
                }
            }
            //判断当前相对路径下存储的文件数量是否已经到达限定数
            string imgDir = ImageDir;
            string folderFullName = imgDir + relative + "\\";
            object[] folder = GetLastCreateFolder(folderFullName);
            if (folder == null)
            {
                folderFullName += "1";
            }
            else 
            {//判断文件夹中文件数量是否达到限定
                string dir = folder[0] as string;
                int fileSize = GetFolderFileSize(dir);
                if (fileSize >= FolderFilesMaxNumder)
                {
                    folderFullName += ((int)folder[1]) + 1;
                }
                else {
                    folderFullName = dir;
                }
            }
            string file = img.SaveImage(ms, folderFullName, fileName);
            if (!string.IsNullOrEmpty(file))
            {
                return folderFullName.Replace(imgDir,"") + "\\" + fileName;
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取上次创建的文件夹
        /// </summary>
        /// <param name="parentFullName">上级文件夹全路径</param>
        /// <returns>数组 =最后创建的文件夹名称,总共文件夹数量</returns>
        object[] GetLastCreateFolder(string parentFullName)
        {//获取文件夹下子文件夹内最后更新的文件夹路径名称信息
            if (!Directory.Exists(parentFullName))
            {
                return null;
            }
            DirectoryInfo dis = new DirectoryInfo(parentFullName);
            DirectoryInfo[] folders = dis.GetDirectories();
            if (folders.Length == 0)
            { //没有文件夹
                return null;
            }
            //此处需要添加一个日志进行耗时分析【以便进行性能改善】
            DateTime ct = folders.Max(d => d.CreationTime);
            DirectoryInfo target= folders.Where(f => f.CreationTime == ct).FirstOrDefault();
            object[] info=new  object[]{target.FullName,folders.Length};
            return info;
        }
        /// <summary>
        /// 文件夹下文件数目
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        int GetFolderFileSize(string folder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            return di.GetFiles().Length;
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
                if (string.IsNullOrEmpty(item.HeadImageUrl))
                {
                    continue;
                }
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
