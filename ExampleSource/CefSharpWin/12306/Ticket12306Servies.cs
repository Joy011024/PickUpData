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
        public void GroupStation(string stationString)
        {
            string file = SystemConfig.DebugDir + SystemSetting.SystemSettingDict["StationJsonFile"];
            List<string> stationList= RegexHelper.GetMatchValue(stationString, SystemSetting.SystemSettingDict["StationUrlRegexFormat"]);
            List<string> coll = RegexHelper.GetMatchValue(stationList[0], @"(?<test>@[\w|-| ]*)");//  SystemSetting.SystemSettingDict["StationSplitRegex"]);
            if (string.IsNullOrEmpty(coll[0]))
            {
                coll.RemoveAt(0);
            }
            int len = int.Parse(SystemSetting.SystemSettingDict["StationFieldNum"]);
            string.Join("\r\n", coll).WriteLog(ELogType.DebugData, true);
            string flag = SystemSetting.SystemSettingDict["SplitFlag"];
            List<Station> station = new List<Station>();
            foreach (var item in coll)
            {
                string[] arr = item.Split(new string[] { flag }, StringSplitOptions.None);
                if (arr.Length < len)
                {
                    continue;
                }
                Station st = new Station();
                st.SetEntity(arr);
                station.Add(st);
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(station);
            if (!string.IsNullOrEmpty(json))
            {
                //FileHelper.ReplaceTxt(file, json);
            }
        }
        [System.ComponentModel.Description("从文件中获取车站列表")]
        public List<Station> GetStationFromFile()
        {
            try
            {
                string file = FileHelper.ReadFile(SystemConfig.DebugDir + SystemSetting.SystemSettingDict["StationJsonFile"]);
                if (string.IsNullOrEmpty(file))
                {
                    return new List<Station>();
                }
                List<Station> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Station>>(file);
                return data;
            }
            catch (Exception ex)
            {
                return new List<Station>();
            }
        }
    }
}
