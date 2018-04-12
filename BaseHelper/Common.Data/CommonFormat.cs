using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public enum EDateTimeFormat
    {
        Value=1,
        Format=2
    }
    public class CommonFormat
    {
        /// <summary>
        /// 精确到微秒
        /// </summary>
        public static string DateTimeMilFormat = "yyyy-MM-dd HH:mm:ss FFF";
        /// <summary>
        /// 精确到微秒
        /// </summary>
        public string DateTimeMilFormatString { get { return DateTime.Now.ToString(DateTimeMilFormat); } }
        /// <summary>
        /// 格式化
        /// </summary>
        public static string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 值
        /// </summary>
        public string DateTimeFormatString = DateTime.Now.ToString(DateTimeFormat);
        public static string DateTimeIntFormat = "yyyyMMddHHmmss";
        /// <summary>
        /// 值
        /// </summary>
        public string DateTimeIntFormatString = DateTime.Now.ToString(DateTimeIntFormat);
        public static string DateFormat = "yyyy-MM-dd";
        /// <summary>
        /// 值
        /// </summary>
        public string DateFormatString = DateTime.Now.ToString(DateTimeFormat);
        public static string DateIntFormat = "yyyyMMdd";
        /// <summary>
        /// 值
        /// </summary>
        public string DateIntFormatString = DateTime.Now.ToString(DateIntFormat);
        public static string DateToMinuteIntFormat = "yyyyMMddHHmm";
        /// <summary>
        /// 值
        /// </summary>
        public string DateToMinuteFormatString = DateTime.Now.ToString(DateToMinuteIntFormat);
        public static string DateToHourIntFormat = "yyyyMMddHH";
        public string DateToHourIntFormatString = DateTime.Now.ToString(DateToHourIntFormat);
    }
    public class DefaultSpecialSignAppConfig 
    {
        /// <summary>
        /// 集合连接符
        /// </summary>
        public static string JoinSign = "|";
        public static string[] DirectorySplitSign={"/","\\"};
        public static string DotSign = ".";
    }
}
