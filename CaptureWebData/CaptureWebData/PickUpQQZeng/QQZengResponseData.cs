using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaptureWebData
{
    public class QQZengResponseData
    {
        public string ip { get; set; }
        /// <summary>
        /// 大洲
        /// </summary>
        public string continent { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        public string district { get; set; }
        /// <summary>
        /// 运营商
        /// </summary>
        public string isp { get; set; }
        /// <summary>
        /// 区划
        /// </summary>
        public string areacode { get; set; }
        /// <summary>
        /// 英文
        /// </summary>
        public string en { get; set; }
        /// <summary>
        /// 简码
        /// </summary>
        public string cc { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string lng { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public string lat { get; set; }
        /// <summary>
        /// IP地址库日期版本
        /// </summary>
        public string version { get; set; }
    }
    public class QQZengResponse
    {
        public int code { get; set; }
        public QQZengResponseData data { get; set; }
    }
}
