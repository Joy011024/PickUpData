using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefSharpWin
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
            //InitSQLiteManage.SyncCityData2SQLite();
            XmlService.GetAppSetting();

            TestRegex();
            InitFakeServices();
            InitRegisterForm();
            Form acc = FacadeFactory.Instance.RetrieveMediator(typeof(WebFrm).Name) as Form;
           
            Application.Run(acc);//cef 只能单进程

            /*
             System.Exception:“CEF can only be initialized once per process. This is a limitation of the underlying CEF/Chromium framework.
             You can change many (not all) settings at runtime through RequestContext.SetPreference. 
             See https://github.com/cefsharp/CefSharp/wiki/General-Usage#request-context-browser-isolation Use Cef.IsInitialized to guard against this exception. 
             If you are seeing this unexpectedly then you are likely calling Cef.Initialize after you've created an instance of ChromiumWebBrowser,
             it must be before the first instance is created.”

             */
        }
        static void TestRegex()
        {
            string reg = "<table class=\"table table-bordered table-striped table-hover\">(.*?)</table";
            string input = "<table><tr>测试数据</tr></table>";
            string file = Domain.CommonData.FileHelper.ReadFile("Dev\\IPProxyTemplate.txt");
            RegexHelper.GetMatchValue(file, reg);
        }
        static void InitRegisterForm()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            WebFrm acc = new WebFrm();
            MoniterTicket mt = new MoniterTicket();
        }
        static void InitFakeServices()
        {
            new System.Threading.Thread(() =>
            {
                try
                {
                    FakeIPService.SwitcHttphPrxoy();
                    FakeIPService.GetFakeIPs();
                }
                catch (Exception ex)
                {

                }
            }).Start();

            
        }
    }
}
