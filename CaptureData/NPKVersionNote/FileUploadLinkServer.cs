using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.IO;
namespace NPKVersionNote
{
    /// <summary>
    /// 将文件上传到远程服务器
    /// </summary>
    public class FileUploadLinkServer
    {
        public void UploadFile(string path, string url, string requestHeader)
        {
            try
            {
                //HttpWebRequest httprequest = (HttpWebRequest)WebRequest.Create(url);
                 
                //Dictionary<String, string> dict = InsertDefaultHeader(requestHeader);
                //httprequest.ContentType = "application/x-www-form-unlencode;charset=utf-8";
                //httprequest.Method = "POST";
                //FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
                //BinaryReader br = new BinaryReader(file);
                //byte[] bs = br.ReadBytes((int)file.Length);
                //httprequest.ContentLength = bs.Length;
                //Stream response = httprequest.GetRequestStream();
                //if (response.CanWrite)
                //{//是否可以将文件提交到远程
                //    response.Write(bs, 0, bs.Length);
                //    br.Dispose();
                //    response.Dispose();
                //    file.Dispose();
                //}
                //传递 的方式1 OK  在web中使用Request.Filese来读取提交的文件
                WebClient client = new WebClient();
                client.UploadFile(url, "POST", path);//不支持 URI 格式。
            }
            catch (Exception ex) 
            {
            
            }
        }
        /// <summary>
        ///  获取请求头的字典形式（如果没有提供请求头则返回默认项）
        /// </summary>
        /// <param name="requestHeader"></param>
        /// <returns></returns>
        public Dictionary<String,string> InsertDefaultHeader(string requestHeader) 
        {
            string head = @"Accept:*/*
Accept-Encoding:gzip, deflate, sdch
Accept-Language:zh-CN,zh;q=0.8
Connection:keep-alive
User-Agent:Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36
X-Requested-With:XMLHttpRequest";
            if (!string.IsNullOrEmpty(requestHeader)) 
            {
                head = requestHeader;
            }
            string[] hs = head.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            Dictionary<string, string> request = new Dictionary<string, string>();
            foreach (var item in hs)
            {//每一组数据提取第一个":" 防止存在用户自定义数据导致影响请求
                int index = item.IndexOf(":");
                string key = item.Substring(0, index).Trim();
                if (key.StartsWith("Get", true, System.Globalization.CultureInfo.CurrentCulture))//进程驱动的语言环境
                {
                    continue;
                }
                if (key.StartsWith("Post", true, System.Globalization.CultureInfo.CurrentCulture))//进程驱动的语言环境
                {
                    continue;
                }
                if (key.StartsWith("Cookie", true, System.Globalization.CultureInfo.CurrentCulture))
                {
                    continue;
                }
                string value = item.Substring(index + 1);
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                request.Add(key, value);
            }
            return request;
        }
    }
}
