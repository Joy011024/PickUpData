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
            lstData.Items.Add(html.Url);
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

        private void btnClearText_Click(object sender, EventArgs e)
        {
            lstData.Items.Clear();
            rtbHtml.Text = string.Empty;
        }

        private void lstData_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lst = sender as ListBox;
            object obj = lst.SelectedValue;
            if (obj != null)
            {
                txtSelectUrl.Text = obj.ToString();
            }
        }
    }
    public enum EFileStatue
    { 
        WaitRead=1,
        Readed=2,
        Reading=3,
        Error=4
    }
    public class PickUpTianMaoHtml
    {
        [Description("根据提供的目录查找文件进行天猫商品分析")]
        public void DoHtmlFileAnalysis(string diskDir)
        {
            //获取目录下的文件列表
            if (!Directory.Exists(diskDir))
            {
                Directory.CreateDirectory(diskDir);
                return;
            }
            DirectoryInfo di = new DirectoryInfo(diskDir);//
            string fileFormat="*.txt";
            int fileCount= di.EnumerateFiles(fileFormat).Count();
            if (fileCount < 1)
            {//没有文件
                return;
            }
            DateTime first= di.EnumerateFiles(fileFormat).Min(s => s.CreationTime);//最先创建的文件
            string file = di.EnumerateFiles(fileFormat).Where(s => s.CreationTime == first).Select(s => s.FullName).FirstOrDefault();
            //根据提供的路径查找最先创建的文本
            di = new DirectoryInfo(file);//提取该文件的目录及文件信息
            string fileName = di.Name;//这是文件名称
            string dir = di.Parent.FullName;//该文件的目录
            try
            {
                string newDir = dir + "." + EFileStatue.Readed.ToString() + "/";
                if (!Directory.Exists(newDir))
                {
                    Directory.CreateDirectory(newDir);
                }
                //读取文件
                string html = FileHelper.ReadFile(file);
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
            catch (Exception ex)
            {
                string newDir = dir + "." + EFileStatue.Error.ToString() + "/";
                File.Move(file, newDir + fileName);
            }
        }
        public void GetGoodList(string html, List<TianmaoGood> outResult)
        {
            //html = html.Replace("><", ">\r\n<").Replace("> <", ">\r\n<");
            Regex reg = new Regex("<DIV class=product-iWrap>(.*?)<DIV class=\"product");
            //new Regex("<P class=productTitle>(.+)</P>");
            MatchCollection mc = reg.Matches(html);//获取商品列表
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
                string hrefStr = GetHtmlEleValue(goods, "<A class=productImg(.*?)</A>");//提取商品图片
                string regHref = "href=\"(.*?)\" target";
                //货物链接
                good.GoodHref = GetHtmlEleValue(hrefStr, regHref);
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
                
                /*
                 "<DIV class=productImg-wrap><A class=productImg href=\"//detail.tmall.com/item.htm?id=557951889087&amp;skuId=3464513665788&amp;user_id=356060330&amp;cat_id=2&amp;is_b=1&amp;rn=c316941c37ea277eb8b7f2cc36209dd1\" target=_blank data-p=\"1-10\"><IMG src=\"//img.alicdn.com/bao/uploaded/i3/356060330/TB1SYhna.tWMKJjy0FaXXcCDpXa_!!0-item_pic.jpg_b.jpg\"> </A></DIV><P class=productPrice><EM title=199.50><B>¥</B>199.50</EM> </P><P class=productTitle><A title=ONLY2017冬季新品宽松V领套头毛衣针织衫女|117324537 href=\"//detail.tmall.com/item.htm?id=557951889087&amp;skuId=3464513665788&amp;user_id=356060330&amp;cat_id=2&amp;is_b=1&amp;rn=c316941c37ea277eb8b7f2cc36209dd1\" target=_blank data-p=\"1-11\">ONLY2017冬季新品宽松V领套头<SPAN class=H>毛衣</SPAN>针织衫女|117324537 </A></P><DIV class=productShop data-atp=\"b!1-3,{user_id},,,,,,\"><A class=productShop-name href=\"//store.taobao.com/search.htm?user_number_id=356060330&amp;rn=c316941c37ea277eb8b7f2cc36209dd1&amp;keyword=毛衣\" target=_blank>ONLY官方旗舰店 </A></DIV><P class=productStatus><SPAN>月成交 <EM>967笔</
    EM></SPAN> <SPAN>评价 <A href=\"//detail.tmall.com/item.htm?id=557951889087&amp;skuId=3464513665788&amp;user_id=356060330&amp;cat_id=2&amp;is_b=1&amp;rn=c316941c37ea277eb8b7f2cc36209dd1&amp;on_comment=1#J_TabBar\" target=_blank data-p=\"1-1\">573</A></SPAN> <SPAN class=\"ww-light ww-small m_wangwang J_WangWang\" data-atp=\"a!1-2,,,,,,,356060330\" data-display=\"inline\" data-tnick=\"only官方旗舰店\" data-nick=\"only官方旗舰店\" data-item=\"557951889087\" data-icon=\"small\"></SPAN></P></DIV></DIV>"

                 */
                string titleStr = GetHtmlEleValue(goods, "<P class=productTitle><A title=(.*?)href=");
                string title = GetHtmlEleValue(goods, "<P class=productTitle><A title=(.*?)</A></P>");
                string goodName = GetHtmlEleValue(goods, "data-p=\"1-11\">(.*?)<SPAN class=H>");
                string curstom = GetHtmlEleValue(goods, "<P class=productTitle>(.*?)</P>");
                string goodShopData = GetHtmlEleValue(goods, "<DIV class=productShop(.*?)</DIV><P class=productStatus>");
                //提取店铺名称
                string goodShopName = GetHtmlEleValue(goodShopData, "target=_blank>(.*?)</A>");
                good.productShop = goodShopName;
                string goodShopLink = GetHtmlEleValue(goods, "<A class=productShop-name href=\"(.*?)target=_blank");
                string ignoreSign = "\"";
                if (!string.IsNullOrEmpty(goodShopLink) && goodShopLink.LastIndexOf(ignoreSign)==goodShopLink.Length-ignoreSign.Length)
                {
                    goodShopLink = goodShopLink.Substring(0, goodShopLink.Length - ignoreSign.Length);
                }
                good.ShopLink = goodShopLink;

                good.SetNormalHttpUrl();//对于不规范的http进行规范化
                //<A class=productShop-name href=\"(.*?)\" target=_blank>
                //string groups=GetHtmlEleValue(goods,"target=_blank data-p=\"1-11\">(.*?)<SPAN class=H>(.*?)</SPAN>(.*?)|(.*?)</A>");
                /*
                 ONLY2017冬季新品宽松V领套头毛衣针织衫女|117324537 
                 * href="//detail.tmall.com/item.htm?id=557951889087&amp;skuId=3464513665788&amp;user_id=356060330&amp;cat_id=2&amp;is_b=1&amp;rn=c316941c37ea277eb8b7f2cc36209dd1" 
                 * target=_blank data-p="1-11">ONLY2017冬季新品宽松V领套头<SPAN class=H>毛衣</SPAN>针织衫女|117324537 </A>
                 */
                /*
                 
                 <DIV class=productImg-wrap><A class=productImg href="//detail.tmall.com/item.htm?id=559884497567&amp;skuId=3659469818510&amp;areaId=110100&amp;user_id=528811819&amp;cat_id=2&amp;is_b=1&amp;rn=7879cf8d2912d25bce4e22d6793715bf" target=_blank data-p="1-10"><IMG src="//img.alicdn.com/bao/uploaded/i3/528811819/TB1biHaaZnI8KJjSsziXXb8QpXa_!!0-item_pic.jpg_b.jpg"> </A></DIV><P class=productPrice><EM title=128.00><B>¥</B>128.00</EM> </P><P class=productTitle><A title=南极人爸爸冬装中年男士假两件男装毛衣中老年加绒加厚保暖针织衫 href="//detail.tmall.com/item.htm?id=559884497567&amp;skuId=3659469818510&amp;areaId=110100&amp;user_id=528811819&amp;cat_id=2&amp;is_b=1&amp;rn=7879cf8d2912d25bce4e22d6793715bf" target=_blank data-p="1-11">南极人中年男士中老年加绒加厚毛衣 </A></P><DIV class=productShop data-atp="b!1-3,{user_id},,,,,,"><A class=productShop-name href="//store.taobao.com/search.htm?user_number_id=528811819&amp;rn=7879cf8d2912d25bce4e22d6793715bf&amp;keyword=毛衣" target=_blank>兰子服饰专营店 </A></DIV><P class=productStatus><SPAN>月成交 <EM>6526笔</EM></SPAN> <SPAN>评价 <A href="//detail.tmall.com/item.htm?id=559884497567&amp;skuId=3659469818510&amp;areaId=110100&amp;user_id=528811819&amp;cat_id=2&amp;is_b=1&amp;rn=7879cf8d2912d25bce4e22d6793715bf&amp;on_comment=1#J_TabBar" target=_blank data-p="1-1">5806</A></SPAN> <SPAN class="ww-light ww-small m_wangwang J_WangWang" data-atp="a!1-2,,,,,,,528811819" data-display="inline" data-tnick="兰子服饰专营店" data-nick="兰子服饰专营店" data-item="559884497567" data-icon="small"></SPAN></P></DIV></DIV>
                 * 
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
