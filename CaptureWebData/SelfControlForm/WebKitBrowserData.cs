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
    public partial class WebKitBrowserData : UserControl
    {
        public WebKitBrowserData()
        {
            InitializeComponent();
        }

        private void WebKitBrowserData_Load(object sender, EventArgs e)
        {

        }
        void GetSelectTaPanel() 
        {
            TabPage page = tab.SelectedTab;
            Control ele = GetElement();
            WebKitBrowser wkb;
            if (ele == null) 
            {
                ele = new WebKitBrowser();
                ele.Dock = DockStyle.Fill;
                page.Controls.Add(ele);
            }
            wkb = (WebKitBrowser)ele;
            string url = rtbUrl.Text;
            wkb.Navigate(url);
        }
        /// <summary>
        ///从容器中查找出谷歌内核
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Control GetElement() 
        {
            TabPage page = tab.SelectedTab;//获取当前的选择页
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

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            GetSelectTaPanel();
        }

        private void btnPickUpCookie_Click(object sender, EventArgs e)
        {
            Control ele = GetElement();
            if (ele == null) 
            {
                rtbTip.Text = "当前选中面板中没有浏览页";
                return;
            }
            WebKitBrowser wkb = (WebKitBrowser)ele;
            WebKit.DOM.Document doc = wkb.Document;
            if (doc == null) 
            {
                rtbTip.Text = "当前面板页中尚未加载网页";
                return;
            }
            wkb.IsScriptingEnabled = true;
            string cookie = wkb.StringByEvaluatingJavaScriptFromString("document.cookie");
            rtbCookie.Text = cookie;
            //object obj = wkb.Document as System.Windows.Forms.HtmlDocument;
           //object obj= wkb.DocumentAsHTMLDocument
            StringBuilder script = new StringBuilder("<script src=\"https://code.jquery.com/jquery-3.1.1.min.js\"></script>");
            script.Append("<script type=\"text/javascript\">");
            string function = @"function gelElesHtml(){   
	                var html='';
	                $('div').each(function(i,ele){
		                html+=ele.innerHTML;
	                });
	                return html;
                };
                gelElesHtml();
";
            script.Append(function);
            script.Append("</script>");
            wkb.Invoke((MethodInvoker)delegate
            {
              
            });
           
        }
    }
}
