﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace CefSharpWin
{
    public class SystemSetting
    {
         public static Dictionary<string, string> SystemSettingDict { get; set; }
    }
    
    [Serializable]
    [XmlRoot(ElementName = "RealTime")]
    public class RealTime
    {
        public int VisiterIndex { get; set; }
        public int VisterDayInt { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "UseSetting")]
    public class UseAppSetting
    {
        public string EleFlagRGB { get; set; }
        public string GridRowFlagRGB { get; set; }
        public virtual void ConvertRGB(string rgbStr)
        {//使用的分割符 |
            string[] rgb = rgbStr.Split('|');
            List<int> rgbInt = new List<int>();
            foreach (var item in rgb)
            {
                int temp = 0;
                if (int.TryParse(item, out temp))
                {

                }
            }
        }
    }
    
   
}
