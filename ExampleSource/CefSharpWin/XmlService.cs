using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CefSharpWin
{
    public class XmlService
    {
        static Grids gridSetting;
        [System.ComponentModel.Description("获取表格列配置")]
        public static Grids GetGridSetting()
        {
            if (gridSetting != null)
            {
                return gridSetting;
            }
            string xml = SystemConfig.DebugDir+ "12306\\ConstGridColumn.xml";
            gridSetting= XmlSerializerHelper.LoadFromXML<Grids>(xml);
            return gridSetting;
        }
        static AppSetting appSetting;
        public static AppSetting GetAppSetting()
        {
            if (appSetting != null)
            {
                return appSetting;
            }
            string xml = SystemConfig.DebugDir + "12306\\AppSetting.xml";
            appSetting = XmlSerializerHelper.LoadFromXML<AppSetting>(xml);
            return appSetting;
        }
    }
   
}
