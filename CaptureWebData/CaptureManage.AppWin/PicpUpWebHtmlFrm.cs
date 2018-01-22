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
        DrawWebBrowserInFromEle web;
        private void PicpUpWebHtmlFrm_Load(object sender, EventArgs e)
        {
            string url = "https://list.tmall.com/search_product.htm?spm=a220m.1000858.1000724.10.4f506713HbGUt4&s=1&q=%C3%AB%D2%C2&sort=s&style=g&from=mallfp..pc_1_searchbutton&active=2&smAreaId=110106&type=pc#J_Filter";
               //羽绒服 "https://list.tmall.com/search_product.htm?spm=a220m.1000858.1000724.10.7f5f72ac0cvkWI&s=60&q=%D3%F0%C8%DE%B7%FE%C4%D0&sort=s&style=g&smAreaId=110106&type=pc";
            web = new DrawWebBrowserInFromEle(htmlPanel, QueryHtmlData, url);
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
            Regex reg = new Regex("<B class=ui-page-s-len>(.+)</B>");//提取符合要求的一段文本
            GroupCollection gc = reg.Match(html.Html).Groups;
            if (gc.Count <=1)
            {//第一项为匹配的完整串，第二项为标签内的文本  [2/100]
                return;
            }
            string htmlEle = gc[0].Value;//提取匹配的标签
            Group g = gc[1];
            string value = g.Value;//提取标签内的内容
            string[] pageInfo = value.Split('/');
            if (pageInfo.Length < 2)
            {
                return;
            }
            int numerator = -1, denominator = -1;
            int.TryParse(pageInfo[0], out numerator);
            if (!int.TryParse(pageInfo[1], out denominator))
            {//转换失败时 设置 参数为-1 避免出现分母为0的情形
                denominator = -1;
            }
            if (numerator >= denominator)
            { //不是查询到尾页
                return;
            }
            numerator++;
            string url = "https://list.tmall.com/search_product.htm?spm=a220m.1000858.1000724.10.4f506713HbGUt4&s={page}&q=%C3%AB%D2%C2&sort=s&style=g&from=mallfp..pc_1_searchbutton&active=2&smAreaId=110106&type=pc#J_Filter";
                // 羽绒服 "https://list.tmall.com/search_product.htm?spm=a220m.1000858.1000724.10.7f5f72ac0cvkWI&s={page}&q=%D3%F0%C8%DE%B7%FE%C4%D0&sort=s&style=g&smAreaId=110106&type=pc";
            int start = numerator * 60;
            url = url.Replace("{page}", start.ToString());
            web.RefreshUrl(url);
            /*
             js: /<B[^>]*>(.+)<\/B>/
             */
        }
        void HtmlAnalisy() 
        {//html解析
        
        }
    }
}
