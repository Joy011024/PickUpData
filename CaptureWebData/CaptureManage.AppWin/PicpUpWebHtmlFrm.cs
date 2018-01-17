using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HttpClientHelper;
using FeatureFrmList;
using Domain.CommonData;
using Infrastructure.ExtService;
namespace CaptureManage.AppWin
{
    public partial class PicpUpWebHtmlFrm : Form
    {
        public PicpUpWebHtmlFrm()
        {
            InitializeComponent();
        }
        int Page = -1;
        private void PicpUpWebHtmlFrm_Load(object sender, EventArgs e)
        {
            string url = "https://list.tmall.com/search_product.htm?spm=a220m.1000858.1000724.10.7f5f72ac0cvkWI&s=60&q=%D3%F0%C8%DE%B7%FE%C4%D0&sort=s&style=g&smAreaId=110106&type=pc";
            DrawWebBrowserInFromEle web = new DrawWebBrowserInFromEle(htmlPanel, QueryHtmlData, url);
        }
        private void QueryHtmlData(object response)
        {
            HtmlItem html = response as HtmlItem;
            if (html == null)
            {
                return;
            }
            LoggerWriter.CreateLogFile(html.Html, NowAppDirHelper.GetNowAppDir(AppCategory.WinApp) +"/"+ ELogType.HttpResponse.ToString()+"/"+html.Domain, ELogType.HttpResponse);
        }
    }
}
