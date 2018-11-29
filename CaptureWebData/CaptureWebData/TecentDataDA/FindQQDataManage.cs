using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataHelp;
using Domain.CommonData;
using ApplicationService.DataService;
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
                if (SystemConfig.UsingDBSaveBaseData)
                {//启用数据库功能
                    List<FindQQ> qqs = find.result.buddy.info_list;
                    List<FindQQDataTable> table = qqs.Select(s => s.ConvertMapModel<FindQQ, FindQQDataTable>(true)).ToList();
                   // FindQQDataService service = new FindQQDataService(ConnString);
                   // service.SaveFindQQ(table);
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
            return 0;// new FindQQDataService(ConnString).CountTodayPickUp();
        }
        public PickUpStatic TodayStatic() 
        {
            return new PickUpStatic();// new FindQQDataService(ConnString).TodayStaticData();
        }
        public void GatherHeadImage(string uin,string imgDir) 
        {
            
        }
    }

    public class DataFromManage
    {
        //进行数据库的适配
        public void CityData()
        {
            //当前数据库
            switch (SystemConfig.MainDBType)
            {
                case DBType.SQLite:
                    break;
                case DBType.MySQL:
                    break;
                case DBType.SQLServer:
                    CategoryDataService cs = new CategoryDataService(new ConfigurationItems().TecentDA);
                    break;
            }

        }
    }
    public class DBType
    {
        public const string SQLite = "SQLite";
        public const string SQLServer = "SQLServer";
        public const string MySQL = "MySQL";
    }
    
}
