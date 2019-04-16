using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
//using EmailHelper;
using Domain.CommonData;
using CaptureManage.AppWin;
using Infrastructure.ExtService;
using System.Text;
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
            BackstageFacade bs = new BackstageFacade();
            Application.Run(new TecentDataFrm());
        }
        static void Test() 
        {
            
            DataLink dl = new DataLink();
            StringBuilder tip = new StringBuilder();
            tip.AppendLine("time:\t" + DateTime.Now.ToString(SystemConfig.DateTimeFormat));
            tip.AppendLine("App :\t CaptureWebData");
            tip.AppendLine("Event:run app");
            dl.SendDataToOtherPlatform(LanguageItem.Tip_PickUpErrorlockAccount, tip.ToString());//需要知道当前在进行采集的账户
            PickUpTianMaoHtml tm = new PickUpTianMaoHtml();
            string dir= new AppDirHelper().GetAppDir(AppCategory.WinApp);
            tm.DoHtmlFileAnalysis(dir + @"\HttpResponse\list.tmall.com\HttpResponse");
        }
        
    }
   
}
