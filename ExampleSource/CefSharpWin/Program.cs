﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                List<string> ipsData= RegexHelper.GetMatchValue(pool, regexIp);
                // 列名：   <th>IP</th>                                    <th>端口号</th>                                    <th>匿名度</th>                                    <th>IP类型</th>                                    <th>位置</th>                                    <th>响应速度</th>                                    <th>更新时间</th> 
                //行数据：  <td>                            222.88.149.32                        </td>                                            <td>                            8060                        </td>                                            <td>                            高匿                        </td>                                            <td>                            HTTP                        </td>                                            <td>                            中国河南安阳                        </td>                                            <td>                            0.15s                        </td>                                            <td>                            43分钟前                        </td> 
            }
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
    }
}
