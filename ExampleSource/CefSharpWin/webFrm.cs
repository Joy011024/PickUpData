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
using PureMVC.Interfaces;
namespace CefSharpWin
{
    public partial class WebFrm : FormMediatorService
    {
        ChromiumWebBrowser browser = null;
        public WebFrm()
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
            browser = new ChromiumWebBrowser("https://kyfw.12306.cn/otn/login/init");
            HttpRequestFlag.Preparelogin();
            //第一步进行登录
            txtUrl.Text = SystemConfig.MainUrl;// "https://www.cnblogs.com/ZuoJinLiang/p/7490497.html";
            layoutPanel.Controls.Add(browser);
            RequestHandler_new handle = new RequestHandler_new(browser.RequestHandler);//登录成功之后进行cookie提取
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

        private void btnGetCookie_Click(object sender, EventArgs e)
        {
            //提取cookie
            IFrame frame = browser.GetMainFrame();

        }
        public override IList<string> ListNotificationInterests()
        {
            return new string[] {
                NotifyList.Notify_Close_Account
            };
        }
        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case NotifyList.Notify_Close_Account:
                    /*
                     System.InvalidOperationException:“线程间操作无效: 从不是创建控件“Form1”的线程访问它。”

                     */

                    CloseFrom();
                    break;
            }
        }
        private void CloseFrom()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    CloseFrom();
                }));
                return;
            }
            Close();
        }
    }
}
