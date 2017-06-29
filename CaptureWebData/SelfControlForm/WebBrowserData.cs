using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebKit;

namespace SelfControlForm
{
    public partial class WebBrowserData : UserControl
    {
        /// <summary>
        /// 获取到的请求头
        /// </summary>
        public string GatherRequestHeader { get; set; }
        public delegate void CallBack(object data);
        /// <summary>
        /// 触发的回调事件
        /// </summary>
        public CallBack Call { get; set; }
        public string Cookie { get; set; }
        enum BrowserTab 
        {
            IE=1,
            WebKit=2
        }
        public WebBrowserData()
        {
            InitializeComponent();
            Init();
            ClearCookie();
        }

        private void WebBrowserData_Load(object sender, EventArgs e)
        {

        }
        string PickUpCookie() 
        {

            BrowserTab bt = IdentifySelectBrowserTab();
            string cookie = string.Empty;
            switch (bt)
            {
                case BrowserTab.IE:
                    cookie= PickUpFromIE();
                    break;
                case BrowserTab.WebKit:
                    cookie = PickUpFromWebKit();
                    break;
            }
            return cookie;
        }
        string PickUpFromIE() 
        {
            WebBrowser wb = webBrowserIE;
            if (wb.Document == null)
            {//没有加载网页
                rtbTip.Text = "网页尚未加载不能提取cookie";
                return string.Empty;
            }
            string cookie = wb.Document.Cookie;
            //这里需要提取出请求的request header  接下来其他请求需要这里面的部分信息作为参数
            object obj = wb.ActiveXInstance;
            rtbIeCookie.Text= cookie;
            return cookie;
        }
        string PickUpFromWebKit()
        {
            WebKitBrowser wkb = PickUpWebKit();
            WebKit.DOM.Document doc = wkb.Document;
            if (doc == null)
            {
                rtbTip.Text = "当前面板页中尚未加载网页";
                return string.Empty; ;
            }
            wkb.IsScriptingEnabled = true;
            string cookie= wkb.StringByEvaluatingJavaScriptFromString("document.cookie");
            rtbWebKitCookie.Text = cookie;
            return cookie;
        }
        WebKitBrowser PickUpWebKit() 
        {
            Control ele = GetElementInWebKitTab();
            if (ele == null)
            {
                rtbTip.Text = "当前选中面板中没有浏览页";
                return null;
            }
            return (WebKitBrowser)ele;
        }
        /// <summary>
        /// 识别当前打开页的浏览器内核类别
        /// </summary>
        /// <returns></returns>
        BrowserTab IdentifySelectBrowserTab() 
        {
            TabPage tab = tabBrowser.SelectedTab;
            string tag = tab.Tag as string;
            BrowserTab bt;
            Enum.TryParse(tag, out bt);
            return bt;
        }
        void PickUp_UsingCookie(WebBrowser wb,string url,string cookie) 
        {
            if (!string.IsNullOrEmpty(cookie)) 
            {
                wb.Document.Cookie = cookie;
            }
            wb.Url = new Uri(url);
        }

        private void btnPickUp_UseCookie_Click(object sender, EventArgs e)
        {
            string url = rtbUrl.Text;
            Cookie = rtbIeCookie.Text;
            BrowserTab tab = IdentifySelectBrowserTab();
            switch (tab)
            {
                case BrowserTab.IE:
                    PickUp_UsingCookie(webBrowserIE, url, Cookie);
                    break;
                case BrowserTab.WebKit:
                    WebKitBrowser wkb = PickUpWebKit();
                    if (wkb == null)
                    {
                        wkb = InitWebKit();
                    }
                    SetIeCookieToWebKit(rtbUrl.Text, Cookie, wkb);
                    break;
            }
        }
        void SetIeCookieToWebKit(string url,string cookie,WebKitBrowser webkit) 
        {
           //如何设置webkit的cookie
            webkit.Navigate(url);
        }
        private void btnBrowser_Click(object sender, EventArgs e)
        {
            string url = rtbUrl.Text;
            GotoWebPage(url, webBrowserIE);
        }
        void GotoWebPage(string url, WebBrowser wb)
        {
            wb.Url = new Uri(url);
        }

        private void btnPickUpCookie_Click(object sender, EventArgs e)
        {
            string url = rtbUrl.Text;
            Cookie= PickUpCookie();
        }
        /// <summary>
        /// 从容器中查找出谷歌内核
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Control GetElementInWebKitTab()
        {
            TabPage page = tabWebKit;
            ControlCollection list = page.Controls;
            foreach (Control item in list)
            {
                if (item.Name == typeof(WebKitBrowser).Name)
                {
                    return item;
                }
            }
            return null;
        }
        private WebKitBrowser InitWebKit() 
        {
            TabPage page = tabWebKit;
            Control ele = GetElementInWebKitTab();
            if (ele == null)
            {
                ele = new WebKitBrowser();
                ele.Dock = DockStyle.Fill;
                page.Controls.Add(ele);
            }
            return (WebKitBrowser)ele;
        }
        private void Init() 
        {
            
            //string url = rtbUrl.Text;
            //webBrowserIE.Url = new Uri(url);
            //WebKitBrowser wkb = InitWebKit();
            //wkb.Navigate(url);
        }
        public void ClearCookie() 
        {
            if (webBrowserIE.Document != null) 
            {
                webBrowserIE.Document.Cookie = string.Empty;
            }
        }
        private void btnCallBack_Click(object sender, EventArgs e)
        {
           if(string.IsNullOrEmpty(Cookie)){Cookie=PickUpCookie();}
            if (Call != null) 
            {
                Call(Cookie);
            }
        }
    }
}
