using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace CefSharpWin
{
    [Serializable]
    [XmlRoot(ElementName = "AppSetting")]
    public class AppSetting
    {
        public SystemAppSetting SystemSetting { get; set; }
        public UseAppSetting UseSetting { get; set; }
    }
    
    [Serializable]
    [XmlRoot(ElementName = "SystemSetting")]
    public class SystemAppSetting
    {
        [System.ComponentModel.Description("相对稳定的")]
        [XmlElement(ElementName = "Keep")]
        public Keep KeepStatic { get; set; }
        [System.ComponentModel.Description("随时会变化的")]
        [XmlElement(ElementName = "RealTime")]
        public RealTime AnyTimeValue { get; set; }

    }
    [Serializable]
    [XmlRoot(ElementName = "Keep")]
    public class Keep
    {
        public int OneClickValue { get; set; }
        public bool CreatePatientAutoClose { get; set; }
        public string DateFormatInUI { get; set; } 
        [System.ComponentModel.Description("车站URL")]
        public string StationAPI { get; set; }
        public string StationUrlRegexFormat { get; set; }
        public string StationJsonFile { get; set; }
        public int StationField { get; set; }
        public string TicketQueryAPI { get; set; }
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
