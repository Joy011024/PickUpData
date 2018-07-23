using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataHelp;
using Domain.CommonData;
using ApplicationService;
namespace CaptureWebData
{
    public class FindQQDataManage
    {
        public string ConnString { get; set; }
        public FindQQDataManage(string connString) 
        {
            ConnString = connString;
        }
        public FindQQResponse SaveFindQQ(string findQQResponseJson) 
        {
            try 
            {
                FindQQResponse find = findQQResponseJson.ConvertObject<FindQQResponse>();
                if (SystemConfig.UsingDB)
                {//启用数据库功能
                    List<FindQQ> qqs = find.result.buddy.info_list;
                    List<FindQQDataTable> table = qqs.Select(s => s.ConvertMapModel<FindQQ, FindQQDataTable>(true)).ToList();
                    FindQQDataService service = new FindQQDataService(ConnString);
                    service.SaveFindQQ(table);
                }
                return find;
            }
            catch (Exception ex) 
            {
                return null;
            }
        }
        public int CountTodayPickUp() 
        {
           return new FindQQDataService(ConnString).CountTodayPickUp();
        }
        public PickUpStatic TodayStatic() 
        {
            return new FindQQDataService(ConnString).TodayStaticData();
        }
        public void GatherHeadImage(string uin,string imgDir) 
        {
            
        }
    }
    
}
