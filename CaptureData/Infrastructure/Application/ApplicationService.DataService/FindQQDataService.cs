using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Domain.CommonData;
using Infrastructure.EFMsSQL;
namespace CaptureWebData
{
    public class FindQQDataService
    {
        public string ConnString { get; set; }
        public FindQQDataService(string connString) 
        {
            ConnString = connString;
        }
        public void SaveFindQQ( List<FindQQDataTable> data) 
        {
            try 
            {
                //此处功能修正：使用SQL 

                DateTime now = DateTime.Now;
                foreach (FindQQDataTable item in data)
                {
                    item.ID = Guid.NewGuid();
                    item.CreateTime = now;
                    if (string.IsNullOrEmpty(item.Url))//没有采集到该账户的头像数据
                        item.ImgType = -1;
                    item.DayInt = int.Parse(now.ToString("yyyyMMdd"));
                }
                MainRespority<FindQQDataTable> mr = new MainRespority<FindQQDataTable>(ConnString);
                mr.InsertList(data);

            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// 统计当前今天统计数目
        /// </summary>
        public int CountTodayPickUp() 
        {
            TodayStaticData();
            string sp = "exec SP_StaticCountToday";
            MainRespority<FindQQDataTable> main = new MainRespority<FindQQDataTable>(ConnString);
            List<int> ret= main.ExecuteSPSelect<int>(sp, null).ToList();
            if (ret.Any()) 
            {
                return ret[0];
            }
            return 0;
        }
        public PickUpStatic TodayStaticData() 
        {
            try
            {
                DateTime today = DateTime.Now;
                string sp = string.Format("exec SP_PickUpStaticWithDay '{0}'", today);
                MainRespority<FindQQDataTable> main = new MainRespority<FindQQDataTable>(ConnString);
                //其他信息: Timeout 时间已到。在操作完成之前超时时间已过或服务器未响应。
                List<PickUpStatic> staticData = main.ExecuteSPSelect<PickUpStatic>(sp, null).ToList();
                return staticData.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string logPath = (new AssemblyDataExt()).GetAssemblyDir()+"\\"+ELogType.ErrorLog; // new ConfigurationItems().LogPath + GeneratePathTimeSpan(cookie);
                LoggerWriter.CreateLogFile(ex.Message, logPath, ELogType.ErrorLog);
                return new PickUpStatic();
            }
        }
        
    }
}
