using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
namespace CefSharpWin
{
    public partial class Form1 : Form
    {
        ChromiumWebBrowser browser = null;
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        void Init()
        {
            CefSettings setting = new CefSettings()
            {
                Locale = "zh-cn",
                AcceptLanguageList = "zh-cn",
                MultiThreadedMessageLoop = true
            };
            Cef.Initialize(setting);
            browser = new ChromiumWebBrowser("https://www.baidu.com/");
            //第一步进行登录
            txtUrl.Text = SystemConfig.MainUrl;// "https://www.cnblogs.com/ZuoJinLiang/p/7490497.html";
            layoutPanel.Controls.Add(browser);
            RequestHandler_new handle  = new RequestHandler_new(browser.RequestHandler);//登录成功之后进行cookie提取
            handle.GetCookieResponse = CookieHandle.FillCookieContainer;
            browser.RequestHandler = handle;
            browser.Dock = DockStyle.Fill;
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            browser.Load(txtUrl.Text);
        }
        /// <summary>
        /// 【setup4】获取联系人列表
        /// </summary>
        void GetContacter()
        {

        }
    }

    

}
