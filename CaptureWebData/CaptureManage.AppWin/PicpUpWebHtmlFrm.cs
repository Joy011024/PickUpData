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
using System.IO;
using TicketData.Model;
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
    public class PickUpTianMaoHtml
    {
        public void DoHtmlFileAnalysis(string file)
        {
            DirectoryInfo di = new DirectoryInfo(file);
            string fileName = di.Name;//这是文件名称
            string dir = di.Parent.FullName;//该文件的目录
            string newDir = dir + ".Read/";
            if (!Directory.Exists(newDir))
            {
                Directory.CreateDirectory(newDir);
            }
            //读取文件
            string html= FileHelper.ReadFile(file);
            if (string.IsNullOrEmpty(html))
            {
                return;
            }
            //进行文件HTML分析
            List<TianmaoGood> goods = new List<TianmaoGood>();
            GetGoodList(html, goods);
            //文件移动到已读库
            //File.Move(file, newDir + fileName);
        }
        public void GetGoodList(string html, List<TianmaoGood> outResult) 
        {
            //html = html.Replace("><", ">\r\n<").Replace("> <", ">\r\n<");
            Regex reg = new Regex("<DIV class=product-iWrap>(.*?)<DIV class=\"product");
            //new Regex("<P class=productTitle>(.+)</P>");
            MatchCollection mc= reg.Matches(html);//获取商品列表
            /*
             https://github.com/zzzprojects/html-agility-pack/tree/master/src  正则表达式提取HTML
             */
            foreach (Match item in mc)
            { //这是一条完整的记录
                if (item.Groups.Count <= 1)
                {
                    continue;
                }
                TianmaoGood good = new TianmaoGood();
                Group g = item.Groups[1];
                string goods = g.Value;
                //提取商品图片已经价格数据信息
                string hrefStr= GetHtmlEleValue(goods, "<A class=productImg(.*?)</A>");//提取商品图片
                string regHref = "href=\"(.*?)\" target";
                //货物链接
                good .GoodHref= GetHtmlEleValue(hrefStr, regHref);
                //货物照片
                good.productImg = GetHtmlEleValue(goods, "<IMG src=\"(.*?)\">");
               string price = GetHtmlEleValue(goods, "<EM title=(.*?)>");
               if (!string.IsNullOrEmpty(price))
               {
                   good.GoodPrice = float.Parse(price);
               }
               string unit = GetHtmlEleValue(goods, "<B>(.*?)</B>");
               if (!string.IsNullOrEmpty(unit)) 
               {
                   good.PriceUnit = unit[0];
               }
               string title = GetHtmlEleValue(goods, "<P class=productTitle><A title=(.*?)</P>");
                /*
                 ONLY2017冬季新品宽松V领套头毛衣针织衫女|117324537 
                 * href="//detail.tmall.com/item.htm?id=557951889087&amp;skuId=3464513665788&amp;user_id=356060330&amp;cat_id=2&amp;is_b=1&amp;rn=c316941c37ea277eb8b7f2cc36209dd1" 
                 * target=_blank data-p="1-11">ONLY2017冬季新品宽松V领套头<SPAN class=H>毛衣</SPAN>针织衫女|117324537 </A>
                 */
            }
            //如果该数据串中还含有商品列表分析格式在使用递归分析

        }
        string  GetHtmlEleValue(string rowHtml, string ruleRegex)
        {
            Regex regGood = new Regex(ruleRegex);//这是图片
            MatchCollection mc = regGood.Matches(rowHtml);
            foreach (Match item in mc)
            { //这是一条完整的记录
                if (item.Groups.Count <= 1)
                {
                    continue;
                }
                Group g = item.Groups[1];
                return g.Value.Trim();
            }
            return string.Empty;
        }
    }
}
