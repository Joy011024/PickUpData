using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
namespace CefSharpWin
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            InitSQLiteManage.QueryCityDataFromSQLite();
            XmlService.GetAppSetting();

            TestRegex();
            InitFakeServices();
            InitRegisterForm();
            Form acc = FacadeFactory.Instance.RetrieveMediator(typeof(WebFrm).Name) as Form;

            Application.Run(acc);//cef 只能单进程

            /*
             System.Exception:“CEF can only be initialized once per process. This is a limitation of the underlying CEF/Chromium framework.
             You can change many (not all) settings at runtime through RequestContext.SetPreference. 
             See https://github.com/cefsharp/CefSharp/wiki/General-Usage#request-context-browser-isolation Use Cef.IsInitialized to guard against this exception. 
             If you are seeing this unexpectedly then you are likely calling Cef.Initialize after you've created an instance of ChromiumWebBrowser,
             it must be before the first instance is created.”

             */
        }
        static void TestRegex()
        {
            string file = Domain.CommonData.FileHelper.ReadFile("Dev\\IPProxyTemplate.txt");
            file = file.Replace("\r\n", string.Empty);
            string reg = SystemSetting.SystemSettingDict["IPsProxyRegex"];//xml中对于转义字符\如何声明 
            //  table-hover\">(.*?)</table
            //<tr>(.*?)</tr
            // reg = "table-hover\">(.*?)</table>";
            List<string> ips = RegexHelper.GetMatchValue(file, reg); //找出全部的列表
            if (ips.Count > 0)
            {
                string pool = ips[0].Trim();
                //  <tr>                                    <th>IP</th>                                    <th>端口号</th>                                    <th>匿名度</th>                                    <th>IP类型</th>                                    <th>位置</th>                                    <th>响应速度</th>                                    <th>更新时间</th>                            </tr>  
                string regexIp = "<tr>(.*?)</tr>";
                List<string> ipsData = RegexHelper.GetMatchValue(pool, regexIp);
                // 列名：   <th>IP</th>                                    <th>端口号</th>                                    <th>匿名度</th>                                    <th>IP类型</th>                                    <th>位置</th>                                    <th>响应速度</th>                                    <th>更新时间</th> 
                //行数据：  <td>                            222.88.149.32                        </td>                                            <td>                            8060                        </td>                                            <td>                            高匿                        </td>                                            <td>                            HTTP                        </td>                                            <td>                            中国河南安阳                        </td>                                            <td>                            0.15s                        </td>                                            <td>                            43分钟前                        </td> 
                string rowOrder = SystemSetting.SystemSettingDict["IPPoolMapData"];
                //此处进行正则匹配，然后将数据串安装排序的规则填充到实体中
                List<ProxyIP> result = RowFillData(rowOrder, ipsData.ToArray());//提取到的IP集合
                InitSQLiteManage db = new InitSQLiteManage();
                db.Inserts(result);
            }
        }
        static List<ProxyIP> RowFillData(string rowOrderFormat, string[] rows)
        {
            ProxyIP ip = new ProxyIP();
            string[] columnsMap = rowOrderFormat.Split('|');//这是每一行匹配的列【当列不需要处理是则设置=后面的值为空即可】
            Dictionary<string, string> headMapProperty = new Dictionary<string, string>();//字符串中行的排序
            foreach (string head in columnsMap)
            {
                string[] property = head.Split('=');
                if (property.Length >= 2 && !string.IsNullOrEmpty(property[1]))
                {
                    headMapProperty.Add(property[0], property[1]);
                }

            }
            string headRex = SystemSetting.SystemSettingDict["ColumnMapInHead"]; //进行正则提取
            List<string> heads = new List<string>();
            if (rows.Length > 0)
            {
                heads = RegexHelper.GetMatchValue(rows[0].Trim(), headRex);
            }
            List<string> datas = new List<string>();
            DateTime now = DateTime.Now;
            string csFormat = SystemSetting.SystemSettingDict["ValueMapInHead"];///数据正则提取
            List<ProxyIP> ips = new List<ProxyIP>();
            for (int i = 1; i < rows.Length; i++)
            {
                datas = RegexHelper.GetMatchValue(rows[i].Trim(), csFormat);
                //转换为实体
                ProxyIP pi = new ProxyIP()
                {
                    DownloadTime = now,
                    CreateTime = now
                };
                for (int c = 0; c < datas.Count; c++)
                {
                    string name = heads[c];//这是那一列=>转换到属性
                    if (headMapProperty.ContainsKey(name))
                    {
                        string property = headMapProperty[name];
                        SetEntityValue(pi, property, datas[c]); //填充属性
                    }
                }
                ips.Add(pi);
            }
            return ips;
        }
        static void InitRegisterForm()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            WebFrm acc = new WebFrm();
            MoniterTicket mt = new MoniterTicket();
        }
        static void InitFakeServices()
        {
            new System.Threading.Thread(() =>
            {
                try
                {
                    FakeIPService.SwitcHttphPrxoy();
                    FakeIPService.GetFakeIPs();
                }
                catch (Exception ex)
                {

                }
            }).Start();


        }
        static void SetEntityValue<T>(T data, string property, object value) where T : class
        {
            Type t = data.GetType();
            PropertyInfo pi = t.GetProperty(property);
            if (pi == null)
            {
                return;
            }
            pi.SetValue(data, value);

        }
    }
}
