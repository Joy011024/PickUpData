using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class AjaxRequestParam
    {
        /// <summary>
        /// ajax类型【Get,Post】
        /// </summary>
        public string AjaxType { get; set; }
        /// <summary>
        /// 请求的URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 请求的参数（json串）
        /// </summary>
        public string JsonString { get; set; }
        /// <summary>
        /// json数据如果在后台需要转换为对象，则对应的对象名
        /// </summary>
        public string JsonType { get; set; }
        /// <summary>
        /// ajax请求的cookie信息
        /// </summary>
        public string Cookie { get; set; }
    }
    public class AjaxResponseData 
    {
        public string JsonString { get; set; }
        public object JsonData { get; set; }
    }
}
