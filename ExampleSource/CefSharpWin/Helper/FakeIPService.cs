using System;
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
                }
            }
            catch (Exception ex)
            {
                ex.ToString().WriteLogForEverDay(ELogType.ErrorLog);
            }
        }
    }
      
}
