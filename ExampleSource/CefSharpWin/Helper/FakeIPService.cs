﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
using HttpClientHelper;
namespace CefSharpWin
{
    public class FakeIPService
    {
        public static void SwitcHttphPrxoy()
        {
            try
            {
                string url = "http://www.iphai.com/free/ng";
                string text = HttpHelper.GetProxyResponse(url, "211.159.171.58", 80);
                text.WriteLogForEverDay(ELogType.HeartBeatLine);
            }
            catch (Exception ex)
            {
                ex.ToString().WriteLogForEverDay(ELogType.ErrorLog);
            }
        }
        public static void GetFakeIPs()
        {
            try
            {
                string ipDataSource = SystemSetting.SystemSettingDict["FakeIPAPISwitchInPool"];
                string url = SystemSetting.SystemSettingDict[ipDataSource];//选择使用什么ip代理项
                string ipPool = HttpClientHelper.HttpClientExtend.HttpClientGet(url);
                if (!string.IsNullOrEmpty(ipPool))
                {
                    ipPool.WriteLogForEverDay(ELogType.HeartBeatLine);
   //进行分离提取
                    string reg = "<tbody>(.*?)</tbody>";
                    string xmlReg = "<tbody>(.*?)</tbody>";

  //进行代理ip数据分析
                    SplitIPs(ipPool, SystemSetting.SystemSettingDict["IPsProxyRegex"]);

				 }
            }
            catch (Exception ex)
            {
                ex.ToString().WriteLogForEverDay(ELogType.ErrorLog);
            }
        }
        private static void SplitIPs(string input,string pattern)
        {
            RegexHelper.GetMatchValue(input, pattern); 
        }
    }
    public class ProxyIP
    {
        public string IP { get; set; }
        public string Port { get; set; }
        public long Id { get; set; }        
		public string Cryptonym { get; set; }
        public string IPHttpType { get; set; }//http https
        public string IPAddress { get; set; }
        public string IPResponseSpleed { get; set; }
        public string IPPoolUpdateTimeDesc { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime DownloadTime { get; set; }
    }
      
}
