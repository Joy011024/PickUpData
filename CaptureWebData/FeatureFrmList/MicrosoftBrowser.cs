﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FeatureFrmList
{
    /// <summary>
    /// 按钮点击之后触发回调
    /// </summary>
    /// <param name="data"></param>
    public delegate void ButtonClickAfetr(object data);
    
    public partial class MicrosoftBrowser : UserControl
    {
        /// <summary>
        /// 是否存储原始的窗体尺寸
        /// </summary>
        bool? RemberOriginSize;
        /// <summary>
        /// 原始窗体尺寸大小
        /// </summary>
        int originWidth, originHeight;
        /// <summary>
        /// 容器尺寸
        /// </summary>
        int containerWidth, containerHeight;
        /// <summary>
        /// 将进行加载的URL
        /// </summary>
        public string Url { get;private set; }
        /// <summary>
        /// 加载URL获取到的cookie
        /// </summary>
        public string Cookie { get; private set; }
        /// <summary>
        /// 采集cookie回调事件
        /// </summary>
        public ButtonClickAfetr PickUpCookieCallBack { get; set; }
        public void RefreshUrl(string url)
        {
            Url = url;
            rtbUrl.Text = url;
            btnGo.PerformClick();
            //btnGetCookies.PerformClick();
        }
        public MicrosoftBrowser()
        {
            InitializeComponent();
            CalculatePoint();
        }
        public MicrosoftBrowser(string url)
        {
            InitializeComponent();
            CalculatePoint();
            Url = url;
            rtbUrl.Text = url;
        }
        void CalculatePoint() 
        {
            if (!RemberOriginSize.HasValue)
            {
                Size s = this.Size;
                originHeight = s.Height;
                originWidth = s.Width;
            }
            containerHeight = originHeight;
            containerWidth = originWidth;
            btnGo.Click += new EventHandler(BtnGo_Click);
            btnGetCookies.Click += new EventHandler(BtnGetCookie_Click);
            MicrosoftWebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Web_DocumentComplated);
        }
        /// <summary>
        /// 设置页面大小
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetPage(int width,int height) 
        {
            if (width > containerWidth && height > containerHeight)
            {//容器最小像素 
                this.Size = new Size(width, height);
                MicrosoftWebBrowser.Height = this.Size.Height - 70;
                MicrosoftWebBrowser.Width = this.Size.Width - 10;
            }

        }
        void BtnGo_Click(object sender, EventArgs e)
        {
            string url = rtbUrl.Text;
            if (string.IsNullOrEmpty(url))
            {
                return;
            }
            MicrosoftWebBrowser.Url = new Uri(url);
            /*
             可能出现异常：
             * 请求的资源在使用中。 (异常来自 HRESULT:0x800700AA)
             */
        }
        void BtnGetCookie_Click(object sender, EventArgs e)
        {
            HtmlDocument doc = MicrosoftWebBrowser.Document;
            if (doc == null) { return; }
            Cookie = doc.Cookie;
            rtbCookie.Text = Cookie;
            if (PickUpCookieCallBack != null)
            {
                PickUpCookieCallBack(new HtmlItem() { Cookie = Cookie });
            }
        }
        private void Web_DocumentComplated(object sender, EventArgs e)
        { 
            //使用回调事件来接收触发完成事件
            HtmlDocument doc= MicrosoftWebBrowser.Document;
            WebBrowserDocumentCompletedEventArgs eve = e as WebBrowserDocumentCompletedEventArgs;
            if (MicrosoftWebBrowser.ReadyState != WebBrowserReadyState.Complete|| (!eve.Url.IsAbsoluteUri||eve.Url.LocalPath == "false;")||eve.Url.AbsoluteUri!=Url)
            { //网页加载成功触发，并且兼容多框架调用情形【应该是类似于iframe】，不考虑这兼容模式的话会出现JavaScript触发完成的情况
                return;
            }
            //经过上述语句实现调用仅仅来自网页而非脚本
            HtmlItem data = new HtmlItem();
            if (doc != null)
            {
                data.Cookie = doc.Cookie;
                data.Html = doc.Body.OuterHtml;
                data.Domain = doc.Domain;
                data.Url = doc.Url.ToString();
            }
            if (PickUpCookieCallBack != null)
            {
                PickUpCookieCallBack(data);
            }
        }
    }
    public class HtmlItem
    {
        public string Cookie { get; set; }
        public string Html { get; set; }
        /// <summary>
        /// cookie的作用域
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 请求的URL
        /// </summary>
        public string Url { get; set; }
    }
    public class DrawWebBrowserInFromEle
    { 
        //将自定义的控件绘制到元素上
        MicrosoftBrowser mb;
        /// <summary>
        /// 将元素渲染到父容器中并进行加载调度
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="LoadUrlComplateEvent"></param>
        /// <param name="url"></param>
        public DrawWebBrowserInFromEle(Control parent, ButtonClickAfetr LoadUrlComplateEvent, string url)
        { //渲染的父容器
            mb = new MicrosoftBrowser();
            mb.Parent = parent;
            mb.Visible = true;
            mb.SetPage(mb.Parent.Size.Width - 20, mb.Parent.Size.Height - 20);
            mb.PickUpCookieCallBack = LoadUrlComplateEvent;
            if (!string.IsNullOrEmpty(url))
                mb.RefreshUrl(url);
        }
        /// <summary>
        /// 页面重新加载
        /// </summary>
        /// <param name="url"></param>
        public void RefreshUrl(string url) 
        {
            mb.RefreshUrl(url);
        }
    }
}
