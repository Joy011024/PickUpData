using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace NPKVersionNote
{
    public class ConfigItem
    {
        public static bool NPKCategoryFromDB 
        {
            get {
                return ConfigurationManager.AppSettings["NPKCategoryReadDB"] == "true" ? true : false;
            }
        }
        /// <summary>
        /// 服务的URL
        /// </summary>
        public static string NpkServiceUrl {
            get
            {
                return ConfigurationManager.AppSettings["NpkServiceUrl"];
            }
        }
        /// <summary>
        /// 提交文件接口部分（不含有服务的URL）
        /// </summary>
        public static string SubmitNpkFileUrl 
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitNpkFileUrl"];
            }
        }
    }
}
