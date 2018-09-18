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
        public static void GetFakeIPs()
        {
            string ipDataSource = SystemSetting.SystemSettingDict["FakeIPAPISwitchInPool"];
            string url = SystemSetting.SystemSettingDict[ipDataSource];//选择使用什么ip代理项
            string ipPool = HttpClientHelper.HttpClientExtend.HttpClientGet(url);
            if (!string.IsNullOrEmpty(ipPool))
            {
                ipPool.WriteLogForEverDay(ELogType.HeartBeatLine);
            }
        }
    }
      
}
