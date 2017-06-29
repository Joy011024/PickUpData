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
                DateTime now = DateTime.Now;
                foreach (FindQQDataTable item in data)
                {
                    item.ID = Guid.NewGuid();
                    item.CreateTime = now;
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
            DateTime today = DateTime.Now;
            string sp = string.Format("exec SP_PickUpStaticWithDay '{0}'", today);
            MainRespority<FindQQDataTable> main = new MainRespority<FindQQDataTable>(ConnString);
            List<PickUpStatic> staticData=main.ExecuteSPSelect<PickUpStatic>(sp,null).ToList();
            return staticData.FirstOrDefault();
        }
        
    }
}
