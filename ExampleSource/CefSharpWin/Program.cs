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
            InitRegisterForm();
            Form1 acc= FacadeFactory.Instance.RetrieveMediator(typeof(Form1).Name) as Form1;
            Application.Run(acc);//cef 只能单进程

            /*
             System.Exception:“CEF can only be initialized once per process. This is a limitation of the underlying CEF/Chromium framework. You can change many (not all) settings at runtime through RequestContext.SetPreference. See https://github.com/cefsharp/CefSharp/wiki/General-Usage#request-context-browser-isolation Use Cef.IsInitialized to guard against this exception. If you are seeing this unexpectedly then you are likely calling Cef.Initialize after you've created an instance of ChromiumWebBrowser, it must be before the first instance is created.”

             */
        }
        static void InitRegisterForm()
        {
            Form1 acc = new Form1();
            MoniterTicket mt = new MoniterTicket();
        }
    }
}
