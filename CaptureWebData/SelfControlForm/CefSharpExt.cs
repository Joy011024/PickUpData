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
    public partial class CefSharpExt : UserControl
    {
        public CefSharpExt()
        {
            InitializeComponent();
            Init();
        }
       // ChromiumWebBrowser web;
        void Init() 
        {
            //web = new ChromiumWebBrowser();
            //web.Parent = panelBody;
            btnGo.Click += new EventHandler(BtnGo_Click);
        }
        void BtnGo_Click(object sender,EventArgs e) 
        {
            //web.Load(txtUrl.Text);
        }
        //提取cookie

    }
}
