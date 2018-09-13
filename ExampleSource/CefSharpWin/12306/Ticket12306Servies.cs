using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CommonData;
namespace CefSharpWin
{
    public class Ticket12306Servies
    {
        public void GroupStation()
        {
            string file = FileHelper.ReadFile(SystemConfig.DebugDir + SystemSetting.SystemSettingDict["StationJsonFile"]);
            List<string> stationList= RegexHelper.GetMatchValue(file, SystemSetting.SystemSettingDict["StationUrlRegexFormat"]);
            List<string> coll = RegexHelper.GetMatchValue(stationList[0], @"(?<test>@[\w|-]*)");//  SystemSetting.SystemSettingDict["StationSplitRegex"]);
            if (string.IsNullOrEmpty(coll[0]))
            {
                coll.RemoveAt(0);
            }
            int len = int.Parse(SystemSetting.SystemSettingDict["StationFieldNum"]);
            string.Join("\r\n", coll).WriteLog(ELogType.DebugData, false);

        }
    }
}
