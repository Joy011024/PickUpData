using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FeatureFrmList
{
    public partial class MicrosoftBrowser : UserControl
    {
        bool? RemberOriginSize;
        int originWidth, originHeight;
        int containerWidth, containerHeight;
        public string Cookie { get; private set; }
        public MicrosoftBrowser()
        {
            InitializeComponent();
            CalculatePoint();
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
        }
        /// <summary>
        /// 设置页面大小
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetPage(int width,int height) 
        {
            if (width < containerWidth || height < containerHeight)
            {//容器最小像素 
            
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
            
        }
        void BtnGetCookie_Click(object sender, EventArgs e)
        {
            HtmlDocument doc = MicrosoftWebBrowser.Document;
            if (doc == null) { return; }
            Cookie = doc.Cookie;
            rtbCookie.Text = Cookie;
        }
    }
}
