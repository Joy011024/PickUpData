using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DataHelp;
using Domain.CommonData;
using ApplicationService.IPDataService;

namespace CaptureWebData
{
    public partial class WebDataFrm : Form
    {
        public int pageList = 200;
        public int pageIndex = 1;
        public int nowIndex = 0;
        int waitTimeForPickUpIpAddress = 10 * 1000;
        List<VIpAddress> foreachIP { get; set; }
        List<QQZengResponseData> waitSave = new List<QQZengResponseData>();
        int totalIp = 0;
        public WebDataFrm()
        {
            InitializeComponent();
            InitUrl();
        }
        void InitUrl() 
        {
            RefreshWeb();
            InitIpNumber();
            QueryIpSrc();
        }
        void RefreshWeb() 
        {
            string url = rtbUrl.Text;
            webBrowser.Url = new Uri(url);
        }
        private void btnLoading_Click(object sender, EventArgs e)
        {
           
            if (foreachIP == null && foreachIP.Count == 0)
            {
                rtbTip.Text = "没有找出可用IP库数据记录";
                QueryIpSrc();
            }
            VIpAddress ip = foreachIP[nowIndex];
            txtEleValue.Text = ip.StartIPText;
            nowIndex++;
            if (nowIndex > foreachIP.Count - 1)
            {
                nowIndex = 0;
            }
        }

        private void btnPickUp_Click(object sender, EventArgs e)
        {
            LoadCompate();
            if (waitSave.Count==10)
            {
                EmportIpAddress(waitSave);
            }
        }
        void LoadCompate() 
        {
            string text = webBrowser.DocumentText;
            rtbHtml.Text = text;
            HtmlElementCollection eles = webBrowser.Document.GetElementsByTagName("tr");
            foreach (HtmlElement item in eles)
            {
                string html = item.OuterHtml;
                if (html.Contains("ip_info"))
                {
                    rtbHtmlData.Text += "\r\n" + html;
                }
            } 
            QQZengResponseData qqzeng = rtbHtmlData.Text.HtmlConvertIpData();
            HtmlElement ipe = webBrowser.Document.GetElementById("obviousIp");
            if (ipe != null)
            {
                string iphtml = ipe.OuterHtml;
                qqzeng.ip = iphtml.Split(new string[] { ">" }, StringSplitOptions.None)[1].Split(new string[] { "<" }, StringSplitOptions.None)[0];
            }
            string json= qqzeng.ConvertJson();
            rtbTip.Text = json;
            if (!string.IsNullOrEmpty(qqzeng.ip)&& !waitSave.Any(p => p.ip == qqzeng.ip)) 
            {
                waitSave.Add(qqzeng);
            }
           
        }
        private void btnSetHtmlEle_Click(object sender, EventArgs e)
        {
            string ele = txtHtmlEle.Text;
            string value = txtEleValue.Text;
            HtmlElement eleHtml= webBrowser.Document.GetElementById(ele);
            eleHtml.SetAttribute("value", value);
            HtmlElement btn = webBrowser.Document.GetElementById("ipSearch");//自动点击
            Thread.Sleep(1000 * 5);
            btn.InvokeMember("click");//自动生成点击
            Thread.Sleep(waitTimeForPickUpIpAddress);//等待若干秒之后开始提取IP地址数据
            LoadCompate();
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {//加载完毕
            LoadCompate();
        }

        private void WebDataFrm_Load(object sender, EventArgs e)
        {

        }

        private void btnClearText_Click(object sender, EventArgs e)
        {
            ClearText();
        }
        void ClearText() 
        {
            rtbHtml.Text = string.Empty;
            rtbHtmlData.Text = string.Empty;
        }
        void QueryIpSrc() 
        {
            int start = (pageIndex - 1) * pageList + 1;
            int end = pageIndex * pageList;
            if (start >= totalIp) 
            {
                rtbTip.Text = "已执行到最后一条";
                return;
            }
            ConfigurationItems config=new ConfigurationItems();
            VIpAddressService ipservice = new VIpAddressService(config.IpDataSrc);
           
            foreachIP= ipservice.GetIpData(start, end);
            pageIndex++;
        }
        void InitIpNumber() 
        {
            ConfigurationItems config = new ConfigurationItems();
            VIpAddressService ipservice = new VIpAddressService(config.IpDataSrc);
            totalIp = ipservice.Count();
        }
        void  EmportIpAddress(List<QQZengResponseData> list) 
        {
            List<IpDataMapTable> target = list.Select(qqz => qqz.ConvertMapModel<QQZengResponseData, IpDataMapTable>(true))
                .Where(ip => { ip.InitData(); return true; }).ToList();
            ConfigurationItems config = new ConfigurationItems();
            IPService ps = new IPService(config.SaveIpAddressConnString);
            bool succ = ps.SaveList(target); ;
            if (succ) 
            {
                list = new List<QQZengResponseData>();
            }
        }

        private void btnForceSave_Click(object sender, EventArgs e)
        {
            EmportIpAddress(waitSave);
        }

        private void btnPickUpCookie_Click(object sender, EventArgs e)
        {
            if (webBrowser.Document == null)
            {
                rtbTip.Text = "网页加载失败不能提取cookie";
                return;
            }
            string cookie = webBrowser.Document.Cookie;
            rtbTip.Text = cookie;
            string ip = foreachIP[nowIndex].StartIPText;
            QQZengIpDataHelper.QueryIpAddress(ip, cookie);
        }

        private void btnRefreshWeb_Click(object sender, EventArgs e)
        {
            //SHDocVw//Interop.SHDocVw.dll 文件来自SHDocVw.dll
            RefreshWeb();
        }
    }
}
