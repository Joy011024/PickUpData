using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataHelp
{
    public  static class ConvertDate
    {
        public static string ConvertDateFormat(string input, string nowFormat, string resultFormat) 
        {
            if (!string.IsNullOrEmpty(input))
            {
                return input;
            } 
            //当前格式是否可以转换为指定格式日期字符

            return input;
        }
        public static string ConvertAutoFormat(string time, string nowFormat,string resultFormat)
        {
            if (string.IsNullOrEmpty(time)) 
            {
                return time;
            }
            DateTime temp;
            bool pass = DateTime.TryParseExact(time, nowFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out temp);
            if (pass) 
            {
               time= temp.ToString(resultFormat);
            }
            return time;
        }
    }
}
