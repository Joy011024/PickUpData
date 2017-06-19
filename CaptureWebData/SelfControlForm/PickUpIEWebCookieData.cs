using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SelfControlForm
{
    public delegate void PickUpCookieCallBack(object data);
    public partial class PickUpIEWebCookieData : UserControl
    {
        /// <summary>
        /// 提取cookie触发的回调事件
        /// </summary>
        public PickUpCookieCallBack CallBack { get; set; }
        public string Cookie { get;private set; }
        enum DoAction 
        {
            GotoUrl=1,
            PickUpCookie=2,
            RemoveCookie=3,
            ClearCookie=4
        }
        void InitElement()
        {
            btnClearCookie.Tag = DoAction.ClearCookie.ToString();
            btnGoto.Tag = DoAction.GotoUrl.ToString();
            btnRemoveCookie.Tag = DoAction.RemoveCookie.ToString();
            btnPickUp.Tag = DoAction.PickUpCookie.ToString();
            EventHandler btnClick = new EventHandler(Button_click);
            btnGoto.Click += btnClick;
            btnClearCookie.Click += btnClick;
            btnPickUp.Click += btnClick;
            btnRemoveCookie.Click += btnClick;
        }
        public void SetGoWeb(string url) 
        {
            GotoWeb(url);
        }
        public PickUpIEWebCookieData()
        {
            InitializeComponent();
            InitElement();
        }
        private void Button_click(object sender,EventArgs e) 
        {
            Button btn=sender as Button;
            string tag = btn.Tag as string;
            DoAction da;
            Enum.TryParse(tag, out da);
            switch (da)
            {
                case DoAction.GotoUrl:
                    GotoWeb(rtbUrl.Text);
                    break;
                case DoAction.PickUpCookie:
                    PickUpCookie();
                    break;
                case DoAction.RemoveCookie:
                    RemoveCookieText();
                    break;
                case DoAction.ClearCookie:
                    ClearCookie();
                    break;
                default:
                    break;
            }
        }
        private void GotoWeb(string url) 
        {
            web.Url = new Uri(url);
        }
        private void RemoveCookieText() 
        {
            rtbCookie.Text = string.Empty;
        }
        private void ClearCookie() 
        {
            if (web.Document != null)
            {
                web.Document.Cookie = string.Empty;
            }
        }
        private void PickUpCookie() 
        {
            if (web.Document != null)
            {
                Cookie = web.Document.Cookie;
                rtbCookie.Text = Cookie;
                rtbTip.Text = string.Empty;
            }
            else 
            {
                rtbTip.Text = "页面尚未加载。";
            }
            if (CallBack != null) 
            {
                CallBack(Cookie);
            }
        }
    }
}
