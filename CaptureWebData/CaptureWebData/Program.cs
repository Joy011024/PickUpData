using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CaptureWebData
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Test();
            Application.Run(new TecentDataFrm());
        }
        static void Test() 
        {
           // CityDataManage cm = new CityDataManage();
            //cm.ImportDB(@"D:\Dream\ExcuteHttpCmd\CaptureWeb\CaptureWebData\CaptureWebData\Wait\国家城市区域名称.txt");
            PrepareParam.PrepareHeader();
            
        }
        
    }
}
