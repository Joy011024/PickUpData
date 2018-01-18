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
using System.Text.RegularExpressions;
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
            //提取索引页码
            //使用xpath <B class=ui-page-s-len>2/100</B>
            Regex reg = new Regex("<B[^>]*>(.+)</B>");//提取符合要求的一段文本
            GroupCollection gc = reg.Match(html.Html).Groups;
            if (gc.Count > 1)
            {//第一项为匹配的完整串，第二项为标签内的文本  [2/100]
                string htmlEle = gc[0].Value;//提取匹配的标签
                Group g = gc[1];
                string value = g.Value;//提取标签内的内容
            }
            /*
             js: /<B[^>]*>(.+)<\/B>/
             */
        }
        void HtmlAnalisy() 
        {//html解析
        
        }
    }
}
